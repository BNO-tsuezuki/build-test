using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Http.ProtocolModels.HandShake;

public class Host : MonoBehaviour
{
	public string Account;
	public string Token;
	public bool Online = false;

	Queue<System.Func<IEnumerator>> taskQue = new Queue<System.Func<IEnumerator>>();


	public int TaskCount
	{
		get { return taskQue.Count; }
	}


	public void AddTask(System.Func<IEnumerator> task)
	{
		taskQue.Enqueue(task);
	}

	Coroutine HandShakeCoroutine;
	public void StartHandShake()
	{
		HandShakeCoroutine = StartCoroutine(HandShakeTask());
	}
	public void StopHandShake()
	{
		if (HandShakeCoroutine != null)
		{
			StopCoroutine(HandShakeCoroutine);
			HandShakeCoroutine = null;
		}
	}
	public IEnumerator HandShakeTask()
	{
		while (true)
		{
			var handShakeBreak = true;

			var handShake = new Http.ProtocolModels.HandShake.HandShake();
			handShake.PostAction = (res) =>
			{
				Debug.Log(handShake.RawResponseText);

				var close = handShake.Remap<HandShake.Close>();
				if (close != null)
				{
					return;
				}
												
				var unauthorized = handShake.Remap<HandShake.Unauthorized>();
				if (unauthorized != null)
				{
					return;
				}

				do
				{
					var poke = handShake.Remap<HandShake.Poke>();
					if (poke != null)
					{
						break;
					}

				} while (true);

				handShakeBreak = false;
			};

			yield return handShake.RequestCoroutine(Token);

			if (handShakeBreak)
			{
				Debug.Log("==== Hand Shake Break ! ====");
				break;
			}

			Online = true;
		}

		Token = "";
		Online = false;
	}

	public IEnumerator Start()
	{
		while (true)
		{
			if (taskQue.Count == 0)
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}

			var task = taskQue.Peek();

			yield return task();

			taskQue.Dequeue();
		}
	}
}

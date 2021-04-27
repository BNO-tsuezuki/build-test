using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDisplay : SingletonMonoBehaviour<LogDisplay>
{
	public int maxLogCnt = 10;

	Queue<string> _logList = new Queue<string>();

	public void Push(string log)
	{
		if (log == null) log = "(null)";

		_logList.Enqueue(log);

		while( maxLogCnt < _logList.Count )
		{
			_logList.Dequeue();
		}
	}

	private void OnGUI()
	{
		foreach( var log in _logList )
		{
			GUILayout.Label(log);
		}
	}

}

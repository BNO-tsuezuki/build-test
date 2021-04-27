using UnityEngine;
using System.Collections;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _Instance = null;
	public static T Instance { get { return _Instance; } }
	
	void Awake()
	{
		if( Instance != null )
		{
			throw new Exception(name + " is already exist!!");
		}

		_Instance = gameObject.GetComponent<T>();
	}

	void OnDestroy()
	{
		if (_Instance == this)
		{
			_Instance = null;
		}
	}
}

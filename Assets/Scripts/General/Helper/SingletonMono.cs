using System;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	#region 单例

	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				T[] managers = GameObject.FindObjectsOfType (typeof (T)) as T[];
				if (managers.Length > 1)
				{
					Debug.LogError ("You have more than one " + typeof (T).Name + " in the scene. You only need 1, it's a singleton!");
					foreach (T manager in managers)
					{
						Component obj = manager as Component;
						Component.Destroy (obj.gameObject);
					}

					new GameObject (typeof (T).Name, typeof (T));
					managers = GameObject.FindObjectsOfType (typeof (T)) as T[];
				}
				else if (managers.Length == 0)
				{
					new GameObject (typeof (T).Name, typeof (T));
					managers = GameObject.FindObjectsOfType (typeof (T)) as T[];
				}

				if (managers.Length == 1)
				{
					instance = managers[0];
					if (Application.isPlaying)
					{
						MonoBehaviour.DontDestroyOnLoad (instance);
					}
				}
			}

			return instance;
		}
		set { instance = value; }
	}

	public static T Get()
	{
		return Instance;
	}
	#endregion

	void Awake ()
	{
		OnAwake ();
	}

	void Start ()
	{
		OnStart ();
	}

	public void Update ()
	{
		OnUpdate ();
	}

	private void LateUpdate ()
	{
		OnLateUpdate ();
	}

	void OnDestroy ()
	{
		BeforeOnDestroy ();
	}

	protected virtual void OnAwake ()
	{
	}

	protected virtual void OnStart ()
	{
	}

	protected virtual void OnUpdate ()
	{
	}

	protected virtual void OnLateUpdate ()
	{
	}
	protected virtual void BeforeOnDestroy ()
	{
	}
}
using UnityEngine;

/// <summary>
/// A generic class for creating singleton instances of a MonoBehaviour.
/// </summary>
/// <typeparam name="T">The type of the singleton instance.</typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				T[] objs = FindObjectsOfType<T>(true);
				if (objs.Length > 0)
				{
					T instance = objs[0];
					_instance = instance;
				}
				else
				{
					GameObject go = new GameObject();
					go.name = typeof(T).Name;
					_instance = go.AddComponent<T>();
				}
			}
			return _instance;
		}
	}
}

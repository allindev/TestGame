using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static bool PersistBetweenScenes = false;
    public static T Instance
    {

        get
        {
            if (_applicationIsQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<T>();
                    if (instances.Length > 0)
                    {
                        _instance = instances[0];
                        if (instances.Length > 1)
                        {
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i].gameObject);
                            }
                        }
                    }
                    else if (!_applicationIsQuitting)
                    {
                        GameObject singleton = new GameObject();
                        singleton.name = "[singleton]" + typeof(T).Name;

                        _instance = singleton.AddComponent<T>();
                    }

                    if (_instance != null & PersistBetweenScenes) DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }

    }


    public virtual void OnDestroy()
    {
        if (_instance == this)
        {
            //_applicationIsQuitting = true;
        }
    }



}
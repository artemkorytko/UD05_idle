using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BaseSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();


                    if (instance == null)
                    {
                        var obj = new GameObject("Singleton");
                        instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }

                return instance;
            }
        }
    }
}
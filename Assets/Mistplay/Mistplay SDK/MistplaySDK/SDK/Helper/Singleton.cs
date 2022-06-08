using UnityEngine;

namespace MistplaySDK
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        [SerializeField] bool dontDestroyOnLoad;
        string className;

        void Awake()
        {
            if(Instance) Destroy(gameObject);
            else
            {
                Instance = GetComponent<T>();
                className = this.GetType().Name;

                if(dontDestroyOnLoad)
                    GameObject.DontDestroyOnLoad(gameObject);

                OnAwake();
            }
        }

        protected virtual void OnAwake(){}

        protected void Log(object message) => Debug.Log($"<b>[{className}]</b> {message}", this);
        protected void Warning(object message) => Debug.LogWarning($"<b>[{className}]</b> {message}", this);
        protected void Error(object message) => Debug.LogError($"<b>[{className}]</b> {message}", this);
    }
}


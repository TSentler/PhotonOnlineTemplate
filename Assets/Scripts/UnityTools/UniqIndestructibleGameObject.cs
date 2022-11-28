using UnityEngine;

namespace UnityTools
{
    [DisallowMultipleComponent]
    public class UniqIndestructibleGameObject : MonoBehaviour
    {
        private static UniqIndestructibleGameObject _instance;

        [SerializeField] private GameObject _root;
        
        private void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
                _root.SetActive(true);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoaders
{
    public class MenuLoader : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}

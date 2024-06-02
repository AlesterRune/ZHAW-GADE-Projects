using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberspaceOlympics
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene("UserInterfaceScene", LoadSceneMode.Additive);
        }
    }
}

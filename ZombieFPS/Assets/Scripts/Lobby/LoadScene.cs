using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Lobby
{
    public class LoadScene : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
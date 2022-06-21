using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Lobby
{
    public class MissionSelection : MonoBehaviour
    {
        private int currentMission;

        public void SwitchMission(int numberMiss)
        {
            currentMission = numberMiss;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(currentMission);
        }
    }
}
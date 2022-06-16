using System.Collections;
using UnityEngine;
using Assets.Scripts.SO;

namespace Assets.Scripts.Player
{
    public class PlayerPockets : MonoBehaviour
    {
        public int firstAmmo;
        public int secondAmmo;
        public PlayerSO playerSO;
        public BonusesSO bonusesSo;

        private void Start()
        {
            firstAmmo = playerSO.maxFirstAmmo;
            secondAmmo = playerSO.maxSecondAmmo;
        }

        public void AddAmmo(int addFirstAmmo, int addSecondAmmo)
        {
            firstAmmo += addFirstAmmo;
            secondAmmo += addSecondAmmo;
        }
    }
}
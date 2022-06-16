using System.Collections;
using UnityEngine;
using Assets.Scripts.SO;
using Assets.Scripts.Player;

namespace Assets.Scripts.Bonuses
{
    public class BonusesManager : MonoBehaviour
    {
        public BonusesSO bonusesSO;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {

                if (gameObject.CompareTag("HealthKit"))
                {
                    if (other.gameObject.GetComponent<PlayerManager>().health < other.gameObject.GetComponent<PlayerPockets>().playerSO.maxHealth)
                    {
                        gameObject.SetActive(false);
                    }
                    other.gameObject.GetComponent<PlayerCount>().AddCount(bonusesSO.addHealth, "health");
                }
                if (gameObject.CompareTag("ArmorKit"))
                {
                    if (other.gameObject.GetComponent<PlayerManager>().armor < other.gameObject.GetComponent<PlayerPockets>().playerSO.maxArmor)
                    {
                        gameObject.SetActive(false);
                    }
                    other.gameObject.GetComponent<PlayerCount>().AddCount(bonusesSO.addArmor, "armor");
                }
                if (gameObject.CompareTag("AmmoKit"))
                {
                    other.GetComponent<PlayerPockets>().AddAmmo(bonusesSO.addFirstAmmo, bonusesSO.addSecondAmmo);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
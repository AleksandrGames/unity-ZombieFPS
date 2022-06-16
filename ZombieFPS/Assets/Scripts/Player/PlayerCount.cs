using System.Collections;
using UnityEngine;
using Assets.Scripts.SO;

namespace Assets.Scripts.Player
{
    public class PlayerCount : MonoBehaviour
    {
        public PlayerSO playerSO;
        public PlayerManager playerManager;

        private void Start()
        {
            playerManager.health = playerSO.maxHealth;
            if (playerManager.healthText != null)
                playerManager.healthText.text = playerManager.health.ToString();
            playerManager.armor = playerSO.maxArmor;
            if (playerManager.armorText != null)
                playerManager.armorText.text = playerManager.armor.ToString();
        }

        public void TakeCount(float count, string name)
        {
            if(name == "health")
            {
                if (playerManager.health < count)
                {
                    playerManager.health -= playerManager.health;
                }
                else
                {
                    playerManager.health -= count;
                }
                if (playerManager.healthText != null)
                {
                    playerManager.healthText.text = playerManager.health.ToString();
                }
                if (playerManager.health <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            if (name == "armor")
            {
                if(playerManager.armor < count)
                {
                    float countDifference = count - playerManager.armor;
                    playerManager.armor -= playerManager.armor;
                    TakeCount(countDifference, "health");
                }
                else
                {
                    playerManager.armor -= count;
                }
                if (playerManager.armorText != null)
                {
                    playerManager.armorText.text = playerManager.armor.ToString();
                }
                if (playerManager.armor <= 0)
                {
                    
                }
            }
        }

        public void AddCount(int addCount, string name)
        {
            if(name == "health")
            {
                if (playerSO.maxHealth - playerManager.health >= addCount)
                {
                    playerManager.health += addCount;
                }
                else
                {
                    playerManager.health += playerSO.maxHealth - playerManager.health;
                }
                if (playerManager.healthText != null)
                {
                    playerManager.healthText.text = playerManager.health.ToString();
                }
            }
            if (name == "armor")
            {
                if (playerSO.maxArmor - playerManager.armor >= addCount)
                {
                    playerManager.armor += addCount;
                }
                else
                {
                    playerManager.armor += playerSO.maxArmor - playerManager.armor;
                }
                if (playerManager.armorText != null)
                {
                    playerManager.armorText.text = playerManager.armor.ToString();
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy
{
    public class Health : MonoBehaviour
    {
        public float health;
        public float maxHealth;
        public Text healthText;

        private void Start()
        {
            health = maxHealth;
            healthText.text = health.ToString();
        }

        public void TakeHit(float damage)
        {
            health -= damage;
            if(healthText != null)
            {
                healthText.text = health.ToString();
            }
        }

        public void AddHealth(float addHealth)
        {
            if (maxHealth - health >= addHealth)
            {
                health += addHealth;
            }
            else
            {
                health += maxHealth - health;
            }
            if (healthText != null)
            {
                healthText.text = health.ToString();
            }
        }
    }
}
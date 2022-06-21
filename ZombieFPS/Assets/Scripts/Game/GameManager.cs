using UnityEngine;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public Transform enemiesSpawn;
        public GameObject firstEnemy;
        public int countEnemies;

        private void Start()
        {
            for (int i = 0; i < countEnemies; i++)
            {
                GameObject obj = Instantiate(firstEnemy, enemiesSpawn.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity);
                obj.transform.parent = enemiesSpawn;
            }
        }

        public void ZombieRespawn(GameObject zombiePrefab)
        {
            zombiePrefab.transform.position = enemiesSpawn.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            zombiePrefab.SetActive(true);
            zombiePrefab.GetComponent<Health>().AddHealth(zombiePrefab.GetComponent<Health>().maxHealth);
        }
    }
}
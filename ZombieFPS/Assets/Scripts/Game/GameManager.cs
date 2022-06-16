using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public Transform enemiesSpawn;
        public int countEnemies;
        public GameObject firstEnemy;
        private void Start()
        {
            for (int i = 0; i < countEnemies; i++)
            {
                GameObject obj = Instantiate(firstEnemy, enemiesSpawn.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity);
                obj.transform.parent = enemiesSpawn;
            }
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "new enemy", menuName = "ScriptableObjects/enemy")]
    public class EnemySO : ScriptableObject
    {
        public int damage;
    }
}
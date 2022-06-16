using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "new Knife", menuName = "ScriptableObjects/knife")]
    public class KnifeSO : ScriptableObject
    {
        public int addDamage;
        public float addPushEnemy;
        public float addJumpForce;
        public float addSpeed;
    }
}
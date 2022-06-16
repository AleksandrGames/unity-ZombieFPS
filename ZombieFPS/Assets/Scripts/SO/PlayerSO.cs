using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "new hero", menuName = "ScriptableObjects/hero")]
    public class PlayerSO : ScriptableObject
    {
        public float maxHealth, maxArmor;
        public int maxFirstAmmo;
        public int maxSecondAmmo;
    }
}
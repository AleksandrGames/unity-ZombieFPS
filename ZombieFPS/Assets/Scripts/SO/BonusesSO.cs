using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "new bonuses stat", menuName = "ScriptableObjects/bonuses stat")]
    public class BonusesSO : ScriptableObject
    {
        public int addHealth;
        public int addArmor;
        public int addFirstAmmo;
        public int addSecondAmmo;
    }
}
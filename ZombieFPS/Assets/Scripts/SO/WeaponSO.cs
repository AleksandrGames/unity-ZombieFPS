using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "new Weapon", menuName = "ScriptableObjects/weapon")]
    public class WeaponSO : ScriptableObject
    {
        [Header("Stats")]
        public int damage, magazineSize, bulletsPerTap;
        public float weaponRange, timeBetweenShooting, spread, reloadTime, timeBetweenShots;
        public bool allowButtonHold;
        public float recoilForce;
    }
}
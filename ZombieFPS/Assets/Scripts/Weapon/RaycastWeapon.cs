using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Player;
using Assets.Scripts.Enemy;
using Assets.Scripts.SO;

namespace Assets.Scripts.Weapon
{
    public class RaycastWeapon : MonoBehaviour
    {
        [Header("Reference")]
        public WeaponSO weaponSO;
        public PlayerPockets playerPockets;
        public Rigidbody playerRigitbody;
        public Camera fpsCamera;
        public Transform attackPoint;
        public Transform targetLook;
        public Transform targetShoot;
        public GameObject muzzleFlash;
        public Text ammunitionDisplay;

        private int bulletsLeft, bulletsShot, spentAmmo;
        private bool shooting, readyToShoot, reloading;
        private bool allowInvoke = true;

        Vector3 startAttack;
        Vector3 endAttack;

        private void Awake()
        {
            bulletsLeft = weaponSO.magazineSize;
            readyToShoot = true;
        }

        private void Update()
        {
            targetShoot.localPosition = new Vector3(0,0, weaponSO.weaponRange);
            startAttack = fpsCamera.transform.position;
            endAttack = targetShoot.position;
            //if(Physics.Linecast(startCam, endPoint, out RaycastHit hit))
            //{
            //    targetShoot.position = hit.point;
            //}
            //else
            //{
            //    targetShoot.position = targetLook.position;
            //}
            //Debug.DrawLine(startCam, endPoint);
            Debug.DrawLine(startAttack, endAttack, Color.black);
            MyInput();
            if (ammunitionDisplay != null)
            {
                AmmoDisplay();
            }
        }

        private void MyInput()
        {
            if (weaponSO.allowButtonHold)
            {
                shooting = Input.GetKey(PlayerInput.instance.fire);
            }
            else
            {
                shooting = Input.GetKeyDown(PlayerInput.instance.fire);
            }
            if(Input.GetKeyDown(PlayerInput.instance.reload) &&bulletsLeft < weaponSO.magazineSize && !reloading)
            {
                Reload();
            }
            if (readyToShoot && shooting && bulletsLeft <= 0 && !reloading)
            {
                Reload();
            }
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = 0;
                Shoot();
            }
        }

        private void Shoot()
        {
            readyToShoot = false;
            Vector3 targetPoint;
            if (Physics.Linecast(startAttack,endAttack, out RaycastHit hit))
            {
                if (hit.collider.gameObject.GetComponent<Health>() != null)
                {
                    hit.collider.gameObject.GetComponent<Health>().TakeHit(weaponSO.damage);
                }
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = endAttack;
            }
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
            float x = Random.Range(-weaponSO.spread, weaponSO.spread);
            float y = Random.Range(-weaponSO.spread, weaponSO.spread);
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
            if(muzzleFlash != null)
            {
                Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            }
            bulletsLeft--;
            bulletsShot++;
            if (allowInvoke)
            {
                Invoke(nameof(ResetShot), weaponSO.timeBetweenShooting);
                allowInvoke = false;
                playerRigitbody.AddForce(-directionWithSpread.normalized * weaponSO.recoilForce, ForceMode.Impulse);
            }
            if(bulletsShot < weaponSO.bulletsPerTap && bulletsLeft > 0)
            {
                Invoke(nameof(Shoot), weaponSO.timeBetweenShots);
            }
        }

        private void ResetShot()
        {
            readyToShoot = true;
            allowInvoke = true;
        }

        private void Reload()
        {
            if (gameObject.CompareTag("FirstWeapon") && playerPockets.firstAmmo != 0)
            {
                reloading = true;
                Invoke(nameof(ReloadFinished), weaponSO.reloadTime);
            }
            if (gameObject.CompareTag("SecondWeapon") && playerPockets.secondAmmo != 0)
            {
                reloading = true;
                Invoke(nameof(ReloadFinished), weaponSO.reloadTime);
            }
        }

        private void ReloadFinished()
        {
            if (gameObject.CompareTag("FirstWeapon"))
            {
                if (playerPockets.firstAmmo <= weaponSO.magazineSize - bulletsLeft)
                {
                    spentAmmo = playerPockets.firstAmmo;
                }
                else
                {
                    spentAmmo = weaponSO.magazineSize - bulletsLeft;
                }
                playerPockets.firstAmmo -= spentAmmo;
            }
            if (gameObject.CompareTag("SecondWeapon"))
            {
                if (playerPockets.secondAmmo <= weaponSO.magazineSize - bulletsLeft)
                {
                    spentAmmo = playerPockets.secondAmmo;
                }
                else
                {
                    spentAmmo = weaponSO.magazineSize - bulletsLeft;
                }
                playerPockets.secondAmmo -= spentAmmo;
            }
            bulletsLeft += spentAmmo;
            reloading = false;
        }

        private void AmmoDisplay()
        {
            if (gameObject.CompareTag("FirstWeapon"))
            {
                ammunitionDisplay.text = bulletsLeft / weaponSO.bulletsPerTap + "/" + playerPockets.firstAmmo;
            }
            if (gameObject.CompareTag("SecondWeapon"))
            {
                ammunitionDisplay.text = bulletsLeft / weaponSO.bulletsPerTap + "/" + playerPockets.secondAmmo;
            }
        }
    }
}
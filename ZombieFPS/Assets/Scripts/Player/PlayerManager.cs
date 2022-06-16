using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SO;
using Assets.Scripts.Player;
using Assets.Scripts.Weapon;

namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private int selectedWeapon = 1;
        private int previousWeapon;
        public float health;
        public float armor;
        public Text healthText;
        public Text armorText;
        public GameObject firstWeapon;
        public GameObject secondWeapon;
        public GameObject knife;
        private PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(PlayerInput.instance.previousWeapon))
            {
                SelectedWeapon(previousWeapon);
            }
            if (Input.GetKeyDown(PlayerInput.instance.select1))
            {
                SelectedWeapon(1);
            }
            if (Input.GetKeyDown(PlayerInput.instance.select2))
            {
                SelectedWeapon(2);
            }
            if (Input.GetKeyDown(PlayerInput.instance.select3))
            {
                SelectedWeapon(3);
            }
        }

        private void SelectedWeapon(int number)
        {
            previousWeapon = selectedWeapon;
            selectedWeapon = number;
            if (number == 1)
            {
                firstWeapon.SetActive(true);
                secondWeapon.SetActive(false);
                knife.SetActive(false);
            }
            if (number == 2)
            {
                firstWeapon.SetActive(false);
                secondWeapon.SetActive(true);
                knife.SetActive(false);
            }
            if (number == 3)
            {
                firstWeapon.SetActive(false);
                secondWeapon.SetActive(false);
                knife.SetActive(true);
                playerMovement.walkSpeed += knife.GetComponent<KnifeWeapon>().knifeSO.addSpeed;
            }
        }
    }
}
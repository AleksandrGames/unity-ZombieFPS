using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Keybinds")]
        public KeyCode jump = KeyCode.Space;
        public KeyCode sprint = KeyCode.LeftShift;
        public KeyCode crouch = KeyCode.LeftControl;
        public KeyCode fire = KeyCode.Mouse0;
        public KeyCode reload = KeyCode.R;
        public KeyCode dropWeapon = KeyCode.G;
        public KeyCode action = KeyCode.E;
        public KeyCode previousWeapon = KeyCode.Q;
        public KeyCode select1 = KeyCode.Alpha1;
        public KeyCode select2 = KeyCode.Alpha2;
        public KeyCode select3 = KeyCode.Alpha3;

        public static PlayerInput instance;
        private void Awake()
        {
            instance = this;
        }
    }
}
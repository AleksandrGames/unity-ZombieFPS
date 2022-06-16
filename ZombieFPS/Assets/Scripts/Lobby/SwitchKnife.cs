using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobby
{
    public class SwitchKnife : MonoBehaviour
    {
        public GameObject[] knifePrefab;
        public GameObject arrowLeft;
        public GameObject arrowRight;
        private int i;
        private int currentKnife;

        private void Start()
        {
            if (PlayerPrefs.HasKey("CurrentKnife"))
            {
                i = PlayerPrefs.GetInt("CurrentKnife");
                currentKnife = PlayerPrefs.GetInt("CurrentKnife");
            }
            knifePrefab[i].SetActive(true);
            if(i == 0)
            {
                if (arrowLeft != null)
                    arrowLeft.SetActive(false);
            }
            if (i + 1 == knifePrefab.Length)
            {
                if (arrowRight != null)
                    arrowRight.SetActive(false);
            }
        }

        public void ArrowLeft()
        {
            if (i < knifePrefab.Length)
            {
                knifePrefab[i].SetActive(false);
                i--;
                knifePrefab[i].SetActive(true);
                arrowRight.SetActive(true);
                if (i == 0)
                {
                    arrowLeft.SetActive(false);
                }
            }
            SelectKnife();
        }
        public void ArrowRight()
        {
            if (i < knifePrefab.Length)
            {
                if (i == 0)
                {
                    arrowLeft.SetActive(true);
                }
                knifePrefab[i].SetActive(false);
                i++;
                knifePrefab[i].SetActive(true);
            }
            if (i + 1 == knifePrefab.Length)
            {
                arrowRight.SetActive(false);
            }
            SelectKnife();
        }

        public void SelectKnife()
        {
            PlayerPrefs.SetInt("CurrentKnife", i);
            currentKnife = i;
        }
    }
}
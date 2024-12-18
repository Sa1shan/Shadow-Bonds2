using System;
using _Source.Script.Player;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Script.UI
{
    public class PressToReload : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Image reloadImage;


        private void Start()
        {
            reloadImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (weapon.CurrentBullets == 0 && Input.GetButtonDown("Fire1"))
            {
                reloadImage.gameObject.SetActive(true);
            }

            if (weapon.CurrentBullets > 0)
            {
                reloadImage.gameObject.SetActive(false);
            }
        }
    }
}

using System;
using _Source.Script.Player;
using UnityEngine;

namespace _Source.Script.Game
{
    public class AddHp : MonoBehaviour
    {
        [SerializeField] private Health health;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                health.playerhp = 100f;
            }
        }
    }
}

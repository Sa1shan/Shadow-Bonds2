using System;
using UnityEngine;

namespace _Source.Script.Player
{
    public class BulletColision : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}

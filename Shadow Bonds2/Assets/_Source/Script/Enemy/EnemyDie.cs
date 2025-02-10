using System;
using UnityEngine;

namespace _Source.Script.Enemy
{
    public class EnemyDie : MonoBehaviour
    {
        [SerializeField] private float enemyhp = 60f;
        [SerializeField] private float damage = 15f;
        [SerializeField] private Move move;

        [HideInInspector] public int isEnter = 0;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("bullet"))
            {
                Debug.Log("bullet");
                enemyhp -= damage;
                isEnter++;
                Debug.Log(isEnter);
            }
        }

        private void Update()
        {
            if (enemyhp ==  0)
            {
                move.enabled = false;
                Destroy(gameObject);

            }
        }
    }
}

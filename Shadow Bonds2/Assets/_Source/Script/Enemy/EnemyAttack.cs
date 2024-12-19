using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 1f; 
    [SerializeField] private int attackDamage = 10; 

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    { 
        Debug.Log("Attack! Damage: " + attackDamage);
    }
}

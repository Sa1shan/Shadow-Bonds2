using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f; 
    [SerializeField] private float attackRange = 1f; 
    [SerializeField] private int attackDamage = 10; 
    [SerializeField] private Transform player; 
    [SerializeField] private string playerTag = "Player"; 

    private void Update()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        
        if (Mathf.Abs(directionToPlayer.x) <= detectionRadius && Mathf.Abs(directionToPlayer.y) <= detectionRadius)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRadius);
            
            if (hit.collider != null && hit.collider.CompareTag(playerTag))
            {
                if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    AttackPlayer();
                }
                else
                {
                    Debug.Log("Enemy sees player, but is not in attack range.");
                }
            }
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Attack! Damage: " + attackDamage);
    }

    // private void OnDrawGizmos()
    // {
    //     // Отображаем луч для отладки
    //     Vector2 directionToPlayer = player.position - transform.position;
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer.normalized * detectionRadius);
    // }
}

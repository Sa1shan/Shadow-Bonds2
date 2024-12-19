using UnityEngine;
using UnityEngine.AI;

namespace _Source.Script.Enemy
{
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints; 
        [SerializeField] private float detectionRadius = 5f; 
        [SerializeField] private string playerTag = "Player"; 
        
        private int currentPointIndex = 0; 
        private NavMeshAgent navMeshAgent; 
        private Transform player; 

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); 
            player = GameObject.FindGameObjectWithTag(playerTag).transform; 
            Patrol(); 
        }

        private void Update()
        {
            Vector2 directionToPlayer = player.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRadius);

            if (hit.collider != null && hit.collider.CompareTag(playerTag))
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
               
                Patrol();
            }
        }

        private void Patrol()
        {
            if (patrolPoints.Length > 0)
            {
                navMeshAgent.SetDestination(patrolPoints[currentPointIndex].position);
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; 
            }
        }
    }
}

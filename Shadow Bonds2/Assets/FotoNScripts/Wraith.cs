using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Wraith : MonoBehaviour
{
    public float health = 80f;
    public float moveSpeed = 5f;
    public int damageMin = 15;
    public int damageMax = 20;
    public float attackRadius = 10f;
    public float abilityCooldown = 4f;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip whisperSound;
    public ParticleSystem auraEffect;
    public GameObject energyProjectile;

    private float lastAbilityTime = -10f;
    private bool isInvisible = false;
    private enum WraithState { Patrolling, Scouting, Attacking, Defending };
    private WraithState currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = moveSpeed;
        currentState = WraithState.Patrolling;

        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case WraithState.Patrolling:
                ScanForPlayer();
                break;
            case WraithState.Scouting:
                ScoutBehavior();
                break;
            case WraithState.Attacking:
                CombatBehavior();
                break;
            case WraithState.Defending:
                DefendBehavior();
                break;
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (currentState == WraithState.Patrolling)
        {
            TeleportToNextPatrolPoint();
            audioSource.PlayOneShot(whisperSound);
            yield return new WaitForSeconds(Random.Range(2, 5));

            ScanForPlayer();

            yield return new WaitForSeconds(1f);
        }
    }

    private void TeleportToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        transform.position = patrolPoints[currentPatrolIndex].position;
    }

    private void ScanForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                currentState = WraithState.Scouting;
                break;
            }
        }
    }

    private void ScoutBehavior()
    {
        if (player == null || Vector3.Distance(transform.position, player.position) > attackRadius * 1.5f)
        {
            currentState = WraithState.Patrolling;
            return;
        }

        TeleportToStrategicPosition();
        currentState = WraithState.Attacking;
    }

    private void TeleportToStrategicPosition()
    {
        Vector3 randomOffset = Random.insideUnitSphere * attackRadius;
        Vector3 newPosition = player.position + randomOffset;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPosition, out hit, attackRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    private void CombatBehavior()
    {
        if (player == null || Vector3.Distance(transform.position, player.position) > attackRadius * 1.5f)
        {
            currentState = WraithState.Patrolling;
            return;
        }

        AttackPlayer();

        if (health <= 30f)
        {
            currentState = WraithState.Defending;
        }
    }

    private void AttackPlayer()
    {
        if (Time.time >= lastAbilityTime + abilityCooldown)
        {
            animator.SetTrigger("Attack");
            ShootEnergyProjectile();
            lastAbilityTime = Time.time;
        }
    }

    private void ShootEnergyProjectile()
    {
        GameObject projectile = Instantiate(energyProjectile, transform.position, Quaternion.identity);
    }

    private void DefendBehavior()
    {
        CreateEnergyBarrier();
        isInvisible = true;
        if (health < 20f)
        {
            RegenerateHealth();
        }
    }

    private void CreateEnergyBarrier()
    {
        // Forgot about this, will return to it later
    }

    private void RegenerateHealth()
    {
        if (isInvisible)
        {
            health += 10f * Time.deltaTime;
        }
    }
}

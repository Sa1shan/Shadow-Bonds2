using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Guardian : MonoBehaviour
{
    public float bodyHealth = 150f;
    public float shieldHealth = 50f;
    private float totalHealth => bodyHealth + shieldHealth;

    public float normalSpeed = 3f;
    public float dashSpeed = 5f;
    public float detectionRadius = 18f;
    public float attackCooldown = 2.5f;
    public float damageMin = 25f;
    public float damageMax = 30f;

    private float lastAttackTime = 0f;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private bool isBerserking = false;

    public AudioClip blockSound;
    public AudioClip attackSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = normalSpeed;
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (player == null)
            {
                if (agent.remainingDistance < 0.5f)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                    yield return new WaitForSeconds(Random.Range(2, 5));
                }

                ScanForEnemies();
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    private void ScanForEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                StopCoroutine(Patrol());
                StartCoroutine(CombatBehavior());
            }
        }
    }

    private IEnumerator CombatBehavior()
    {
        while (player != null && totalHealth > 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= agent.stoppingDistance)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PerformAttack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                EngagePlayer();
            }

            if (totalHealth < 30f)
            {
                EnterBerserkerMode();
            }
            else if (totalHealth < 70f)
            {
                EnterAttackMode();
            }
            else
            {
                EnterDefenseMode();
            }

            yield return null;
        }

        player = null;
        StartCoroutine(Patrol());
    }

    private void EngagePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void PerformAttack()
    {
        float damage = Random.Range(damageMin, damageMax);

        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSound);
    }

    private void EnterDefenseMode()
    {
        animator.SetBool("IsBlocking", true);

        if (shieldHealth > 0)
        {
            audioSource.PlayOneShot(blockSound);
        }
    }

    private void EnterAttackMode()
    {
        animator.SetBool("IsBlocking", false);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }

        agent.speed = dashSpeed;
    }

    private void EnterBerserkerMode()
    {
        if (!isBerserking)
        {
            isBerserking = true;
            shieldHealth = 0;
            agent.speed *= 1.5f;
        }

        PerformAttack();
    }

    private void TakeDamage(float damage)
    {
        if (shieldHealth > 0)
        {
            shieldHealth -= damage;
            if (shieldHealth < 0)
            {
                bodyHealth += shieldHealth;
                shieldHealth = 0;
            }
        }
        else
        {
            bodyHealth -= damage;
        }

        if (totalHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
    }
}

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Crawler : MonoBehaviour
{
    public float health = 40f;
    public float detectionRadius = 12f;
    public float moveSpeed = 7f;
    public float attackCooldown = 1f;
    public float recoverySpeed = 3f;
    public float criticalHealth = 20f;

    private float lastAttackTime = 0f;
    private NavMeshAgent agent;
    private Transform target;
    private bool isRetreating = false;
    private Vector3 patrolPoint;
    private AudioSource audioSource;

    public AudioClip moveSound;
    public AudioClip attackSound;
    public AudioClip detectSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.speed = moveSpeed;
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (!isRetreating)
            {
                FindNextPatrolPoint();
                agent.SetDestination(patrolPoint);
                yield return new WaitForSeconds(Random.Range(5, 7));

                ScanSurroundings();
                yield return new WaitForSeconds(Random.Range(30, 40));

                audioSource.PlayOneShot(moveSound);
            }
            yield return null;
        }
    }

    void FindNextPatrolPoint()
    {
        //patrolPoint = ...;
    }

    void ScanSurroundings()
    {
        Transform detectedTarget = DetectPlayer();
        if (detectedTarget != null)
        {
            target = detectedTarget;
            audioSource.PlayOneShot(detectSound);
            Attack();
        }
    }

    Transform DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return hit.transform;
            }
        }
        return null;
    }

    void Attack()
    {
        if (target == null) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            transform.LookAt(target);
            audioSource.PlayOneShot(attackSound);

            agent.SetDestination(target.position);
            lastAttackTime = Time.time;

            if (Vector3.Distance(transform.position, target.position) <= agent.stoppingDistance)
            {
                // hitPlayer();

                Vector3 retreatDirection = (transform.position - target.position).normalized;
                agent.SetDestination(transform.position + retreatDirection * Random.Range(2f, 3f));
            }
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (health < criticalHealth)
        {
            Retreat();
        }
    }

    void Retreat()
    {
        isRetreating = true;
        Vector3 retreatDirection = -transform.forward * Random.Range(5f, 7f);
        agent.SetDestination(transform.position + retreatDirection);

        StartCoroutine(RecoverHealth());

        if (health >= criticalHealth)
        {
            isRetreating = false;
        }
    }

    IEnumerator RecoverHealth()
    {
        while (health < 40f && isRetreating)
        {
            health += recoverySpeed * Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            Retreat();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

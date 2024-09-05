using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health;
    public float maxHealth;
    public float baseSpeed;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;
    private float lastAttackTime;

    // Reference to the Player
    private GameObject player;

    [Header("Health Bar")]
    public GameObject healthBarUI;
    public Slider healthBar;

    public NavMeshAgent agent;
    public GameObject model;
    private Animator animator;
    private WaveManager waveManager;
    private bool isDead = false;
    private bool isAttacking = false;
    
    [Header("Powerup Drop")]
    public GameObject[] powerUpPrefabs;

    public float powerUpDropChance = 0.5f;

    void Start()
    {
        health = maxHealth;
        healthBar.value = CalculateHealth();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Add randomness to enemy speed
        agent.speed = baseSpeed + Random.Range(-0.5f, 0.5f);
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        model.transform.localPosition = Vector3.zero;
        model.transform.LookAt(player.transform);
        
        if (isDead) return;

        healthBar.value = CalculateHealth();
        healthBarUI.SetActive(health < maxHealth); // Show health bar only when damaged

        if (health <= 0)
        {
            Die();
            return; // Exit early if dead
        }

        // If currently attacking, do not move or start another attack
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Stop moving if within attack range
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true; // Stop NavMeshAgent from moving
            animator.SetBool("isRunning", false); // Stop running animation

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            animator.SetBool("isRunning", true);
        }
    }

    public void TakeDamage(string damageType)
    {
        float damageAmount = 0;

        switch (damageType)
        {
            case "InstantKill":
                health = 0; // Set health to 0 instantly
                break;
            case "Bullet":
                damageAmount = 20f;
                break;
            case "MachineGun":
                damageAmount = 10f;
                break;
            case "Sniper":
                damageAmount = 100f;
                break;
            case "Explosion":
                damageAmount = 50f;
                break;
            case "Melee":
                damageAmount = 25f;
                break;
            default:
                break;
        }

        if (damageType != "InstantKill")
        {
            health -= damageAmount;
        }
        
        GameManager.instance.AddPoints(25);

        if (health <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        int attackType = Random.Range(0, 6); 
        animator.SetInteger("attackType", attackType);
        float[] attackTimes = { 1.3f, 0.6f, 1f, 1.2f, 0.6f, 1f };
        animator.SetTrigger("attack");
        StartCoroutine(ResetAttackFlag());
        Invoke("DealDamage", attackTimes[attackType]);
    }

    IEnumerator ResetAttackFlag()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("isRunning", true);
    }

    void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) // Check if player is still in range
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        healthBarUI.SetActive(false);
        animator.SetTrigger("death");

        waveManager.ZombieKilled();
        GameManager.instance.AddPoints(100);
        Destroy(gameObject, 1f); // Delay to allow death animation to play
    }
}

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
    private WaveManager waveManager;
    private bool isDead = false;

    void Start()
    {
        health = maxHealth;
        healthBar.value = CalculateHealth();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        
        // Add randomness to enemy speed
        agent.speed = baseSpeed + Random.Range(-0.5f, 0.5f); // Adding a small random variation to speed

        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        if (isDead) return;

        healthBar.value = CalculateHealth();
        healthBarUI.SetActive(health < maxHealth); // Show health bar only when damaged

        if (health <= 0)
        {
            Die();
            return; // Exit early if dead
        }

        // Stop moving if within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Debug.Log("Player within attack range");
            agent.isStopped = true;
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
        }
    }

    public void TakeDamage(string damageType)
    {
        Debug.Log("Enemy hit (" + damageType + ")");
        float damageAmount = 0;

        switch (damageType)
        {
            case "Bullet":
                damageAmount = 20f;
                break;
            case "Explosion":
                damageAmount = 50f;
                break;
            case "Melee":
                damageAmount = 20f;
                break;
            default:
                break;
        }

        health -= damageAmount;
        GameManager.instance.AddPoints(10);

        if (health <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        Debug.Log("Attacking player");
        DealDamage();
        // animator.SetTrigger("Attack");
        // Invoke("DealDamage", 0.5f); // Delay the actual damage to sync with attack animation
    }

    void DealDamage()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }

    float CalculateHealth()
    {
        return health / maxHealth; // Calculate the health percentage
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true; // Stop the enemy from moving
        // animator.SetBool("Dead", true); // Trigger death animation if you have one

        waveManager.ZombieKilled(); // Notify the WaveManager that this zombie has died
        
        // Award points for killing the enemy
        GameManager.instance.AddPoints(50); // Example: Award 50 points for a kill

        // Add any additional death effects here (e.g., sound, loot)
        Destroy(gameObject);
    }
}
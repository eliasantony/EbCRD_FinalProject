using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health; // Set the initial health of the enemy
    public float maxHealth; // Set the max health of the enemy
    public float speed;  // Set the speed of the enemy
    public float attackRange; // Set the attack range of the enemy
    public float attackDamage; // Set the attack damage of the enemy
    public float attackCooldown; // Set the attack cooldown of the enemy
    private float lastAttackTime; // Set the last attack time of the enemy
    
    // Reference to the Player
    private PlayerHealth player;

    [Header("Health Bar")]
    public GameObject healthBarUI; // Reference to the health bar UI
    public Slider healthBar; // Reference to the health bar

    private Transform playerTransform; // Reference to the player's transform
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    private Animator animator; // Reference to the Animator
    
    private WaveManager waveManager;
    private bool isDead = false; // Track if the enemy is dead

    public void Start()
    {
        health = maxHealth; // Set the initial health to max health
        healthBar.value = CalculateHealth(); // Set the health bar value
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>(); // Get the PlayerHealth script
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Get the player's transform
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        agent.speed = speed; // Set the speed of the NavMeshAgent
        animator = GetComponent<Animator>(); // Get the Animator component
        waveManager = GameObject.FindObjectOfType<WaveManager>(); // Get the WaveManager script
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; // If dead, don't perform any more actions

        healthBar.value = CalculateHealth(); // Update the health bar value

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true); // Show the health bar UI
        }
        if (health <= 0)
        {
            Die();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // Make the enemy face the player only on the y-axis
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // This makes the enemy not look up/down
        transform.rotation = Quaternion.LookRotation(direction);

        agent.SetDestination(playerTransform.position); // Set the destination of the NavMeshAgent to the player's position
        
        // Set animator parameter for movement
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange 
            && Time.time - lastAttackTime >= attackCooldown) // Check if enough time has passed since the last attack
        {
            Attack();
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    public void TakeDamage(string damageType)
    {
        switch (damageType)
        {
            case "Bullet":
                Debug.Log("Bullet Damage: 20");
                health -= 20f;
                //player.GetComponent<PlayerHealth>().AddPoints(10); // Add points to the player
                break;
            case "Explosion":
                Debug.Log("Explosion Damage: 50");
                health -= 50f;
                //player.GetComponent<PlayerHealth>().AddPoints(25); // Add points to the player
                break;
            case "Melee":
                Debug.Log("Melee Damage: 20");
                health -= 20f;
                //player.GetComponent<PlayerHealth>().AddPoints(15); // Add points to the player
                break;
            default:
                break;
        }
    }

    void Attack()
    {
        // Set animator parameter for attack
        animator.SetTrigger("Attack");
        
        Invoke("DealDamage", 0.5f);
    }
    
    void DealDamage()
    {
        // Decrease the player's health
        player.TakeDamage(attackDamage);
    }

    float CalculateHealth()
    {
        return health / maxHealth; // Calculate the health percentage
    }

    void Die()
    {
        if (isDead) return; // Ensure Die is only called once

        isDead = true; // Mark the enemy as dead

        // Set animator parameter for death
        animator.SetBool("Dead", true);
        
        agent.isStopped = true; // Stop the enemy from moving
        
        // Notify the WaveManager that this zombie has died
        waveManager.ZombieKilled();
        
        // Destroy the enemy object after a delay to allow death animation to play
        Destroy(gameObject, 2f); 
    }
}

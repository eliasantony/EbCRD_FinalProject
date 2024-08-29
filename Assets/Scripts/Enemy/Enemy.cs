using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health;
    public float maxHealth;
    public float speed;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;
    private float lastAttackTime;
    
    // Reference to the Player
    private PlayerHealth player;

    [Header("Health Bar")]
    public GameObject healthBarUI;
    public Slider healthBar;

    private Transform playerTransform;
    public NavMeshAgent agent;
    //private Animator animator;
    
    private WaveManager waveManager;
    private bool isDead = false;

    public void Start()
    {
        health = maxHealth;
        healthBar.value = CalculateHealth();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        //animator = GetComponent<Animator>();
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }
    
    void Update()
    {
        if (isDead) return; 

        healthBar.value = CalculateHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health <= 0)
        {
            Die();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        //agent.SetDestination(playerTransform.position); 
        //animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        
        Debug.Log("Distance to player: " + Vector3.Distance(transform.position, playerTransform.position));
        // Debug.Log("Distance to player (Agent): " + agent.remainingDistance(playerTransform.position));
        
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange 
            && Time.time - lastAttackTime >= attackCooldown) // Check if enough time has passed since the last attack
        {
            Attack();
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    public void TakeDamage(string damageType)
    {
        Debug.Log("Taking damage from: " + damageType);
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
        Debug.Log("Attacking player");
        //animator.SetTrigger("Attack");
        Invoke("DealDamage", 0.5f);
    }
    
    void DealDamage()
    {
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
        
        //animator.SetBool("Dead", true);
        
        agent.isStopped = true; // Stop the enemy from moving
        
        // Notify the WaveManager that this zombie has died
        waveManager.ZombieKilled();
        
        // Destroy the enemy object after a delay to allow death animation to play
        Destroy(gameObject, 2f); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName = "Knife";
    public float damage = 25f; // Damage dealt by the melee weapon
    public float attackRange = 1.5f; // Range of the melee attack
    public float attackCooldown = 0.5f; // Time between attacks
    private float lastAttackTime = 0f;

    private Animator animator;
    private AudioSource audioSource;
    private InputManager _inputManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Optional: add audio feedback
    }

    void Update()
    {
        // Check for attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        // Play attack animation immediately
        if (animator != null)
        {
            animator.ResetTrigger("Attack"); // Ensure trigger resets before setting
            animator.SetTrigger("Attack");
        }

        // Play attack sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Detect enemies within attack range using OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage("Melee");
                }
            }
        }
    }

}

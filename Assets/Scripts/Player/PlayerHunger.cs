using System;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDecayRate = 0.75f;   // Hunger depletion per tick
    public float healthDecayRate = 0.5f;    // Health decay rate when hunger is zero
    public float tickSpeed = 1f;            // Time in seconds between each tick
    private float tickTimer = 0f;           // Time since the last tick
    private PlayerHealth playerHealth;

    void Start()
    {
        currentHunger = maxHunger;
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickSpeed)
        {
            HandleHunger();
            tickTimer = 0f;
        }
        UIManager.instance.UpdateHunger(currentHunger / maxHunger);
    }

    private void HandleHunger()
    {
        currentHunger -= hungerDecayRate;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (currentHunger <= 0)
        {
            playerHealth.TakeDamage(healthDecayRate);
        }
    }

    public void EatFood(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
    }

    public bool IsFull()
    {
        return currentHunger >= maxHunger;
    }
}
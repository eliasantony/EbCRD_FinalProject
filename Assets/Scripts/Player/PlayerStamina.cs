using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float sprintStaminaCost = 10f; // Stamina cost per second while sprinting
    public float staminaRegenRate = 5f; // Stamina regeneration per second
    public float idleRegenMultiplier = 2f; // Additional regen multiplier when idle
    private PlayerMotor playerMotor;

    void Start()
    {
        currentStamina = maxStamina;
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        HandleStamina();
        UIManager.instance.UpdateStamina(currentStamina / maxStamina);
    }

    private void HandleStamina()
    {
        if (playerMotor.IsSprinting() && currentStamina > 0)
        {
            currentStamina -= sprintStaminaCost * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                playerMotor.SetCanSprint(false); // Disable sprinting when stamina is depleted
            }
        }
        else
        {
            RegenerateStamina();
        }
    }

    private void RegenerateStamina()
    {
        float regenRate = playerMotor.IsMoving() ? staminaRegenRate : staminaRegenRate * idleRegenMultiplier;
        currentStamina += regenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina > sprintStaminaCost)
        {
            playerMotor.SetCanSprint(true); // Allow sprinting when stamina is fully regenerated
        }
    }

    public void ModifyStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
    }
}
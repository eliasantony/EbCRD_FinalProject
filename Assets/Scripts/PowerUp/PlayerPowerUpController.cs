using UnityEngine;

public class PlayerPowerUpController : MonoBehaviour
{
    private bool unlimitedAmmo;
    private bool instantKill;
    private bool unlimitedStamina;
    private bool shieldActive;
    private float powerUpTimer;

    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;
    private ProjectileGun equippedGun;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerStamina = GetComponent<PlayerStamina>();
        equippedGun = GetComponentInChildren<ProjectileGun>();
    }

    void Update()
    {
        if (powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0)
            {
                DeactivatePowerUps();
            }
        }
    }

    public void ActivatePowerUp(PowerUp.PowerUpType powerUpType, float duration)
    {
        powerUpTimer = duration;

        switch (powerUpType)
        {
            case PowerUp.PowerUpType.UnlimitedAmmo:
                unlimitedAmmo = true;
                equippedGun?.SetUnlimitedAmmo(true);
                break;

            case PowerUp.PowerUpType.InstantKill:
                instantKill = true;
                equippedGun?.SetInstantKill(true);
                break;

            case PowerUp.PowerUpType.UnlimitedStamina:
                unlimitedStamina = true;
                playerStamina.SetUnlimitedStamina(true);
                break;

            case PowerUp.PowerUpType.HealthBoost:
                playerHealth.HealToFull();
                break;

            case PowerUp.PowerUpType.Shield:
                shieldActive = true;
                playerHealth.EnableShield();
                break;

            case PowerUp.PowerUpType.PointsMultiplier:
                GameManager.instance.SetPointsMultiplier(2f);
                break;

            case PowerUp.PowerUpType.SpeedBoost:
                playerStamina.SetSpeedBoost(true);
                break;
        }

        Debug.Log($"{powerUpType} activated for {duration} seconds.");
    }

    private void DeactivatePowerUps()
    {
        unlimitedAmmo = false;
        instantKill = false;
        unlimitedStamina = false;
        shieldActive = false;
        
        equippedGun?.SetUnlimitedAmmo(false);
        equippedGun?.SetInstantKill(false);
        playerStamina.SetUnlimitedStamina(false);
        playerStamina.SetSpeedBoost(false);
        GameManager.instance.SetPointsMultiplier(1f); // Reset points multiplier

        Debug.Log("Power-up effects deactivated.");
    }
}

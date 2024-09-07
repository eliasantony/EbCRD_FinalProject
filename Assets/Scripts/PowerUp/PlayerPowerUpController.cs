using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPowerUpController : MonoBehaviour
{
    private bool _unlimitedAmmo;
    private bool _instantKill;
    private bool _unlimitedStamina;
    private bool _shieldActive;

    private float _unlimitedAmmoTimer;
    private float _instantKillTimer;
    private float _unlimitedStaminaTimer;
    private float _shieldTimer;
    private float _speedBoostTimer;
    private float _pointsMultiplierTimer;

    private PlayerHealth _playerHealth;
    private PlayerStamina _playerStamina;
    private ProjectileGun _equippedGun;
    private MeleeWeapon _equippedMelee;

    // UI
    private Dictionary<string, float> activePowerUps = new Dictionary<string, float>();
    public TextMeshProUGUI powerUpTextPrefab; // Reference to the Text prefab
    public Transform powerUpPanel; // Reference to the panel to display the power-up UI
    private Dictionary<string, TextMeshProUGUI> powerUpTexts = new Dictionary<string, TextMeshProUGUI>();

    void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerStamina = GetComponent<PlayerStamina>();
        _equippedMelee = GetComponentInChildren<MeleeWeapon>();
    }

    void Update()
    {
        UpdatePowerUpTimers();
        UpdatePowerUpUI();
    }

    private void UpdatePowerUpTimers()
    {
        // Decrease the timers for each power-up if they're active
        if (_unlimitedAmmoTimer > 0)
        {
            _unlimitedAmmoTimer -= Time.deltaTime;
            if (_unlimitedAmmoTimer <= 0)
            {
                _equippedGun = GetComponentInChildren<ProjectileGun>();
                _equippedGun?.SetUnlimitedAmmo(false);
                Debug.Log("Unlimited Ammo power-up expired.");
                RemovePowerUp("Unlimited Ammo");
            }
        }

        if (_instantKillTimer > 0)
        {
            _instantKillTimer -= Time.deltaTime;
            if (_instantKillTimer <= 0)
            {
                _equippedMelee?.SetInstantKill(false);
                _equippedGun = GetComponentInChildren<ProjectileGun>();
                _equippedGun?.SetInstantKill(false);
                RemovePowerUp("Instant Kill");
            }
        }

        if (_unlimitedStaminaTimer > 0)
        {
            _unlimitedStaminaTimer -= Time.deltaTime;
            if (_unlimitedStaminaTimer <= 0)
            {
                _playerStamina.SetUnlimitedStamina(false);
                RemovePowerUp("Unlimited Stamina");
            }
        }

        if (_shieldTimer > 0)
        {
            _shieldTimer -= Time.deltaTime;
            if (_shieldTimer <= 0)
            {
                _playerHealth.DisableShield();
                RemovePowerUp("Shield");
            }
        }

        if (_speedBoostTimer > 0)
        {
            _speedBoostTimer -= Time.deltaTime;
            if (_speedBoostTimer <= 0)
            {
                _playerStamina.SetSpeedBoost(false);
                RemovePowerUp("Speed Boost");
            }
        }
        
        if (_pointsMultiplierTimer > 0)
        {
            _pointsMultiplierTimer -= Time.deltaTime;
            if (_pointsMultiplierTimer <= 0)
            {
                GameManager.instance.SetPointsMultiplier(1f);
                RemovePowerUp("Points Multiplier");
            }
        }
    }

    public void ActivatePowerUp(PowerUp.PowerUpType powerUpType, float duration)
    {
        switch (powerUpType)
        {
            case PowerUp.PowerUpType.UnlimitedAmmo:
                _unlimitedAmmoTimer += duration;
                _equippedGun = GetComponentInChildren<ProjectileGun>();
                _equippedGun?.SetUnlimitedAmmo(true);
                AddPowerUpToUI("Unlimited Ammo", duration);
                break;

            case PowerUp.PowerUpType.InstantKill:
                _instantKillTimer += duration;
                _equippedMelee?.SetInstantKill(true);
                _equippedGun = GetComponentInChildren<ProjectileGun>();
                _equippedGun?.SetInstantKill(true);
                AddPowerUpToUI("Instant Kill", duration);
                break;

            case PowerUp.PowerUpType.UnlimitedStamina:
                _unlimitedStaminaTimer += duration;
                _playerStamina.SetUnlimitedStamina(true);
                AddPowerUpToUI("Unlimited Stamina", duration);
                break;

            case PowerUp.PowerUpType.HealthBoost:
                _playerHealth.HealToFull();
                break;

            case PowerUp.PowerUpType.Shield:
                _shieldTimer += duration;
                _playerHealth.EnableShield();
                AddPowerUpToUI("Shield", duration);
                break;

            case PowerUp.PowerUpType.SpeedBoost:
                _speedBoostTimer += duration;
                _playerStamina.SetSpeedBoost(true);
                AddPowerUpToUI("Speed Boost", duration);
                break;
            
            case PowerUp.PowerUpType.PointsMultiplier:
                _pointsMultiplierTimer += duration;
                GameManager.instance.SetPointsMultiplier(2f);
                AddPowerUpToUI("Points Multiplier", duration);
                break;
        }
    }
    
    private void AddPowerUpToUI(string powerUpName, float remainingTime)
    {
        if (powerUpTexts.ContainsKey(powerUpName))
        {
            // Update the remaining time in the existing UI text
            powerUpTexts[powerUpName].text = $"{powerUpName}: {Mathf.Ceil(remainingTime)}s";
        }
        else
        {
            // Add a new UI element for this power-up if it's not already there
            activePowerUps[powerUpName] = remainingTime;
            TextMeshProUGUI newPowerUpText = Instantiate(powerUpTextPrefab, powerUpPanel);
            newPowerUpText.text = $"{powerUpName}: {Mathf.Ceil(remainingTime)}s";
            powerUpTexts[powerUpName] = newPowerUpText;
        }
    }


    // Update the power-up UI
    private void UpdatePowerUpUI()
    {
        List<string> powerUpsToRemove = new List<string>();
        foreach (var powerUp in new Dictionary<string, float>(activePowerUps))
        {
            if (powerUpTexts.ContainsKey(powerUp.Key))
            {
                float remainingTime = activePowerUps[powerUp.Key] -= Time.deltaTime;
                powerUpTexts[powerUp.Key].text = $"{powerUp.Key}: {Mathf.Ceil(remainingTime)}s";

                if (remainingTime <= 0)
                {
                    powerUpsToRemove.Add(powerUp.Key);
                }
            }
        }
        
        foreach (string powerUp in powerUpsToRemove)
        {
            RemovePowerUp(powerUp);
        }
    }


    // Remove the power-up from the dictionary and UI
    private void RemovePowerUp(string powerUpName)
    {
        if (activePowerUps.ContainsKey(powerUpName))
        {
            activePowerUps.Remove(powerUpName);
        }

        if (powerUpTexts.ContainsKey(powerUpName))
        {
            Destroy(powerUpTexts[powerUpName].gameObject);
            powerUpTexts.Remove(powerUpName);
        }
    }
}

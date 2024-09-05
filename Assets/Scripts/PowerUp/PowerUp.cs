using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { UnlimitedAmmo, InstantKill, UnlimitedStamina, HealthBoost, Shield, PointsMultiplier, SpeedBoost }
    public PowerUpType powerUpType;
    public float powerUpDuration = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp(other.gameObject);
            Destroy(gameObject); // Destroy power-up model once collected
        }
    }

    void ActivatePowerUp(GameObject player)
    {
        PlayerPowerUpController powerUpController = player.GetComponent<PlayerPowerUpController>();
        if (powerUpController != null)
        {
            powerUpController.ActivatePowerUp(powerUpType, powerUpDuration);
        }
    }
}
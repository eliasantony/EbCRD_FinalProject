using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { UnlimitedAmmo, InstantKill, UnlimitedStamina, HealthBoost, Shield, PointsMultiplier, SpeedBoost }
    public PowerUpType powerUpType;
    public float powerUpDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-up collected!");
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
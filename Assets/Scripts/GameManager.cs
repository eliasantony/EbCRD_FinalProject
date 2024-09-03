using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    public PlayerHealth playerHealth;
    public PlayerPoints playerPoints;
    public ConfigLoader configLoader;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    void InitGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AddPoints(1000); // Initial points for testing
    }

    public void AddPoints(int points)
    {
        playerPoints.AddPoints(points);
        UIManager.instance.UpdatePoints(playerPoints.points);
    }

    public bool SpendPoints(int amount)
    {
        bool success = playerPoints.SpendPoints(amount);
        if (success)
        {
            UIManager.instance.UpdatePoints(playerPoints.points);
        }
        return success;
    }

    // Purchase a gun and provide it to the player
    public bool PurchaseGun(ProjectileGun gun)
    {
        configLoader.LoadGunConfig(gun.weaponName);
        GunConfig gunConfig = configLoader.gunConfig;
        int cost = gunConfig.price;
        if (SpendPoints(cost))
        {
            // Instantiate or enable the gun in the player's inventory
            gun.totalAmmo = gun.magazineSize; // Only 1 magazine initially
            gun.bulletsLeft = gun.magazineSize;
            UIManager.instance.UpdateAmmoDisplay(gun.bulletsLeft + " / " + gun.totalAmmo);
            return true;
        }
        return false;
    }

    // Purchase ammo for the current gun
    public bool PurchaseAmmo(ProjectileGun gun)
    {
        configLoader.LoadGunConfig(gun.weaponName);
        GunConfig gunConfig = configLoader.gunConfig;
        int cost = gunConfig.ammoCost;
        if (SpendPoints(cost))
        {
            gun.AddAmmo(gun.magazineSize);
            UIManager.instance.UpdateAmmoDisplay(gun.bulletsLeft + " / " + gun.totalAmmo);
            return true;
        }
        return false;
    }

    public void UpdateHealthUI()
    {
        UIManager.instance.UpdateHealth(playerHealth.GetCurrentHealth());
    }
    
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}

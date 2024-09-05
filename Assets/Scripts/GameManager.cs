using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    public PlayerHealth playerHealth;
    public PlayerPoints playerPoints;
    public ConfigLoader configLoader;
    public GameObject playerHUD;
    public GameObject pauseUI;
    public GameObject gameOverUI;
    private bool isGameOver = false;
    private float pointsMultiplier = 1f;

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

        AddPoints(10000); // Initial points for testing
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (Time.timeScale == 0f)
            {
                pauseUI.SetActive(false);
                ResumeGame();
            }
            else
            {
                pauseUI.SetActive(true);
                PauseGame();
            }
        }
    }

    public void SetPointsMultiplier(float multiplier)
    {
        pointsMultiplier = multiplier;
        Debug.Log("Points Multiplier set to: " + multiplier);
    }

    public void AddPoints(int points)
    {
        int finalPoints = Mathf.RoundToInt(points * pointsMultiplier);
        playerPoints.AddPoints(finalPoints);
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
    
    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        playerHUD.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToStartScreen()
    {
        // Destroy Game Manager and UIManager
        Destroy(gameObject);
        Destroy(UIManager.instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }
    
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}

using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI promptDisplay;
    public TextMeshProUGUI pointsText;
    
    public Image healthBar;
    public Image hungerBar;
    public Image staminaBar;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of UIManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePoints(int points)
    {
        pointsText.text = points.ToString();
    }

    public void UpdateHealth(float health)
    {
        healthBar.fillAmount = health / 100f;
    }
    
    public void UpdateHunger(float hunger)
    {
        hungerBar.fillAmount = hunger;
    }
    
    public void UpdateStamina(float stamina)
    {
        staminaBar.fillAmount = stamina;
    }
    
    public void UpdateAmmoDisplay(string text)
    {
        ammunitionDisplay.text = text;
    }

    public void UpdateReloadDisplay(string text)
    {
        promptDisplay.text = text;
    }
}
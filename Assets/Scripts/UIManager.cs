using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI promptMessage;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI waveNumber;
    public TextMeshProUGUI waveTimer;
    
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
    
    public static string ToRoman(int number)
    {
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
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
    
    public void EnableAmmoDisplay(bool enabled)
    {
        ammunitionDisplay.gameObject.SetActive(enabled);
    }
    
    public void UpdateAmmoDisplay(string text)
    {
        ammunitionDisplay.text = text;
    }

    public void UpdatePromptMessage(string message)
    {
        promptMessage.text = message;
    }
    
    public void UpdateReloadingText(string message)
    {
        reloadingText.text = message;
    }
    
    public void UpdateWaveNumber(int wave)
    {
        waveNumber.text = ToRoman(wave);
    }
    
    public void UpdateWaveTimer(float time)
    {
        if (time <= 0.5)
            waveTimer.text = "";
        else
            waveTimer.text = "Next wave in: " + Mathf.Ceil(time).ToString() + "s";
    }
}
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    public PlayerHealth playerHealth;
    public PlayerPoints playerPoints;
    
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
        
        AddPoints(1000);
    }
    
    void Update()
    {
        // TODO: Add game update logic here
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

    public void UpdateHealthUI()
    {
        UIManager.instance.UpdateHealth(playerHealth.GetCurrentHealth());
    }
}
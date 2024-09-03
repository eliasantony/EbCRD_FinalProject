using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediPackInteractable : Interactable
{
    public int mediPackPrice = 200; // Set price for medipack
    public float healAmount = 50f; // Amount to heal
    private GameObject player;

    private void Start()
    {
        promptMessage = $"Buy Health for {mediPackPrice} points";
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Interact()
    {
        // Check if the player has enough points
        if (GameManager.instance.SpendPoints(mediPackPrice))
        {
            // Logic for healing the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.GetCurrentHealth() < playerHealth.maxHealth)
            {
                playerHealth.Heal(healAmount);
                Debug.Log("Medipack purchased! Health increased.");
            }
        }
        else
        {
            Debug.Log("Not enough points to purchase medipack.");
            UIManager.instance.UpdatePromptMessage("Not enough points to purchase medipack.");
        }
    }
}
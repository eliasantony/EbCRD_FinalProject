using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInteractable : Interactable
{
    public int foodPrice = 300; // Set price for food
    public float hungerAmount = 20f; // Amount to increase hunger
    private GameObject player;

    private void Start()
    {
        promptMessage = $"Buy Food for {foodPrice} points";
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Interact()
    {
        if (GameManager.instance.SpendPoints(foodPrice))
        {
            PlayerHunger playerHunger = player.GetComponent<PlayerHunger>();
            if (playerHunger != null && !playerHunger.IsFull())
            {
                playerHunger.EatFood(hungerAmount);
            }
        }
        else
        {
            UIManager.instance.UpdatePromptMessage("Not enough points to purchase food.");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShopInteractable : Interactable
{
    private GameObject player;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        promptMessage = "Buy Ammo"; // Initial prompt, updated dynamically based on the weapon
    }

    protected override void Interact()
    {
        // Get the current gun equipped by the player
        ProjectileGun currentGun = player.GetComponentInChildren<ProjectileGun>();

        if (currentGun == null)
        {
            Debug.Log("No gun equipped. Cannot purchase ammo.");
            UIManager.instance.UpdatePromptMessage("No gun equipped. Cannot purchase ammo.");
            return;
        }
        
        int ammoPrice = currentGun.ammoCost;
        
        if (GameManager.instance.SpendPoints(ammoPrice))
        {
            currentGun.AddAmmo(currentGun.magazineSize);
            UIManager.instance.UpdateAmmoDisplay(currentGun.bulletsLeft + " / " + currentGun.totalAmmo);
            Debug.Log("Ammo purchased! Ammo increased.");
        }
        else
        {
            Debug.Log("Not enough points to purchase ammo.");
            UIManager.instance.UpdatePromptMessage("Not enough points to purchase ammo.");
        }
    }

    private void Update()
    {
        ProjectileGun currentGun = player.GetComponentInChildren<ProjectileGun>();
        if (currentGun != null)
            promptMessage = $"Buy Ammo for {currentGun.ammoCost} points";
        else
            promptMessage = "No gun equipped. Cannot buy ammo.";
    }
}
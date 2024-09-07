using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject knifeHolder; // The knife holder with the melee weapon
    public GameObject gunHolder; // The gun holder where guns will be equipped
    public MeleeWeapon meleeWeaponScript;

    private ProjectileGun gunScript; // Reference to the currently equipped gun
    private GameObject currentGunObject; // The GameObject for the current gun
    private int currentWeaponIndex = 0; // 0 for melee, 1 for gun
    private InputManager _inputManager;

    void Start()
    {
        _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();

        // Start with the melee weapon equipped
        EquipWeapon(0);
    }

    void Update()
    {
        HandleWeaponSwitch();
    }

    void HandleWeaponSwitch()
    {
        // Only allow weapon switching if a gun is equipped
        if (gunScript != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f || _inputManager._onFootActions.SwitchWeapon.triggered)
            {
                // Toggle weapon index (0 for melee, 1 for gun)
                currentWeaponIndex = (currentWeaponIndex == 0) ? 1 : 0;
                EquipWeapon(currentWeaponIndex);
            }
        }
        else
        {
            // If no gun is equipped, ensure the melee weapon is always active
            currentWeaponIndex = 0;
            EquipWeapon(0);
        }
    }

    public void EquipWeapon(int index)
    {
        if (index == 0) // Melee weapon
        {
            knifeHolder.SetActive(true);
            if (gunHolder != null) gunHolder.SetActive(false);
            if (meleeWeaponScript != null) meleeWeaponScript.enabled = true;
            if (gunScript != null) gunScript.enabled = false;
            UIManager.instance.EnableAmmoDisplay(false);
        }
        else if (index == 1 && gunScript != null) // Gun
        {
            knifeHolder.SetActive(false);
            if (gunHolder != null) gunHolder.SetActive(true);
            if (meleeWeaponScript != null) meleeWeaponScript.enabled = false;
            gunScript.enabled = true;
            UIManager.instance.EnableAmmoDisplay(true);
        }
    }

    // Called by PickUpController when a gun is picked up
    public void EquipGun(GameObject newGun)
    {
        // Check if a gun is already equipped, if yes, drop it
        if (currentGunObject != null)
        {
            DropCurrentGun(); // Drop the currently equipped gun
        }

        // Set the new gun to the gun holder
        newGun.transform.SetParent(gunHolder.transform);
        newGun.transform.localPosition = Vector3.zero;
        newGun.transform.localRotation = Quaternion.identity;

        // Update gunScript reference and current gun object
        gunScript = newGun.GetComponent<ProjectileGun>();
        currentGunObject = newGun;

        // Equip the gun automatically
        EquipWeapon(1);
    }

    // Called by PickUpController when a gun is dropped
    public void DropGun()
    {
        if (gunScript != null)
        {
            // Clear gun reference
            gunScript = null;
            currentGunObject = null;

            // Re-equip melee weapon
            EquipWeapon(0);
        }
    }

    // Drop the current equipped gun (if one is equipped)
    public void DropCurrentGun()
    {
        if (currentGunObject != null)
        {
            // Call the Drop method on the currently equipped gun's PickUpController
            currentGunObject.GetComponent<PickUpController>().Drop();
        }
    }
}

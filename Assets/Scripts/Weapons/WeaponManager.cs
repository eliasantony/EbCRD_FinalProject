using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject knifeHolder; // The knife holder with the melee weapon
    public GameObject gunHolder; // The gun holder where guns will be equipped
    public MeleeWeapon meleeWeaponScript;

    private ProjectileGun gunScript;
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
        }
        else if (index == 1 && gunScript != null) // Gun
        {
            knifeHolder.SetActive(false);
            if (gunHolder != null) gunHolder.SetActive(true);
            if (meleeWeaponScript != null) meleeWeaponScript.enabled = false;
            gunScript.enabled = true;
        }
    }

    // Called by PickUpController when a gun is picked up
    public void EquipGun(GameObject newGun)
    {
        // Set the gun to the gun holder
        newGun.transform.SetParent(gunHolder.transform);
        newGun.transform.localPosition = Vector3.zero;
        newGun.transform.localRotation = Quaternion.identity;

        // Update gunScript reference
        gunScript = newGun.GetComponent<ProjectileGun>();

        // Initially equip the melee weapon if it was switched
        EquipWeapon(1);
    }

    // Called by PickUpController when a gun is dropped
    public void DropGun()
    {
        if (gunScript != null)
        {
            // Clear gun reference
            gunScript = null;

            // Re-equip melee weapon
            EquipWeapon(0);
        }
    }
}

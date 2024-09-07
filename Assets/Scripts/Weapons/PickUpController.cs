using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpController : MonoBehaviour
{
    public ProjectileGun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    private InputManager _inputManager;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public bool equipped;
    public bool slotFull; // Removed static keyword

    private WeaponManager weaponManager; // Reference to WeaponManager

    private void Start()
    {
        _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();

        originalPosition = gunContainer.localPosition;
        originalRotation = gunContainer.localRotation;

        // Setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            gunScript.LoadAmmoState();
        }
    }

    private void Update()
    {
        if (equipped)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            HandleAiming();
        }

        // Check if player is in range of the weapon
        Ray ray = fpsCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.transform == transform)
            {
                if (!gunScript.isPurchased)
                {
                    UIManager.instance.UpdatePromptMessage($"Press E to buy {gunScript.weaponName} for {gunScript.price} points.");
                }
                else
                {
                    UIManager.instance.UpdatePromptMessage("Press E to pick up the weapon.");
                }
                if (!equipped && _inputManager._onFootActions.Interact.triggered && !slotFull)
                {
                    AttemptPickUp();
                }
            }
        }
        else
        {
            UIManager.instance.UpdatePromptMessage(""); // Clear the prompt when not looking at a weapon
        }

        // Drop if equipped and "Q" is pressed
        if (equipped && _inputManager._onFootActions.Drop.triggered)
        {
            Drop();
        }
    }

    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            gunContainer.localPosition = new Vector3(gunScript.aimSide, gunScript.aimHeight, 0.35f);
            gunContainer.localRotation = Quaternion.Euler(0, -180, 0);
        }
        else if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            gunContainer.localPosition = originalPosition;
            gunContainer.localRotation = originalRotation;
        }
    }

    private void AttemptPickUp()
    {
        if (!gunScript.isPurchased)
        {
            StartCoroutine(TryPurchaseGun());
        }
        else
        {
            PickUp();
        }
    }

    private IEnumerator TryPurchaseGun()
    {
        // Try purchasing the gun via coroutine
        yield return StartCoroutine(GameManager.instance.PurchaseGun(gunScript));
        // If purchase was successful, pick up and mark as purchased
        if (gunScript.isPurchased)
        {
            PickUp();
        }
        else
        {
            UIManager.instance.UpdatePromptMessage("Not enough points to purchase this weapon!");
        }
    }


    private void PickUp()
    {
        equipped = true;
        slotFull = true;
        UIManager.instance.EnableAmmoDisplay(true);

        // Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Enable script
        gunScript.enabled = true;
        gunScript.LoadAmmoState();

        // Add the gun to the WeaponManager
        weaponManager.EquipGun(gameObject);

        // Clear prompt after pickup
        UIManager.instance.UpdatePromptMessage("");
    }

    internal void Drop()
    {
        equipped = false;
        slotFull = false;
        UIManager.instance.EnableAmmoDisplay(false);

        // Set parent to null
        transform.SetParent(null);

        // Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Gun carries momentum of player
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        // Add forces
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        // Disable the Ammo UI
        UIManager.instance.UpdateAmmoDisplay("");

        // Save ammo state and disable script
        gunScript.SaveAmmoState();
        gunScript.enabled = false;

        // Inform WeaponManager to drop the gun
        weaponManager.DropGun();

        // Clear prompt after drop
        UIManager.instance.UpdatePromptMessage("");
    }
}
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
    private UIManager _uiManager;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        
        originalPosition = gunContainer.localPosition;
        originalRotation = gunContainer.localRotation;
        //Setup
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

        //Check if player is in range and "E" is pressed while looking at the weapon
        if (!equipped && _inputManager._onFootActions.Interact.triggered && !slotFull)
        {
            Ray ray = fpsCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickUpRange))
            {
                if (hit.transform == transform)
                {
                    PickUp();
                }
            }
        }

        //Drop if equipped and "Q" is pressed
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

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        // Disable the Ammo UI
        UIManager.instance.UpdateAmmoDisplay("");
        
        //Disable script
        gunScript.enabled = false;
    }
}
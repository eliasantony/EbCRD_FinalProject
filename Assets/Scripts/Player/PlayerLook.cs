using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    
    public float xSensitivityWhileAiming = 10f;
    public float ySensitivityWhileAiming = 10f;
    
    private GameObject _gunHolder;
    private bool _weaponEquipped;
    
    void Start()
    {
        _gunHolder = GameObject.FindGameObjectWithTag("GunHolder");
    }

    private void Update()
    {
        _weaponEquipped = _gunHolder.transform.childCount > 0;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        if (_weaponEquipped && Input.GetKey(KeyCode.Mouse1))
        {
            xRotation -= (mouseY * Time.deltaTime) * ySensitivityWhileAiming;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivityWhileAiming);
        }
        else
        {
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
}

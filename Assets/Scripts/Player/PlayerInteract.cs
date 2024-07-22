using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask;
    private PlayerUI _playerUI;
    private InputManager _inputManager;
    
    void Start()
    {
        _camera = GetComponent<PlayerLook>().cam;
        _playerUI = GetComponent<PlayerUI>();
        _inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerUI.UpdateText(String.Empty);
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, interactableMask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                _playerUI.UpdateText(interactable.promptMessage);
                if (_inputManager._onFootActions.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}

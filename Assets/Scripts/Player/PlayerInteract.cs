using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask;
    private InputManager _inputManager;
    
    void Start()
    {
        _camera = GetComponent<PlayerLook>().cam;
        _inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, interactableMask))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                UIManager.instance.UpdatePromptMessage(interactable.promptMessage);
                if (_inputManager._onFootActions.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
        else
        {
            UIManager.instance.UpdatePromptMessage("");
        }
    }
}
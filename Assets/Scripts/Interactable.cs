using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    [SerializeField] public String promptMessage;

    // This function is called when the player interacts with the object
    public void BaseInteract()
    {
        if(useEvents)
            GetComponent<InteractionEvent>().onInteract.Invoke();
        Interact();
    }
    
    // No Code in this function, only a template to be overridden by subclasses
    protected virtual void Interact(){}
}

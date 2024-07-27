using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    public void BaseInteract() //accessible by Player
    {
        Interact();
    }

    protected virtual void Interact()
    {
     
    }
}

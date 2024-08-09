using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool isDoorOpen = false;

    protected override void Interact()
    {
        isDoorOpen = !isDoorOpen;

        if (door != null)
        {
            door.GetComponent<Animator>().SetBool("isOpen", isDoorOpen);
        }
    }
}

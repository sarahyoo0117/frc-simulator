using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{
    protected override void Interact()
    {
        PlayerPickUpItem playerHand = FindObjectOfType<PlayerPickUpItem>(); //TODO
        if (playerHand != null)
        {
            playerHand.PickupItem(gameObject);
        }
    }
}

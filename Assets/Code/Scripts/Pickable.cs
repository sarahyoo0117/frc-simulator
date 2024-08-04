using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Pickable : Interactable
{
    private Rigidbody itemRb;
    private Collider itemCollider;

    protected override void Interact()
    {
        PlayerHand hand = FindObjectOfType<PlayerHand>();
        if (hand != null)
        {
            hand.PickupItem(gameObject);
        }
    }
    public void OnPickUp(Transform hand)
    {
        transform.parent = hand.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        itemRb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();

        if (itemRb != null && itemCollider != null)
        {
            itemRb.isKinematic = true;
            itemCollider.enabled = false;
        }
    }

    public void OnDrop()
    {
        gameObject.transform.parent = null;

        if (itemRb != null && itemCollider != null)
        {
            itemRb.isKinematic = false;
            itemCollider.enabled = true;
        }
    }
}

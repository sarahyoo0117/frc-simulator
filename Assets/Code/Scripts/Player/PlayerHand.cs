using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public GameObject hand;
    public GameObject Item;
    public bool hasItem;
    public bool hasNote;

    private PlayerInputManager m_inputManager;

    private void Start()
    {
        m_inputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (hasItem)
            Item.transform.position = hand.transform.position;

        if (m_inputManager.onFoot.Dump.triggered)
        {
            DropItem();
        }
    }

    public void PickupItem(GameObject pickedItem)
    {
        if (pickedItem != null && hasItem == false)
        {
            Item = pickedItem;

            Pickable pickable = Item.GetComponent<Pickable>();

            if (pickable != null)
            {
                pickable.OnPickUp(hand.transform);
                hasItem = true;

                if (Item.GetComponent<Note>() != null)
                    hasNote = true;
            }
        }
    }

    public void DropItem()
    {
        if (Item != null && hasItem)
        {
            Pickable pickable = Item.GetComponent<Pickable>();

            if (pickable != null)
            {
                pickable.OnDrop();
                Item = null;
                hasItem = false;

                if (hasNote) hasNote = false;
            }
        }
    }
}

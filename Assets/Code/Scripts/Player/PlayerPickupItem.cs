using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpItem : MonoBehaviour
{
    [SerializeField]
    private GameObject m_hand;
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
            Item.transform.position = m_hand.transform.position;

        if (m_inputManager.onFoot.Dump.triggered && hasItem)
        {
            Item.GetComponent<Rigidbody>().isKinematic = false;
            Item.transform.parent = null;
            hasItem = false;

            if (hasNote) hasNote = false;
        }
    }

    public void PickupItem(GameObject pickedItem)
    {
        if (pickedItem != null && hasItem == false)
        {
            Item = pickedItem;
            Item.GetComponent<Rigidbody>().isKinematic = true;
            Item.transform.parent = m_hand.transform;
            Item.transform.position = m_hand.transform.position;
            hasItem = true;

            if (Item.GetComponent<Note>() != null)
                hasNote = true;
        }
    }
}

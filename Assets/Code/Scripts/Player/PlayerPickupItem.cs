using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpItem : MonoBehaviour
{
    [SerializeField]
    private GameObject m_hand;
    public GameObject Item;
    public bool hasItem;

    private PlayerInputManager m_inputManager;

    private void Start()
    {
        m_inputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (m_inputManager.onFoot.Dump.triggered && hasItem)
        {
            Item.GetComponent<Rigidbody>().isKinematic = false;
            Item.transform.parent = null;
            hasItem = false;
        }
    }

    public void PickupItem(GameObject pickedItem)
    {
        if (pickedItem != null && !hasItem)
        {
            Item = pickedItem;
            Item.GetComponent<Rigidbody>().isKinematic = true;
            Item.transform.position = m_hand.transform.position;
            Item.transform.parent = m_hand.transform;
            hasItem = true;
        }
    }
}

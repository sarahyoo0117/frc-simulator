using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNoteInsert : Interactable
{
    [Header("Note Settings")]
    public GameObject loadedNote;
    public Transform notePosition;
    public bool isLoaded;

    private void Update()
    {
        if (isLoaded && loadedNote != null)
        {
            loadedNote.transform.localPosition = Vector3.zero;
            loadedNote.transform.localRotation = Quaternion.identity;
        }
    }

    public void InsertNote(GameObject note , PlayerHand hand)
    {
        if (isLoaded || loadedNote != null) return;

        note.transform.parent = notePosition;
        note.transform.localPosition = Vector3.zero;
        note.transform.localRotation = Quaternion.identity;

        loadedNote = note;
        isLoaded = true;

        hand.Item = null;
        hand.hasItem = false;
        hand.hasNote = false;
    }

    public void UnloadNote(PlayerHand hand)
    {
       if (!isLoaded || loadedNote == null) return;

        hand.PickupItem(loadedNote);

        loadedNote = null;
        isLoaded = false;
    }
}

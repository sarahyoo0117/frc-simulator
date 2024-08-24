using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitbotNoteInsert : RobotNoteInsert
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void InsertNote(GameObject note, PlayerHand hand)
    {
        m_animator.Play("Kitbot_Shoot");
        base.InsertNote(note, hand);
    }

    public override void UnloadNote(PlayerHand hand)
    {
        base.UnloadNote(hand);
    }
}

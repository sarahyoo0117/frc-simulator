using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwervebotNoteInsert : RobotNoteInsert
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
        //TODO: m_animator.Play("insertAnim");
        base.InsertNote(note, hand);
    }

    public override void UnloadNote(PlayerHand hand)
    {
        base.UnloadNote(hand);
    }
}

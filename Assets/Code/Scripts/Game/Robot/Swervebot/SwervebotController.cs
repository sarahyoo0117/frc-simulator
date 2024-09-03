using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwervebotController : RobotController
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Feed()
    {
        base.Feed();
    }

    public override void Shoot()
    {
        if (m_insert.isLoaded && readyToShoot)
        {
            StartCoroutine(ShootNoteDelayed(0.5f));
        }
    }

    IEnumerator ShootNoteDelayed(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ShootNote(shootForce);
    }

    public override void OnShootAnimationComplete()
    {
        base.OnShootAnimationComplete();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachainBullet : BulletBase
{
    Vector3 moveValue= Vector3.right;
    protected override void OnEnable()
    {
        base.OnEnable();
        moveValue = Vector3.right;
        moveValue *= transform.localScale.x;

    }
    private void FixedUpdate()
    {
        transform.Translate(moveValue * bulletMoveSpeed * Time.deltaTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{

    public override void OnFire(Transform aimPostion, Vector3 aimVector, Vector3 localScale, int sign)
    {
        //기본 총알 발사

        if (readyToFire)
        {
            readyToFire = false;
            PoolFactory.Instance.GetPistolBullet(aimPostion.position, aimVector, localScale);
            if (sign != 2)
            {
                StartRaedyToFireDelay(fireDelay);
            }
            else
            {
                StartRaedyToFireDelay(fireDelay * sitDelay);
            }
        }
    }
}

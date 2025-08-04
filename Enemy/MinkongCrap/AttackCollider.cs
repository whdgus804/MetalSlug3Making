using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    Enemy_MinkongCrap minkongCrap;
    private void Awake()
    {
        minkongCrap=GetComponentInParent<Enemy_MinkongCrap>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            minkongCrap.OnAttackTrigger();
        }
    }
}

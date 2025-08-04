using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyNPC : NPC_Base
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DropItem();
        }
    }
    protected override void DropItem()
    {
        anim.SetTrigger("Item");
        gameObject.layer = 10;
        base.DropItem();
        WaitMove(1.5f);
        moveSpeed = moveSpeed * 3;
        StartCoroutine(ItemDropDelay());
    }
    IEnumerator ItemDropDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(dropItem,itemPos.position,Quaternion.identity);
    }

}

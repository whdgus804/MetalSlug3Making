using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpMorden : EnemyHP
{
    Animator pumpAnim;
    Animator mordenAnim;

    BoxCollider2D hitcol;
    private void Awake()
    {
        pumpAnim=transform.GetChild(1).GetComponent<Animator>();
        mordenAnim=transform.GetChild(0).GetComponent<Animator>();
        hitcol=GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        PumpStart();
    }
    void PumpStart()
    {
        mordenAnim.gameObject.SetActive(true);
        float rebelTime=mordenAnim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(PumpStart(rebelTime));
    }
    IEnumerator PumpStart(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        hitcol.enabled = true;  
        pumpAnim.SetBool("Pump", true);
    }
    protected override void OnDie()
    {
        pumpAnim.SetBool("Pump", false);
        mordenAnim.SetTrigger("Die");
        hitcol.enabled = false;
        StartCoroutine(RespawnMorden());
    }
    IEnumerator RespawnMorden()
    {
        float time = mordenAnim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time+3.0f);
        PumpStart();

    }
}

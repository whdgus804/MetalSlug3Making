using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDead : MonoBehaviour
{
    ///// <summary>
    ///// �÷��̾ ��Ȱ�� ��ġ
    ///// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// �÷��̾ ���������� ����� ����Ʈ
    /// </summary>
    [SerializeField] GameObject waterEffect;

    

    /// <summary>
    /// ���� ���� Ƣ������µ��� ����Ʈ
    /// </summary>
    [SerializeField] GameObject waterExplosionEffect;
    /// <summary>
    /// ���� Ƣ�����  ����Ʈ�� ���� ����
    /// </summary>
    [SerializeField] Transform waterEffectPos;
    BoxCollider2D boxCollider2D;
    PlayerCameraMove playerCamera;
    private void Awake()
    {
        playerCamera = FindAnyObjectByType<PlayerCameraMove>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Vector2 vec = new Vector2(collision.transform.position.x, -4.63f);
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();    //��ġ������ ���� ��ũ��Ʈ �ޱ�
            if (health.HP > 0)
            {
                GameObject water=Instantiate(waterEffect,vec,Quaternion.identity);
                PlayerAnimation anim=collision.gameObject.GetComponent<PlayerAnimation>();
                Rigidbody2D rigid=collision.gameObject.GetComponent<Rigidbody2D>();
                health.spanwPosition = spawnPos;                          //��ġ ����
                anim.Dead_Water();
                health.HP = 100;
                playerCamera.CameraStayHere();
                boxCollider2D.enabled = false;
                StartCoroutine(ColliderDisabler(rigid,water));
            }
            else
            {
                Instantiate(waterExplosionEffect, new Vector2(collision.transform.position.x, waterEffectPos.position.y), Quaternion.identity);
            }
        }
        else
        {
            collision.gameObject.SetActive(false);  
            Instantiate(waterExplosionEffect, new Vector2(collision.transform.position.x, waterEffectPos.position.y), Quaternion.identity);
        }
    }
    IEnumerator ColliderDisabler(Rigidbody2D rigid,GameObject water)
    {
        rigid.AddForce(Vector2.up * 200.0f);
        yield return new WaitForSeconds(0.7f);
        Animator anim=water.GetComponent<Animator>();
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Sink");
        boxCollider2D.enabled=true;
    }
}

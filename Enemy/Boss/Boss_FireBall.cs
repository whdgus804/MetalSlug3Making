using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// �߻� ����Ʈ ������Ʈ
    /// </summary>
    [SerializeField] GameObject effect;
    /// <summary>
    /// ��������Ʈ
    /// </summary>
    [SerializeField] GameObject hugeExplosion;
    /// <summary>
    /// �÷��̾�
    /// </summary>
    PlayerHealth player;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody2D rigid;

    private void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
        rigid=GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        
        rigid.AddForce(Vector2.right*moveSpeed);        //�Ѿ� �߻�
        StartCoroutine(FireEffect());                   //�߻� ����Ʈ
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            StartCoroutine(FireBallFall());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.HP = 1;
            Explosion();
        }else if (collision.gameObject.CompareTag("Ground"))
        {
            Explosion();
        }
    }
    /// <summary>
    /// ���� �Լ� 
    /// </summary>
    void Explosion()
    {
        //���� �ִϸ��̼�
        Vector2 vector2 = transform.position;                       //��ġ �ޱ�
        vector2.y += 1.7f;                                          //��ġ�� ����
        Instantiate(hugeExplosion, vector2, Quaternion.identity);   //���� ����Ʈ
        Destroy(gameObject);                                        //����
    }
    /// <summary>
    /// ���̾�� �������� �ڷ�ƾ �Լ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator FireBallFall()
    {
        rigid.drag = 3;                             //�����߰�
        yield return new WaitForSeconds(0.15f);     //��ٸ���
        rigid.gravityScale = 3.0f;                  //������ ��������
    }
    /// <summary>
    ///  �ڿ� ����ٴϴ� ����Ʈ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator FireEffect()
    {
        WaitForSeconds wait=new WaitForSeconds(0.2f);
        for (int i = 0; i<200; i++)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            yield return wait;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    /// <summary>
    /// �ּ� ���� ƥ ��
    /// </summary>
    [SerializeField] float minUpAddForce;
    /// <summary>
    /// �ּ� ������ ƥ��
    /// </summary>
    [SerializeField] float minSideAddForce;
    /// <summary>
    /// ���� Ƣ����� ��
    /// </summary>
    [SerializeField] float upAddForce;
    /// <summary>
    /// ������ ƨ������
    /// </summary>
    [SerializeField] float sideAddForce;
    /// <summary>
    /// �ִϸ����� ������Ʈ
    /// </summary>
    Animator anim;
    /// <summary>
    /// �ش� ��ü�� ������ٵ�
    /// </summary>
    Rigidbody2D rigid;
    
    /// <summary>
    /// ��ü�� ���������� ���� �ִϸ��̼��� ����ǰ� �Ҷ� ������ ����
    /// </summary>
    int ground_to_Hash = Animator.StringToHash("Ground");
    private void Awake()
    {
        anim = GetComponent<Animator>();        //������Ʈ �ޱ�
        rigid=GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger(ground_to_Hash);
        }
    
    }
    private void OnEnable()
    {
        float upforerand=Random.Range(minSideAddForce, upAddForce);
        float sideforceRand=Random.Range(minSideAddForce, sideAddForce);
        rigid.AddForce(new Vector2(sideforceRand * transform.localScale.x, upforerand));
    }
}

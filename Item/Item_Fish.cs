using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fish : ItemBase
{
    [SerializeField] Transform waterEffectPos;
    /// <summary>
    /// �������� �����ð�
    /// </summary>
    [SerializeField] float disableTime;
    /// <summary>
    /// Ƣ������� ����Ʈ
    /// </summary>
    [SerializeField] GameObject waterEffect;
    /// <summary>
    /// ���� Ƣ������� ��
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// ������ Ƣ�� ��
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody2D rigid;
    protected override void Awake()
    {
        base.Awake(); 
        rigid = GetComponent<Rigidbody2D>();        //������ �ٵ� �ޱ�
    }
    private void OnEnable()
    {
        if(waterEffect != null)
            Instantiate(waterEffect, waterEffectPos.position, Quaternion.identity);          //����Ʈ ����
        rigid.AddForce(new Vector2(sideForce, upForce));                            //Ƣ�������
        DisableTime(disableTime);                                                   //���� �� �������� �������� �Լ� ����
    }
}

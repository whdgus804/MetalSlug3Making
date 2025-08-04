using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoamStick : GrenadeBase
{

    [SerializeField] float upAddForece;
    [SerializeField] float sideAddForece;
    /// <summary>
    /// ���� ���� �������� ��Ÿ���� ����
    /// </summary>
    bool readToExplosion = false;


    /// <summary>
    /// ������ �ٵ�
    /// </summary>
    Rigidbody2D rigid;
    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();    
    }
    private void OnEnable()
    {
        readToExplosion=false;
        rigid.AddForce(new Vector2(transform.localScale.x * sideAddForece, upAddForece));
    }
    protected override void GroundHit()
    {
        if (!readToExplosion)
        {
            rigid.AddForce(new Vector2 (transform.localScale.x * sideAddForece*0.7f, upAddForece* 0.7f));
            readToExplosion = true;
        }
        else
        {
            Explosion();
        }
    }

    protected override void Explosion()
    {
        base.Explosion();
        PoolFactory.Instance.GetExplosion_A(transform.position);
        gameObject.SetActive(false);

    }
}

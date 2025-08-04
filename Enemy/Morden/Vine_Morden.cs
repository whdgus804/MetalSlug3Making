using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine_Morden : Morden
{
    /// <summary>
    /// Ÿ�� ������ ������ ����Ʈ
    /// </summary>
    SpringJoint2D springJoint;
    /// <summary>
    /// �ش� ��ü�� ������ٵ�
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// ������ Ÿ�� ���� ���� �߸� ��
    /// </summary>
    [SerializeField] GameObject leaf;
    /// <summary>
    /// ����߸� ���� ��ȯ�� ���
    /// </summary>
    [SerializeField] Transform leafSpawnPosition;
    /// <summary>
    /// ó���� ��ȯ�� ���� ���� ����߷��� ������ ó������ ������ ������ ��Ÿ���� ����
    /// </summary>
    [HideInInspector]public bool first = false;
    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();                    
        springJoint=GetComponentInParent<SpringJoint2D>();
    }
    private void Start()
    {
        secMoveSpeed = 0.0f;
    }
    private void OnEnable()
    {
        anim.SetTrigger("Vine");
        float time = anim.GetCurrentAnimatorStateInfo(0).length;            //�ִϸ��̼� �ð� �ޱ�
        StartCoroutine(RideTime(time));                                     //���� �ð����� �ڷ�ƾ ����
        if(first)
            StartCoroutine(LeavesSpawn());
    }
    /// <summary>
    /// ���� ������ Ÿ�� �ð�
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator RideTime(float time)
    { 
        yield return new WaitForSeconds(time);
        Ejaculation();              //���� �ð� �ڿ� �������� ����
    }
    /// <summary>
    /// �������� �������� �Լ�
    /// </summary>
    public void Ejaculation()
    {
        springJoint.connectedBody = null;       //������ ����Ʈ ���� 
        transform.parent = null;                //�θ� ����
        rigid.drag = 3.0f;                      //���� ���������� �ǵ� �� ��ġ�� ����߸���
    }
    public override void Landing()
    {
        base.Landing();
        secMoveSpeed = 1.0f;                    //���� ������ �����̰� �ϱ�
    }
    /// <summary>
    /// �������� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator LeavesSpawn()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(leaf,leafSpawnPosition.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

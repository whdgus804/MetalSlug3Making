using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ship : EnemyHP
{
    /// <summary>
    /// ���� �ִϸ�����
    /// </summary>
    Animator anim;
    /// <summary>
    /// �� ���� ���� �ִϸ�����
    /// </summary>
    Animator waterAnim;
    /// <summary>
    /// �������� �����ϴ� ��ũ��Ʈ
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// �ν��� ������ ��
    /// </summary>
    [SerializeField] GameObject brokenShip;


    [SerializeField] Transform explosionRangeA;
    [SerializeField] Transform explosionRangeB;
    /// <summary>
    /// ��������A
    /// </summary>
    [SerializeField] GameObject woodA;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] GameObject woodB;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] GameObject woodC;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] GameObject woodD;
    /// <summary>
    /// ���� ������Ʈ
    /// </summary>
    [SerializeField] GameObject smoke;
    /// <summary>
    /// ���Ⱑ ����� ��ġ
    /// </summary>
    [SerializeField] Transform smokePos;

    bool isAlive = true;
    private void Awake()
    {
        movePlatform = GetComponent<MovePlatform>();
        anim = GetComponent<Animator>();
        waterAnim=transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void OnDie()
    {
        if(isAlive)
        {
            isAlive = false;
            base.OnDie();
            anim.SetTrigger("Broken");
            StartCoroutine(Explosion());
        }
    }

    /// <summary>
    /// ���� �������� �����ϴ� �Լ�
    /// </summary>
    public void MoveStart()
    {

        movePlatform.PlatformStart(movePlatform.moveSpeed);     //������ ����
        StartCoroutine(smoking());
    }
    /// <summary>
    /// ���� �������� ���ߴ� �Լ�
    /// </summary>
    public void MoveStop()
    {
        movePlatform.MoveStop();                //������ ����
    }
    /// <summary>
    /// �谡 �ν����� �Լ� �ִϸ����Ϳ��� ȣ���
    /// </summary>
    public void Borken()
    {
        gameObject.SetActive(false);            //������Ʈ ��Ȱ��
        brokenShip.transform.parent = null;     //�ν��� �� �θ�����
        brokenShip.SetActive(true);             //�ν����� Ȱ��
        Destroy(gameObject);                    //������Ʈ ����

    }

    /// <summary>
    /// �谡 �ν����� ���� �� ���� ���� ����Ʈ�� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Explosion()
    {
        for(int i=0;i<8;i++)
        {
            int randint = Random.Range(0, 4);           //�������� ������ ���� ���� �ޱ�
            GameObject obj = null;
            switch (randint)
            {
                case 0:
                    obj = woodA;
                    break;
                case 1:
                    obj= woodB;
                    break;
                case 2:
                    obj= woodC;
                    break;
                default:
                    obj = woodD;
                    break;
            }
            Vector2 vec = new Vector2(Random.Range(explosionRangeA.position.x, explosionRangeB.position.x),
                Random.Range(explosionRangeA.position.y, explosionRangeB.position.y));      //������ġ �ޱ�
            yield return new WaitForSeconds(0.3f);
            Instantiate(obj,vec,Quaternion.identity);       //���� ��ġ�� �������� ����
            PoolFactory.Instance.Get_N_Explosion_M(vec);    //���� ��ġ�� ���� ����Ʈ ����
        }
    }
    /// <summary>
    /// ���⸦ �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator smoking()
    {
        Instantiate(smoke, smokePos.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(smoking());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ship_Broken : EnemyHP
{
    [SerializeField] float brokenTime;
    [SerializeField] float sinkedTime;
    [SerializeField] float sinkSpeed;

    [SerializeField] Transform explosionA;
    [SerializeField] Transform explosionB;
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
    /// �÷��̾ Ÿ�� ���� ��ũ��Ʈ
    /// </summary>
    [SerializeField] Ship ship;
    Animator wateranim;
    bool onBreak=false;
    private void Awake()
    {
        wateranim=GetComponentInChildren<Animator>();
    }
    protected override void OnDie()
    {
        if (!onBreak)
        {
            onBreak = true;
            wateranim.SetTrigger("Sink");
            StartCoroutine(Sink());
            StartCoroutine(Sinked());
        }
    }
    /// <summary>
    /// ����ɴ� �Լ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator Sink()
    {
        wateranim.transform.parent = null;                                  //�踸 ����ɰ� ������ �θ� ���� 
        yield return new WaitForSeconds(brokenTime);                        
        Vector2 vec = Vector2.down;         
        while (true)                                                        //������ õõ�� �̵�
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(vec * sinkSpeed * Time.fixedDeltaTime);
        }
    }
    IEnumerator Sinked()
    {
        for(int i = 0; i <8; i++)                                               //�μ����� ƨ�ܳ��� ���� ��ǰ �������� �ޱ�
        {
            int randint = Random.Range(0, 4);
            GameObject obj = null;
            switch (randint)
            {
                case 0:
                    obj = woodA;
                    break;
                case 1:
                    obj = woodB;
                    break;
                case 2:
                    obj = woodC;
                    break;
                default:
                    obj = woodD;
                    break;
            }
            Vector2 vector2=new Vector2(Random.Range(explosionA.position.x,explosionB.position.x),Random.Range(explosionA.position.y,explosionB.position.y));       //��ġ �������� �ޱ�
            yield return new WaitForSeconds(sinkedTime*0.1f);       
            PoolFactory.Instance.Get_N_Explosion_M(vector2);            //�� ��ġ�� ���� ����Ʈ ����
            Instantiate(obj,vector2,Quaternion.identity);               //���� ����Ʈ ���� ��ġ�� ���� ���� ����

        }
        wateranim.SetTrigger("Sink");           //�ִϸ��̼� �� ����
        ship.MoveFront();                       //�÷��̾�谡 ������ ���ư����ϱ� 
        Destroy(gameObject);
    }
}

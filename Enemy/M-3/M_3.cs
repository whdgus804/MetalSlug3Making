using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_3 : EnemyHP
{
    /// <summary>
    /// ������ �̻���
    /// </summary>
    [SerializeField] GameObject missile;
    /// <summary>
    /// �̻����� ������ ��ġ �迭
    /// </summary>
    [SerializeField]
    Transform[] missilPos;
    /// <summary>
    /// ���� ����Ʈ
    /// </summary>
    [SerializeField] GameObject fireEffect;
    /// <summary>
    /// �߻� ����Ʈ�� ���� ��ġ �迭
    /// </summary>
    [SerializeField] Transform[] fireEffectPos;
    
    /// <summary>
    /// �ı��� ���� �ν��� �ٸ� ������Ʈ
    /// </summary>
    [SerializeField] GameObject bridge;
    /// <summary>
    /// �ν��� �ٸ� ������Ʈ
    /// </summary>
    [SerializeField] GameObject brokenBridge;
    /// <summary>
    /// ���� ����Ʈ
    /// </summary>
    [SerializeField] GameObject hugeExplosion;
    /// <summary>
    /// ��ȯ�� ��
    /// </summary>
    [SerializeField] GameObject morden;
    /// <summary>
    /// �ٸ��� �ν����� ������ ����
    /// </summary>
    [SerializeField] GameObject woodPartA;
    /// <summary>
    /// �ٸ��� �ν����� �󼺵� ����
    /// </summary>
    [SerializeField] GameObject woodPartB;
    /// <summary>
    /// ���� ��ġ
    /// </summary>
    [SerializeField] Transform explosionPos;
    /// <summary>
    /// ���� �������� ������ ��ũ��Ʈ
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;
    [SerializeField] Ship ship;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        movePlatform=ship.GetComponent<MovePlatform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            movePlatform.MoveStop();                                    //�� ����
            StartCoroutine(MordenSpawn());                              //�� ��ȯ
            ship.backGroundMove.StopAllCoroutines();
            //StartCoroutine(OnFireCouroutine(2.0f));
        }
    }

    protected override void OnDie()
    {
        gameObject.layer = 10;
        //Ư�� �� ��ŭ �������� ��ȯ
        for(int i = 0; i < 3; i++)
        {
            Instantiate(woodPartB,transform.position,Quaternion.identity);
            Instantiate(woodPartA,transform.position,Quaternion.identity);
        }
        
        bridge.SetActive(false);                                                    //�ٸ� ��Ȱ��ȭ
        brokenBridge.SetActive(true);                                               //�ν��� �ٸ� Ȱ��ȭ
        Instantiate(hugeExplosion, explosionPos.position, Quaternion.identity);     //���� ����Ʈ ����
        movePlatform.PlatformStart(movePlatform.moveSpeed);                         //�谡 �ٽ� �����̰� �ϱ�
        ship.backGroundMove.OnMoveAuto();
        Destroy(gameObject);                                                        //������Ʈ ����
    }
    /// <summary>
    /// �̻����� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="Delay">�߻� ������</param>
    /// <returns></returns>
    IEnumerator OnFireCouroutine(float Delay)
    {
        anim.SetTrigger("Fire");                                                    //�ִϸ��̼� ���
        StartCoroutine(FireEffect());                                               //����Ʈ ��� �Լ� ����
        yield return new WaitForSeconds(Delay);                                     //�߻� ������ ��ٸ���
        for(int j =0;  j<3; j++)                                                    //�߻� 4���� 3�� �ݺ�
        {
            for (int i = 0; i < missilPos.Length; i++)                              //�߻� ��ġ�� �������� �޾� �߻� 
            {
                int rand=Random.Range(0, missilPos.Length);
                Instantiate(missile, missilPos[rand].position,Quaternion.identity);     //�̻��� ����
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return new WaitForSeconds(3.0f);                                      //���� ���� ������
        StartCoroutine(MordenSpawn());                                              //�� ��ȯ �ڷ�ƾ ����
    }
    /// <summary>
    /// �߻� ����Ʈ�� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator FireEffect()
    {
        int sign = 0;                                                                       //������ ����� �ʰ� ���� ����
        for (int i = 0;i < missilPos.Length*3;i++)                                            //�̻����� �߻� ��ŭ �ݺ�
        {
            Instantiate(fireEffect, fireEffectPos[sign].position, Quaternion.identity);         //����ȯ
            sign++;                                                                            //���� ����
            if (sign == 2)                                                                      //���� ������ ����Ʈ ��ġ���� ������
                sign = 0;                                                                       //���� ����
            yield return new WaitForSeconds(0.2f);                                              //������
        }
    }
    /// <summary>
    /// ���� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MordenSpawn()
    {
        for(int i = 0; i < 5; i++)
        {
            anim.SetTrigger("Morden");                                          //�ִϸ��̼� ���
            int rand = Random.Range(0, missilPos.Length);                       //������ �ޱ�
            yield return new WaitForSeconds(1.0f);                              //������
            Instantiate(morden, missilPos[rand].position, Quaternion.identity); //������ġ�� ������
        }
        yield return new WaitForSeconds(2.0f);                                  //���� ���� ������
        StartCoroutine(OnFireCouroutine(2.0f));                                 //�̻��� �߻� �ڷ�ƾ ����
    }

}

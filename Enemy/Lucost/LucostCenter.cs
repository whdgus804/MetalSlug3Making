using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LucostCenter : MonoBehaviour
{
    GameObject chenamachaine;
    /// <summary>
    /// �߽��� �̵��� ��ġ
    /// </summary>
    Transform centerPosL;
    /// <summary>
    /// �߽����� �̵��� ������ ��ġ
    /// </summary>
    Transform centerPosR;
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;
    Enemy_Lucost lucost;
    /// <summary>
    /// �÷��̾ ������ ����ĥ����
    /// </summary>
    [SerializeField] float runawayRange;
    PlayerHealth player;
    Transform lucostTransform;

    /// <summary>
    /// �¿츦 �а����� ���� true�� ����
    /// </summary>
    bool targetSign=true;

    
    private void Awake()
    {
        //if (chenamachaine == null)
        //{
        //    Debug.Log("Point==null");
        //}
        //else
        //{
        //    centerPosL = chenamachaine.transform.GetChild(2);
        //    Debug.Log(centerPosL.name);
        //    centerPosR = chenamachaine.transform.GetChild(1);
        //    Debug.Log(centerPosR.name);
        //}
        lucostTransform = transform.GetChild(0);
        CinemachineVirtualCamera camera = FindAnyObjectByType<CinemachineVirtualCamera>();          //�����̴� ī�޶� �޾ƿ���
        chenamachaine =camera.gameObject;                                                           //���� ī�޶� ������Ʈ�� �ޱ�
        centerPosL=chenamachaine.transform.GetChild(2);                                             //�ش� ������Ʈ�� �ڽ��� ��ġ �ޱ�
        centerPosR=chenamachaine.transform.GetChild(1);
        lucost=GetComponentInChildren<Enemy_Lucost>();                                               
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnEnable()
    {
        //ChangeMove();                       //���� �̵�
        //lucost.Left(targetSign);
        StartCoroutine(MoveLeft()); 
        StartCoroutine(RunRange());
        //StartCoroutine(RePosition());
    }
    //IEnumerator RePosition()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    centerPosL = chenamachaine.transform.GetChild(2);
    //    centerPosR = chenamachaine.transform.GetChild(1);
    //    StartCoroutine(RePosition());
    //}
    /// <summary>
    /// ���� �̵��� �����ϴ� �ڷ�ƾ �Լ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCoolTime()
    {
        yield return new WaitForSeconds(1.5f);              //���� �ð� ��ٸ���
        StartCoroutine(RunRange());                         //�÷��̾���� �Ÿ����
        lucost.Runaway(false);                              //������ �Լ� ����
    }
    /// <summary>
    /// �ݴ��� ���ͷ� �̵��ϴ� �Լ�
    /// </summary>
     void ChangeMove()
    {
        StopAllCoroutines();                //��� �ڷ�ƾ ����
        //bool sign = false;
        //float distanc = player.transform.position.x - transform.position.x;
        //if (distanc < 0)
        //{
        //    //�÷��̾ ����
        //    StartCoroutine(MoveRight());
        //    sign = false;
        //    if(targetSign != sign)
        //    {
        //        lucost.LookPlayerStart();
        //    }
        //}
        //else
        //{
        //    StartCoroutine(MoveLeft());
        //    sign = true;
        //    if (targetSign != sign)
        //    {
        //        lucost.LookPlayerStart();
        //    }
        //}
        //targetSign = sign;
        if (targetSign)
        {
            StartCoroutine(MoveRight());
            targetSign = false;
        }
        else
        {
            StartCoroutine (MoveLeft());
            targetSign=true;
        }
        lucost.LookPlayerStart();

        //StopAllCoroutines();                //�̵�����
        //targetSign = !targetSign;           //��������
        
        //lucost.LookPlayerStart();
        //if (targetSign)                      //������ ���� �ڷ�ƾ ���౸��
        //{
        //    StartCoroutine(MoveLeft());     //true�� �������� �̵�
        //}
        //else
        //{
        //    StartCoroutine (MoveRight());   //false�� ���������� �̵�
        //}
        //StartCoroutine(RePosition());
    }


    /// <summary>
    /// ������ ���� �߽����� ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveLeft()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, centerPosL.position, moveSpeed * Time.deltaTime);      //��ġ�� �������� ���󰡰� �ϱ� 
            yield return null;
        }
    }
    /// <summary>
    /// �������� ���� �߽����� ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRight()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, centerPosR.position, moveSpeed * Time.deltaTime);      //��ġ�� ���������� ���󰡰� �ϱ�
            yield return null;
        }
    }
    /// <summary>
    /// �߽����� �̵��ϴ� ���� ���ߴ� �Լ�
    /// </summary>
    /// <param name="removeTime">�ٽ� �����̱� ���� �ɸ� �ð�</param>
    public void StopMoveCenter(float removeTime)
    {
        StopAllCoroutines();                            //��� �ڷ�ƾ ����
        StartCoroutine(ReStartMove(removeTime));        //���� �ð� �� �ٽ� ���� �̰� �ϱ�
    }
    /// <summary>
    /// ���� �� �ٽ� �̵��ϰ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="time">��ٸ� �ð�</param>
    /// <returns></returns>
    IEnumerator ReStartMove(float time)
    {
        yield return new WaitForSeconds(time);          //�ð� ��ٸ���
        
        StartCoroutine(MoveCoolTime());
        if (targetSign)                                 //������ ���� ���� ��ġ ���ϱ�
        {
            StartCoroutine(MoveLeft());                 //true�� ������ �߽����� �̵�
        }
        else                                            //false�� �������� �߽����� �̵�
        {
            StartCoroutine(MoveRight());
        }
    }
    /// <summary>
    /// �÷��̾���� �Ÿ��� ����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RunRange()
    {
        while (true)
        {
            float distanc = (player.transform.position-transform.position).sqrMagnitude;        //�÷��̾���� �Ÿ� ���
            if (distanc < runawayRange * runawayRange)                                          //���� �÷��̾ ������
            {
                //lucostTransform.position = transform.position;
                StartCoroutine (MoveCoolTime());                                                //��Ÿ�� ����
                lucost.Runaway(true);                                                           //���������� �˸�
                ChangeMove();                                                                   //���� �������� ��������
                //StopAllCoroutines();
            }
            yield return null;
        }
    }
    public void Attacking(bool nowAttack)
    {
        StopAllCoroutines();
        if(!nowAttack)
        {
            StartCoroutine(RunRange());
        }

    }
    public void OnDead()
    {
        StopAllCoroutines();
    }
}

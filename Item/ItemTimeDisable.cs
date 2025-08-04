using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTimeDisable : MonoBehaviour
{
    /// <summary>
    /// �������� ����� �ð�
    /// </summary>
    [SerializeField] float disabelTime;
    /// <summary>
    /// ���� ���� �Ŀ� ������� ��Ÿ���� ���� true�� ���� ���� �Ŀ� Ÿ�̸� ����
    /// </summary>
    [Tooltip("���� ���� �� Ÿ�̸� ����")]
    [SerializeField] bool groundDisable;
    /// <summary>
    /// ��ü�� �������� ��Ȱ��ȭ ���� ��Ÿ���� ���� true�� ��Ȱ��ȭ �Ѵ�
    /// </summary>
    [Tooltip("�������� ��� ����")]
    [SerializeField] bool disable;

    AnimationDisabler animDisabler;
    private void Awake()
    {
        animDisabler=GetComponent<AnimationDisabler>();   
    }
    private void OnEnable()
    {
        if(!groundDisable)                              //���� ���� �� �����ϴ°� �ƴ϶�� Ÿ�̸� ����
            StartCoroutine(DisableTime());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundDisable)                                      //���� ���� �� ���� �ǰԲ� ������ Ȱ�� ���ְ�
        {
            if (collision.gameObject.CompareTag("Ground"))      //���� �������
            {   
                StartCoroutine(DisableTime());                  //Ÿ�̸� ����
            }
        }
    }
    /// <summary>
    /// ���� �ð� �� �������� �����ϴ� �ڷ�ƾ �Լ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(disabelTime);   //��ٸ���
        if (disable)                                    //���� ��Ȱ��ȭ ������ �����ִٸ�
        {
            animDisabler.Twinkle();                     //��¦�� �� ��Ȱ��ȭ �ϱ�
        }
        else                                            //�ƴϸ�
        {
            animDisabler.Destoyer();                    //��¦�� �� �����ϱ�
        }
    }
}

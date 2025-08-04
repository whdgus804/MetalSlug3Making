using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    /// <summary>
    /// ������Ʈ�� �����ð�
    /// </summary>
    [SerializeField] float disableTime;
    /// <summary>
    /// ���� ƥ��
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// ������ ƥ��
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();                 //������ٵ� �ޱ�
    }

    private void OnEnable()
    {
        float up=Random.Range(upForce*0.8f, upForce);       //���� ���������� �ޱ�
        float side=Random.Range(-sideForce, sideForce);     //���� ���������� �ޱ�
        rigid.AddForce(new Vector2(side, up));              //���� ���������� Ƣ��
        StartCoroutine(DisableTimer());
    }
    /// <summary>
    /// ������Ʈ�� ���� �ð� �� ������ �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(disableTime);   //��ٸ���
        gameObject.SetActive(false);                    //������Ʈ ��Ȱ��ȭ
        //Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Misson1UI : MonoBehaviour
{
    //�̹����� �ް� ��ȣ�� ������ ����� ��ġ�� ���ʷ� �̵� ���� 0.3�� ����
    //���� �̹������� x�� ���� �� �ش� ��ġ�� �̵� 
    //start , complete �ǹ��ڸ� ���� �ް� y���� �ٸ� �ٸ� ��ġ ������ �ް� �ش� ��ġ�� �� ��Ģ���� �̵��ϰ��ϱ�
    //��� �̵������� ���� �ð� �� �������
    //������� ��ġ�� �ް� ������ ���� �� �ݴ� �������� �̵��ϰ� �ϱ�

    /// <summary>
    /// MISSION 1 �̹���
    /// </summary>
    [SerializeField] Image[] missionImage;
    /// <summary>
    /// START Ȥ�� COMLETE �̹���
    /// </summary>
    [SerializeField] Image[] startOrComplete;
    /// <summary>
    /// ��ó�� ��ġ
    /// </summary>
    [SerializeField] Vector2 firstPos;
    /// <summary>
    /// Start Ȥ�� COMPLETE ���� ��ġ
    /// </summary>
    [SerializeField] Vector2 secPos;
    //���� ���� �Ÿ�
    [SerializeField] float distance;

    [SerializeField] GameObject clearMenu;
    
    private void OnEnable()
    {
        
        StartCoroutine(MoveToMisson1UI());
    }
    /// <summary>
    /// UI������ ���� �ڷ�ƾ �Լ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToMisson1UI()
    {
        //float moveSpeed = 100.0f;
        for (int i = 0; i < missionImage.Length; i++)
        {
            MoveUI moveUI = missionImage[i].gameObject.GetComponent<MoveUI>();  //������Ʈ �ޱ�
            Vector2 vec = firstPos;                 //��ġ ���� 
            vec.x += distance*(i+1);                //�Ÿ� �� ����
            if (i == 7)                             //���� 1 ���ʸ� �� �ָ� �������� �ϱ�
                vec.x += distance;
            moveUI.tartgetPos = vec;                //��ǥ��ġ ����
            moveUI.sideMoveSpeed = 0.0f;            //�� ������ ����
            moveUI.moveSpeed = 300;                 //�̵��ӵ� ����
            missionImage[i].enabled = true;         //�̹��� Ȱ��ȭ
            //missionImage[i].rectTransform.anchoredPosition = vec;
            yield return new WaitForSeconds(0.3f);  //��ٸ���
        }
            StartCoroutine(CompleteOrStartUI());        //���� �ڷ�ƾ ����

    }
    /// <summary>
    /// complete Ȥ�� start ui ���� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator CompleteOrStartUI()
    {
        for(int i=0; i<startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //��ũ��Ʈ �ޱ�
            moveUI.sideMoveSpeed = 70.0f;               //�̵��ӵ� ����
        }
        for(int i = 0;i < startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //��ũ��Ʈ �ޱ�
            Vector2 vec = secPos;           //��ǥ ����
            vec.x += distance * (i + 1);        //�Ÿ� ����
            moveUI.tartgetPos = vec;            //��ǥ ����
            moveUI.sideMoveSpeed = 0.0f;        //�̵� ����
            moveUI.moveSpeed = 300;             //��ǥ�������� �̵� ����
            startOrComplete[i].enabled = true;     //Ȱ��ȭ
            yield return new WaitForSeconds(0.3f);      //��ٸ���
        }
        yield return new WaitForSeconds(2.0f);      //��ٸ���
        for (int i = 0; i < missionImage.Length; i++)   
        {
            MoveUI moveUI = missionImage[i].gameObject.GetComponent<MoveUI>(); //��ũ��Ʈ �ޱ�
            moveUI.MoveFar();               //�־����� �Լ� ����
            moveUI.DisableStart();
        }
        for (int i = 0; i < startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //��ũ��Ʈ �ޱ�
            moveUI.MoveFar();           //�־����� �Լ� ���त
            moveUI.DisableStart();

        }
        yield return new WaitForSeconds(2.0f);  //��ٸ���
        //Time.timeScale = 0.0f;
        if (clearMenu != null)
            clearMenu.SetActive(true);
    }
}

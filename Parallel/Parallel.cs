using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : MonoBehaviour
{
    ///// <summary>
    ///// �б��� ���� �� ī�޶� �����ϴ� ������Ʈ�� ��ũ��Ʈ
    ///// </summary>
    //[SerializeField] CameraPointReset setCamera;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        setCamera.ResetCameraPos();         //�÷��̾ ������ ī�޶� �������� �� �÷��̾� ���󰡱�
    //        gameObject.SetActive(false);    //���ӿ�����Ʈ ��Ȱ��
    //    }
    //}


    PlayerCameraMove playerCamera;
    private void Awake()
    {
        playerCamera=FindAnyObjectByType<PlayerCameraMove>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}

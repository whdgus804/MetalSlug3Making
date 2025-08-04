using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : MonoBehaviour
{
    ///// <summary>
    ///// 분기전 진입 전 카메라 고정하는 오브젝트의 스크립트
    ///// </summary>
    //[SerializeField] CameraPointReset setCamera;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        setCamera.ResetCameraPos();         //플레이어가 들어오면 카메라 고정해제 및 플레이어 따라가기
    //        gameObject.SetActive(false);    //게임오브젝트 비활성
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

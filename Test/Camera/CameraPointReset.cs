using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointReset : MonoBehaviour
{
    //�ش� ��ũ��Ʈ�� ���� ������Ʈ�� �÷��̾��� ���� ���θ��� �μ��������� ī�޶� ������Ų��

    /// <summary>
    /// �÷��̾ ����ٴϰ� �ϴ� ��ũ��Ʈ
    /// </summary>
    PlayerCameraMove playerCamera;
    private void Awake()
    {
        playerCamera =FindAnyObjectByType<PlayerCameraMove>();  //��ũ��Ʈ ����
    }
    
    public void ResetCameraPos()
    {
        
        playerCamera.CameraReset();     //�ش� ��ü�� ��Ȱ��ȭ �ɶ� ī�޶� �ٽ� �÷��̾ ����ٴ� �� �ְ� �Ѵ�
    }

}

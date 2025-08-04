using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointReset : MonoBehaviour
{
    //해당 스크립트를 가진 오브젝트는 플레이어의 길을 가로막고 부서질때까지 카메라를 고정시킨다

    /// <summary>
    /// 플레이어를 따라다니게 하는 스크립트
    /// </summary>
    PlayerCameraMove playerCamera;
    private void Awake()
    {
        playerCamera =FindAnyObjectByType<PlayerCameraMove>();  //스크립트 지정
    }
    
    public void ResetCameraPos()
    {
        
        playerCamera.CameraReset();     //해당 객체가 비활성화 될때 카메라가 다시 플레이어를 따라다닐 수 있게 한다
    }

}

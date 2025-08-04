using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation_UpperDisabler : MonoBehaviour
{
    [SerializeField]SpriteRenderer upper;
    [SerializeField] Transform upperTransform;
    PlayerCameraMove playerCameraMove;
    private void Awake()
    {
        playerCameraMove=FindAnyObjectByType<PlayerCameraMove>();
    }
    public void Enabler()
    {
        upper.enabled = true;   
    }
    public void Disabler()
    {
        upper.enabled=false;
    }
    public void Trun()
    {
        playerCameraMove.CameraStayHere();
        Vector3 vec = upperTransform.localScale;
        vec.x *= -1;
        upperTransform.localScale = vec;
    }
}

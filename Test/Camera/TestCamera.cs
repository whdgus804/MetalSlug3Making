using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    //플레이어가 뒤로가는 것을 방지


    [SerializeField] Transform cam;
    Vector2 vector2 = Vector2.zero;
    private void Update()
    {
        vector2 = cam.position;
    }
    private void FixedUpdate()
    {
        transform.position=vector2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    //�÷��̾ �ڷΰ��� ���� ����


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

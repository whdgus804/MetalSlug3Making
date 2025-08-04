using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_left : MonoBehaviour
{
    [SerializeField] Vector2 force;
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rigid.AddForce(force);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

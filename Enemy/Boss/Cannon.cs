using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject explosion;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;

    Rigidbody2D rigid;
    PlayerHealth player;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid= GetComponent<Rigidbody2D>();
        player = GameManager.Instance.PlayerHealth;
    }
    private void OnEnable()
    {
        rigid.AddForce(new Vector2(moveSpeed, 100));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            rigid.drag = 10;
            rigid.gravityScale = 4.5f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.HP = 1;
            Instantiate(explosion,transform.position,Quaternion.identity);  
            gameObject.SetActive(false);

        }else if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(explosion,transform.position,Quaternion.identity);  
            gameObject.SetActive(false);
        }
    }

}

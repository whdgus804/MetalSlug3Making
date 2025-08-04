using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasuit : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D playerRigid; 
    Animator anim;

    Player playerScrip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigid=player.GetComponent<Rigidbody2D>();
        playerScrip = player.GetComponent<Player>();
    }
    private void OnEnable()
    {
        playerRigid.gravityScale = 0.1f;
    }
    private void Start()
    {
        playerScrip.MoveInputEnble(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==9)
        {
            anim.SetTrigger("Break");
            playerRigid.gravityScale = 1;
            playerScrip.MoveInputEnble(true);
        }
        else if (collision.CompareTag("Ground"))
        {
            anim.SetTrigger("Break");
            playerRigid.gravityScale = 1;
            playerScrip.MoveInputEnble(true);
        }
    }
}

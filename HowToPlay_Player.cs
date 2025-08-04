using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay_Player : MonoBehaviour
{
    Animator upperAnim;
    Animator lowerAnim;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject grenade;
    float setMoveSpeed;
    private void Awake()
    {
        upperAnim = GetComponent<Animator>();
        lowerAnim=transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnEnable()
    {
        setMoveSpeed= moveSpeed;
        moveSpeed = 0.0f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnJump(false);
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.localScale.x*moveSpeed * Time.deltaTime, 0,0);
    }
    public void Move(bool onMove)
    {
        if(onMove)
        {
            moveSpeed = setMoveSpeed;
        }
        else
        {
            moveSpeed = 0.0f;
            lowerAnim.SetTrigger("MoveStop");
        }
        upperAnim.SetBool("Move",onMove);
        lowerAnim.SetBool("Move",onMove);
    }
    public void OnJump(bool onJump)
    {
        if (onJump)
        {
            Rigidbody2D rigi=GetComponent<Rigidbody2D>();
            Vector2 vec = new Vector2(0, jumpForce);
            rigi.AddForce(vec, ForceMode2D.Impulse);        
        }
        upperAnim.SetBool("Jump", onJump);
        lowerAnim.SetBool("Jump", onJump);
    }
    public void OnThrow()
    {
        upperAnim.SetTrigger("Throw");
        Instantiate(grenade, firePos.position, Quaternion.identity);
    }
    public void OnFire()
    {
        upperAnim.SetTrigger("Fire");
        Instantiate(bullet,firePos.position,Quaternion.identity);
    }
    public void Trun()
    {
        StartCoroutine(TrunCoroutin());
    }
    IEnumerator TrunCoroutin()
    {
        lowerAnim.SetTrigger("Trun");
        yield return new WaitForSeconds(0.1f);
        Vector2 vec = transform.localScale;
        vec.x *= -1;
        transform.localScale = vec;
    }
}

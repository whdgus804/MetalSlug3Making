using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGrenade : MonoBehaviour
{
    [SerializeField] float sideFoce;
    [SerializeField] float upFoce;
    Rigidbody2D rigid;
    [SerializeField] GameObject explosion;
    bool readyToBoam=false;
    private void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Vector2 vec = new Vector2(sideFoce*0.8f, upFoce*0.8f);
        rigid.AddForce(vec, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (readyToBoam)
            {
                Instantiate(explosion,transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
            else
            {
                readyToBoam=true;
                Vector2 vec=new Vector2(sideFoce,upFoce);
                rigid.AddForce(vec, ForceMode2D.Impulse);
            }
        }
    }
}

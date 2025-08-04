using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBackGroundMove : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] Transform spawnPos;



    Vector2 moveForce = new Vector2(-1, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trun"))
        {
            transform.position = spawnPos.position;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(moveForce * moveSpeed * Time.deltaTime);
    }

    public void GameOver()
    {
        moveSpeed= 0;
    }
}

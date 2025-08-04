using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIn : MonoBehaviour
{
    public Transform spanwPos;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            MovePlatform movePlatform = collision.GetComponent<MovePlatform>();
            movePlatform.MoveStop();
            PlayerHealth playerHealth=GameManager.Instance.PlayerHealth;
            playerHealth.spanwPosition= spanwPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDisabller : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.SetActive(false);
        }
    }
}

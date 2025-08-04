using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformUse : MonoBehaviour
{
    
    public void MovePlatform(float moveSpeed,Vector2 moveForce)
    {
        if(gameObject.activeSelf==true)
            StartCoroutine(Move(moveSpeed,moveForce));
        //Debug.Log(gameObject.name);

    }
    //public void StopMove()
    //{
    //    StopAllCoroutines();
    //}
    IEnumerator Move(float moveSpeed,Vector2 moveForce)
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(moveForce * moveSpeed * Time.deltaTime);
        }
    }

}

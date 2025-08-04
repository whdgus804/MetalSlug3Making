using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDisabler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;      
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Disabler()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void Twinkle()
    {
        StartCoroutine(TwinkleCoroutin());  
    }
    private IEnumerator TwinkleCoroutin()
    {
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Vector4.zero;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Vector4.one;
            yield return new WaitForSeconds(0.05f);
        }
        Disabler();
    }
    public void Destoyer()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    private IEnumerator TwinkleCoroutinDestorid()
    {
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Vector4.zero;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Vector4.one;
            yield return new WaitForSeconds(0.05f);
        }
        Destoyer();
    }
    public void Trun()
    {
        Vector3 vec = transform.localScale;
        vec.x *= -1;
        transform.localScale = vec;
    }
}

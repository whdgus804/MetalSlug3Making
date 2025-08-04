using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour
{
    public float moveSpeed;
    public float sideMoveSpeed;
    public Vector2 tartgetPos;
    [SerializeField] RectTransform center;
    float runMoveSpeed=0.0f;
    Image image;
    Vector2 vec = new Vector2(1, 0);
    private void Awake()
    {
        image= GetComponent<Image>();
    }

    private void Update()
    {
        image.rectTransform.anchoredPosition=Vector2.MoveTowards(image.rectTransform.anchoredPosition,tartgetPos,moveSpeed*Time.deltaTime);
        transform.Translate(vec * sideMoveSpeed*Time.deltaTime);
        transform.Translate(distance*runMoveSpeed*Time.deltaTime);
        
    }
    Vector2 distance;
    public void MoveFar()
    {
        sideMoveSpeed = 0.0f;
        moveSpeed = 0.0f;
        runMoveSpeed = 15;
        distance=center.anchoredPosition-image.rectTransform.anchoredPosition;
        distance *= -1;
    }

    public void DisableStart()
    {
        StartCoroutine(Disable());
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}

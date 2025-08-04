using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    /// <summary>
    /// 스크린의 이동속도
    /// </summary>
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject canvers;
    Image Image;
    Vector2 moveForce= new Vector2(1,0);

    private void Awake()
    {
        Image = GetComponent<Image>();
    }
    private void Start()
    {
        StartCoroutine(Disable());
        //Time.timeScale = 1.0f;
    }
    private void Update()
    {
        Image.rectTransform.Translate(moveForce * moveSpeed * Time.deltaTime);
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(3.0f);
        if(canvers != null)
            canvers.SetActive(true);
        Image.enabled= false;
    }
}

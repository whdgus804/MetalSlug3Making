using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrisonerUI : MonoBehaviour
{
    [SerializeField] Sprite[] prisonerSprit;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        StartCoroutine(AnimStart());
    }
    IEnumerator AnimStart()
    {
        for(int i = 0; i < prisonerSprit.Length; i++)
        {
            yield return new WaitForSeconds(0.07f);
            image.sprite= prisonerSprit[i];
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
    [SerializeField] GameObject[] gameOVERUI;

    private void OnEnable()
    {
        StartCoroutine(UIEnable());
    }
    IEnumerator UIEnable()
    {
        for(int i = 0; i < gameOVERUI.Length; i++)
        {
            gameOVERUI[i].SetActive(true);
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}

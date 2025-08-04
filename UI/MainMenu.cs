using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    MenuInput menuInput;
    /// <summary>
    /// 검은색 스크린
    /// </summary>
    [SerializeField] GameObject screen;
    private void Awake()
    {
        menuInput=new MenuInput();
    }
    private void OnEnable()
    {
        menuInput.Enable();
        menuInput.AnyKey.anykey.performed += Anykey;
    }
    private void OnDisable()
    {
        menuInput.AnyKey.anykey.performed -= Anykey;
        menuInput.Disable();
    }

    private void Anykey(InputAction.CallbackContext context)
    {
        screen.SetActive(true);
        StartCoroutine(SceneLoad());
    }
    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}

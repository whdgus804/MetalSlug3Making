using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pasue : MonoBehaviour
{
    PasueInput pasueInput;
    bool nowPasue = false;
    [SerializeField] GameObject pauseCanvers;
    private void Awake()
    {
        pasueInput=new PasueInput();
    }
    private void OnEnable()
    {
        pasueInput.Enable();
        pasueInput.Pasue.pasue.performed += OnPause;
    }
    private void OnDisable()
    {
        
        pasueInput.Pasue.pasue.performed -= OnPause;
        pasueInput.Disable();
    }

    private void OnPause(InputAction.CallbackContext _)
    {
        Pause();
    }
    public void Pause()
    {
        pauseCanvers.SetActive(!nowPasue);
        if (nowPasue)
        {
            nowPasue = false;
            Time.timeScale = 1.0f;

        }
        else
        {
            nowPasue = true;
            Time.timeScale = 0.0f;
        }
    }
    public void ReStart()
    {
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        Application.Quit();
    }
}

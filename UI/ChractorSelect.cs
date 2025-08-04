using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChractorSelect : MonoBehaviour
{

    /// <summary>
    /// 케릭터 프로필
    /// </summary>
    [SerializeField] GameObject[] profiles;

    [SerializeField] GameObject eriSelect;
    [SerializeField] GameObject door;
    [SerializeField] GameObject screen;
    SelectInput inputs;

    int selectCount = 0;

    int SelectCount
    {
        get =>selectCount;
        set
        {
            if (value < 0)
            {
                value = 3;
            }else if(value > 3)
            {
                value = 0;
            }
            profiles[selectCount].SetActive(false);
            selectCount = value;
            profiles[selectCount].SetActive(true);

        }
    }
    private void Awake()
    {
        inputs=new SelectInput();
    }
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Select.Select.performed += OnSelet;
        inputs.Choice.choiceRight.performed += OnRight;
        inputs.Choice.choiceLeft.performed += OnLeft;
    }



    private void Start()
    {
        SelectCount = selectCount;
    }
    private void OnDisable()
    {
        inputs.Choice.choiceLeft.performed -= OnLeft;
        inputs.Choice.choiceRight.performed -= OnRight;
        inputs.Select.Select.performed -= OnSelet;
        inputs.Disable();
    }
    private void OnSelet(InputAction.CallbackContext _)
    {
        eriSelect.SetActive(true);
        SelectCount = 1;
        inputs.Choice.choiceLeft.performed -= OnLeft;
        inputs.Choice.choiceRight.performed -= OnRight;
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        door.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        screen.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(4);
    }


    private void OnLeft(InputAction.CallbackContext _)
    {
        SelectCount--;
    }

    private void OnRight(InputAction.CallbackContext _)
    {
        SelectCount++;
    }

}

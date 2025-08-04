using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] GameObject screen;
    /// <summary>
    /// 마르코
    /// </summary>
    [SerializeField] GameObject player;
    [SerializeField] Image joyStick;
    [SerializeField] GameObject rightStick;
    [SerializeField] GameObject leftStick;

    [Space(20.0f)]
    [SerializeField] Image aButton;
    [SerializeField] Image bButton;
    [SerializeField] Image cButton;

    [SerializeField] Sprite[] aButtonClick;
    [SerializeField] Sprite[] bButtonClick;
    [SerializeField] Sprite[] cButtonClick;
    HowToPlay_Player playerScript;
    private void Awake()
    {
        playerScript=player.GetComponent<HowToPlay_Player>();
    }
    private void Start()
    {
        StartCoroutine(OnMoveTutorial());
    }
    IEnumerator OnMoveTutorial()
    {
        
        yield return new WaitForSeconds(2.0f);  //등장
        player.SetActive(true);
        playerScript.Move(true);
        yield return new WaitForSeconds(2.0f);          
        playerScript.Move(false);                   //정지
        yield return new WaitForSeconds(1.0f);
        playerScript.Trun();                        //회전
        LeftStick(true);
        yield return new WaitForSeconds(0.1f);
        LeftStick(false);
        yield return new WaitForSeconds(0.5f);
        LeftStick(true);
        playerScript.Move(true);                    //뒤로이동         
        yield return new WaitForSeconds(1.0f);
        LeftStick(false);
        playerScript.Move(false);                   //정지
        yield return new WaitForSeconds(0.5f);
        playerScript.Trun();                        //회전
        RightStick(true);
        yield return new WaitForSeconds(0.1f);
        RightStick(false);
        yield return new WaitForSeconds(0.5f);
        RightStick(true);
        playerScript.Move(true);                    //이동
        yield return new WaitForSeconds(1.0f);
        RightStick(false);
        playerScript.Move(false);                   //정지
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(OnButtonClick(cButton, true, cButtonClick));
        playerScript.OnJump(true);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(OnFireTutorial());
        //오른쪽으로 이동
        //기다리기
        //왼쪽으로 이동

        //다시 오른쪽 보기 

        //점프

    }
    IEnumerator OnFireTutorial()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(OnButtonClick(aButton, true, aButtonClick));
        playerScript.OnFire();
        yield return new WaitForSeconds(0.7f);
        playerScript.OnFire();
        yield return new WaitForSeconds(0.7f);
        playerScript.OnFire();
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(OnButtonClick(bButton, true, bButtonClick));
        playerScript.OnThrow();
        yield return new WaitForSeconds(1.5f);
        screen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
        //발사 3번
        //기다리기
        //수류탄
        //기다리기
        //씬전환
    }
    void LeftStick(bool enable)
    {
        leftStick.SetActive(enable);
        joyStick.enabled=!enable;
    }
    void RightStick(bool enable)
    {
        rightStick.SetActive(enable);
        joyStick.enabled = !enable;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buuton">0,1,2</param>
    /// <param name="On"></param>
    IEnumerator OnButtonClick(Image button,bool On, Sprite[] clicks)
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        if (On)
        {

            for(int i=0; i < 3; i++)
            {
                button.sprite=clicks[1];
                yield return wait;
                button.sprite = clicks[2];
                yield return wait;
            }
        }
        
        button.sprite = clicks[0];
    }
}

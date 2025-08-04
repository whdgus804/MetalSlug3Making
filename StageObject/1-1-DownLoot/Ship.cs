using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ship : MonoBehaviour
{
    /// <summary>
    /// ī�޶� ������ų ��ġ
    /// </summary>
    [SerializeField] Transform cameraPoint; 
    /// <summary>
    /// ī�޶� ��ũ��Ʈ
    /// </summary>
    PlayerCameraMove playerCamera;
    /// <summary>
    /// �÷����� �������� �����ϴ� ����
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// ���� �������� �����ߴ��� ��Ÿ���� ����
    /// </summary>
    bool onMove = false;
    /// <summary>
    /// ����� �� �ִϸ�����
    /// </summary>
    [SerializeField]Animator waterAnim;
    /// <summary>
    /// ó�� �ε��� ���� ��
    /// </summary>
    [SerializeField] Enemy_Ship enemyShip;

    /// <summary>
    /// �÷��̾ ������ ��Ȱ �� ���
    /// </summary>
    [SerializeField] Transform spawnPos;

    /// <summary>
    /// ���� �Ŵ���
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;
    [SerializeField] Vector2 wateranimPos;

    public BackGroundMove backGroundMove;

    
    private void Awake()
    {
        anim= GetComponent<Animator>();
        playerCamera=FindAnyObjectByType<PlayerCameraMove>();
        movePlatform=GetComponent<MovePlatform>();
        scoreManager = GameManager.Instance.ScoreManager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ship"))
        {
            backGroundMove.StopAllCoroutines();
            enemyShip.MoveStart();                                      //�� �� ������ ����
            //StartCoroutine(MoveBackStop());
            movePlatform.MoveStop();                                    //������ ����
            movePlatform.moveSpeed *= -1;                               //�̵���������
            movePlatform.PlatformStart(movePlatform.moveSpeed);         //�ڷ� �����̱�
            StartCoroutine(MoveBackStop());                             //�ڷ� �׸� �����̴� �ڷ�ƾ ����
            playerCamera.cM_Camera.Follow = null;                       //ī�޶�� ������ ����
        } else if (collision.CompareTag("Target"))
        {
            //�谡 �������� ���߰� �ν��� ��ġ
            Broken();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!onMove)
        {
            backGroundMove.moveSpeed = 0.7f;
            onMove = true;                  //���� ����
            StartCoroutine(Move());         //������ �ڷ�ƾ ����
            CameraSet();                    //ī�޶� ����
            PlayerHealth playerHealth=collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.spanwPosition = spawnPos;
        }
    }

    /// <summary>
    /// �������� �����ϴ� �Լ��� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {

        yield return new WaitForSeconds(1.0f);                              //��ٸ�
        backGroundMove.OnMoveAuto();
        movePlatform.PlatformStart(movePlatform.moveSpeed);                 //������
        waterAnim.SetBool("Move", true);                                    //�ִϸ��̼� ������
        
    }
    /// <summary>
    /// ī�޶� ����ġ�� ������Ű�� �Լ�
    /// </summary>
    void CameraSet()
    {
        playerCamera.CameraPositioSet(cameraPoint);
        scoreManager.CameraPos(cameraPoint);
    }
    /// <summary>
    /// �谡 ������ ���ư��� �Լ�
    /// </summary>
    public void MoveFront()
    {
        backGroundMove.OnMoveAuto();
        if(movePlatform.moveSpeed < 0)                                      //�ӵ��� �����̸�
        {
            movePlatform.moveSpeed*=-1;                                     //������� ����
            playerCamera.cM_Camera.Follow = cameraPoint;                    //ī�޶� ������� �ϱ�
        }
        movePlatform.PlatformStart(movePlatform.moveSpeed);                 //�����̱�
    }
    /// <summary>
    /// �ڷ� �����̴� ���� ���ߴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveBackStop()
    {
        yield return new WaitForSeconds(3.3f);  //��ٸ���
        movePlatform.MoveStop();                //����
        enemyShip.MoveStop();                   //�� �� ������ ����
        waterAnim.SetBool("Move",false);        //�ִϸ��̼� ������
    }

    void Broken()
    {
        backGroundMove.StopAllCoroutines();
        waterAnim.transform.parent= null;
        cameraPoint.parent= null;
        waterAnim.SetBool("Move", false);
        waterAnim.SetTrigger("Broken");
        waterAnim.transform.position = wateranimPos;
        anim.SetTrigger("Broken");
        StartCoroutine(BrokenAgain());
    }
    IEnumerator BrokenAgain()
    {
        yield return new WaitForSeconds(1.0f);
        anim.SetTrigger("Broken");
        waterAnim.SetTrigger("Broken");
        yield return new WaitForSeconds(2.0f);
        anim.SetTrigger("Broken");
        waterAnim.SetTrigger("Broken");
        

    }

}

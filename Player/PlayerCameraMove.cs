using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    /// <summary>
    /// �ó׸ӽ� ī�޶�
    /// </summary>
    [HideInInspector] public CinemachineVirtualCamera cM_Camera;

    BoxCollider2D[] col;
    PlayerHealth player;
    [SerializeField] LayerMask playerLayer;
    /// <summary>
    /// �޹�� ��ũ��Ʈ
    /// </summary>
    [SerializeField]BackGroundMove backGroundMove;
    /// <summary>
    /// ���� ī�޶� Ư�� ��ǥ�� �����Ǿ� �ִ��� ���´� ����
    /// </summary>
    bool nowCameraSet = false;

    /// <summary>
    /// �����Ŵ���
    /// </summary>
    ScoreManager scoreManager;

    private void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
        cM_Camera=FindAnyObjectByType<CinemachineVirtualCamera>();  //�ó׸ӽ� ī�޶� ����
        //col=GetComponent<BoxCollider2D>();
        col=GetComponentsInChildren<BoxCollider2D>();
        scoreManager = GameManager.Instance.ScoreManager;
    }
    
    /// <summary>
    /// �÷��̾ ���ʰ��� ī�޶� �ǵ��ư��� ���� ���� �ϴ� �Լ�
    /// </summary>
    public void CameraStayHere()
    {
        if (!nowCameraSet)
        {
            StopAllCoroutines();
            //cM_Camera.Follow = null;
            cM_Camera.Follow = scoreManager.cameraPos;
        }
    }
    /// <summary>
    /// ī�޶� ���ڸ����� ���ߵ� ��� �ݶ��̴��� ���� �Լ�
    /// </summary>
    public void CameraColOff()
    {
        for (int i = 0; i < col.Length; i++)                         //������ �ִ� ��� �ݶ��̴� ��Ȱ��ȭ
        {
            col[i].enabled = false;                                  //�ش� �ݷ����� ������� �������� ī�޶� ����(�÷��̾ �μ��� ������)

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player==null)
            player=GameManager.Instance.PlayerHealth;
        if (collision.CompareTag("Player") && player.HP>0)                     //Ʈ���ſ� �÷��̾ ������ 
        {
            //transform.position = collision.transform.position;  //�÷��̾� ��ġ�� �̵�
            //transform.parent=collision.transform;               //�÷��̾��� �ڽ����� ��(���󰡱� ����)
            //cM_Camera.Follow = col[0].transform;
            cM_Camera.Follow = transform;
            StartCoroutine(CamerMove());
        }else if (collision.CompareTag("CameraPoint"))          //ī�޶� ����Ʈ�� ������
        {

            //cM_Camera.Follow = collision.transform;             //�ó׸ӽ� ī�޶� ���󰡴� ��ü ����
            //transform.parent = null;                              //�߰� �̵� ������ ���� �θ� ����
            CameraPositioSet(collision.transform);
        }
    }
    public void CameraPositioSet(Transform trans)
    {
        nowCameraSet = true;
        CameraColOff();
        cM_Camera.Follow = trans;
    }
    
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        transform.position = collision.transform.position;
    //    }
    //}
    /// <summary>
    /// ī�޶� ����Ʈ�� ���� �� �ٽ� �÷��̾ ���󰡾��Ҷ� �����ϴ� �Լ�
    /// </summary>
    public void CameraReset()
    {
        nowCameraSet= false;
        StopAllCoroutines();
        if (scoreManager.cameraPos == null)
        {
            cM_Camera.Follow = transform;
            for(int i = 0;i < col.Length; i++)
            {
                col[i].enabled = true;
            }

        }
        else
        {

            cM_Camera.Follow = scoreManager.cameraPos;
        }
        
        StartCoroutine(CamerMove());
        //if (player != null)
        //{
        //    for(int i = 0;i < col.Length;i++)
        //    {

        //        col[i].enabled = true;     //Ʈ���� �ٽ� Ȱ��ȭ 
        //    }
        //    transform.position = player.transform.position;
        //    transform.parent = player.transform;               //�÷��̾��� �ڽ����� ��(���󰡱� ����)
        //    cM_Camera.Follow = col[0].transform;
        //}
    }
    
    IEnumerator CamerMove()
    {
        if (backGroundMove != null)
        {
            while (true)
            {
                backGroundMove.OnMove();
                cameraPosition.x = player.transform.position.x;
                transform.position = cameraPosition;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (true)
            {
                cameraPosition.x = player.transform.position.x;
                transform.position = cameraPosition;
                yield return new WaitForFixedUpdate();
            }
        }
    }

   public Vector2 cameraPosition=new Vector2(0,1.07f);
}

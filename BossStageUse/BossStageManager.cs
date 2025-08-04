using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    public bool gameover = false;
    /// <summary>
    /// �÷��̾ �������� �̵��� �̵��ӵ�
    /// </summary>
    [SerializeField]float moveSpeed;
    /// <summary>
    /// �÷��̾� Ʈ������
    /// </summary>
    [SerializeField]Transform player;
    /// <summary>
    /// ������ ����
    /// </summary>
    Vector2 moveVector = Vector2.left;
    /// <summary>
    /// ī�޶� ���� ��ġ
    /// </summary>
    [SerializeField] Transform cameraPos;
    /// <summary>
    ///�÷��̾ ��Ȱ�� ��ġ
    /// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// �÷��̾� �¸����
    /// </summary>
    [SerializeField] GameObject clearMotion;
    /// <summary>
    /// �޹�� ������Ʈ
    /// </summary>
    [SerializeField]BossBackGroundMove[] bossBackGroundMove;
    /// <summary>
    /// �÷��̾� ī�޶� ��ũ��Ʈ
    /// </summary>
    PlayerCameraMove playerCameraMove;
    /// <summary>
    /// �ٸ��� �����ϴ� ��ũ��Ʈ
    /// </summary>
    BridgeSpanwer bridgeSpanwer;
    /// <summary>
    /// ���� ���� ��ũ��Ʈ
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// �÷��̾�
    /// </summary>
    PlayerHealth playerHealth;
    AmmoCounter ammocounter;
    [SerializeField] Weapon_Attack weapon_Attack;
    [SerializeField] Machain_Gun machain_Gunl;
    

    [SerializeField] Timer timer;

    [SerializeField] GameObject finalScoreCanvers;
    private void Awake()
    {
        ammocounter=GameManager.Instance.AmmoCounter;
        playerHealth = GameManager.Instance.PlayerHealth;
        playerCameraMove=FindAnyObjectByType<PlayerCameraMove>();
        bridgeSpanwer = FindAnyObjectByType<BridgeSpanwer>();
        scoreManager=GameManager.Instance.ScoreManager;
    }
    private void OnEnable()
    {

        if (weapon_Attack.weaponType == Weapon_Attack.WeaponType.Machain_Gun)       //���Ⱑ ������̸� �Ѿ� ��¹ޱ�
        {
            machain_Gunl.ammo = ammocounter.saveAmmo;
        }
        //player = playerHealth.transform;
        weapon_Attack.haveGrenade=ammocounter.saveGrenade;      //��ź �� ��¹ޱ�
        playerCameraMove.CameraPositioSet(cameraPos);       //�÷��̾� ī�޶� ����
        scoreManager.CameraPos(cameraPos);                  //ī�޶� ����
        playerHealth.spanwPosition = spawnPos;              //���� ��ġ ����
        StartCoroutine(WaitStageStart());                   //��� ��ٸ� �� �������� ��� �����ϴ� �ڷ�ƾ �Լ� ����
        
    }
    /// <summary>
    /// ��� ��ٸ� �� �������� ��� �����ϴ� �ڷ�ƾ �Լ� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitStageStart()
    {
        float set = moveSpeed;                  //�̵��ӵ� ����
        moveSpeed = 0.0f;                       //�̵��ӵ� ����
        yield return new WaitForSeconds(3.0f);      //3�� ��ٸ���
        moveSpeed = set;                        //����� �̵��ӵ� ����
        bridgeSpanwer.SpawnStart();             //�ٸ� ���� ����
        
    }

    private void FixedUpdate()
    {
        player.transform.Translate(moveVector * moveSpeed* Time.deltaTime); //�÷��̾� �̵�
    }
    /// <summary>
    /// ���� Ŭ���� �Լ�
    /// </summary>
    public void GameOver()
    {
        finalScoreCanvers.SetActive(true);
        timer.StopAllCoroutines();
        moveSpeed = 0.0f;       //�̵� ����
        bridgeSpanwer.SpawnOver();      //�ٸ� ���� ����
        gameover = true;                //���� Ȱ��
        Vector2 vec=player.transform.position;          //��ġ �ޱ�
        vec.y -= 0.2f;                              //���� ����
        Instantiate(clearMotion,vec,Quaternion.identity);       //Ŭ���� ��� ����
        player.gameObject.SetActive(false);                 //�÷��̾� ��Ȱ��ȭ 
        for(int i = 0;i<bossBackGroundMove.Length;i++)      //�� ��� �̵� ����
        {
            bossBackGroundMove[i].GameOver();

        }
        //missionComplete.SetActive(true);
    }
}

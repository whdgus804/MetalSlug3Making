using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    public bool gameover = false;
    /// <summary>
    /// 플레이어가 왼쪽으로 이동할 이동속도
    /// </summary>
    [SerializeField]float moveSpeed;
    /// <summary>
    /// 플레이어 트랜스폼
    /// </summary>
    [SerializeField]Transform player;
    /// <summary>
    /// 움직임 벡터
    /// </summary>
    Vector2 moveVector = Vector2.left;
    /// <summary>
    /// 카메라 고정 위치
    /// </summary>
    [SerializeField] Transform cameraPos;
    /// <summary>
    ///플레이어가 부활할 위치
    /// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// 플레이어 승리모션
    /// </summary>
    [SerializeField] GameObject clearMotion;
    /// <summary>
    /// 뒷배경 오브젝트
    /// </summary>
    [SerializeField]BossBackGroundMove[] bossBackGroundMove;
    /// <summary>
    /// 플레이어 카메라 스크립트
    /// </summary>
    PlayerCameraMove playerCameraMove;
    /// <summary>
    /// 다리를 생성하는 스크립트
    /// </summary>
    BridgeSpanwer bridgeSpanwer;
    /// <summary>
    /// 점수 관리 스크립트
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// 플레이어
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

        if (weapon_Attack.weaponType == Weapon_Attack.WeaponType.Machain_Gun)       //무기가 기관총이면 총알 계승받기
        {
            machain_Gunl.ammo = ammocounter.saveAmmo;
        }
        //player = playerHealth.transform;
        weapon_Attack.haveGrenade=ammocounter.saveGrenade;      //폭탄 수 계승받기
        playerCameraMove.CameraPositioSet(cameraPos);       //플레이어 카메라 고정
        scoreManager.CameraPos(cameraPos);                  //카메라 고정
        playerHealth.spanwPosition = spawnPos;              //스폰 위치 고정
        StartCoroutine(WaitStageStart());                   //잠시 기다린 후 스테이지 재상 시작하는 코루틴 함수 실행
        
    }
    /// <summary>
    /// 잠시 기다린 후 스테이지 재상 시작하는 코루틴 함수 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitStageStart()
    {
        float set = moveSpeed;                  //이동속도 저장
        moveSpeed = 0.0f;                       //이동속도 정지
        yield return new WaitForSeconds(3.0f);      //3초 기다리기
        moveSpeed = set;                        //저장된 이동속도 변경
        bridgeSpanwer.SpawnStart();             //다리 생성 시작
        
    }

    private void FixedUpdate()
    {
        player.transform.Translate(moveVector * moveSpeed* Time.deltaTime); //플레이어 이동
    }
    /// <summary>
    /// 게임 클리어 함수
    /// </summary>
    public void GameOver()
    {
        finalScoreCanvers.SetActive(true);
        timer.StopAllCoroutines();
        moveSpeed = 0.0f;       //이동 정지
        bridgeSpanwer.SpawnOver();      //다리 생산 정지
        gameover = true;                //변수 활성
        Vector2 vec=player.transform.position;          //위치 받기
        vec.y -= 0.2f;                              //변수 수정
        Instantiate(clearMotion,vec,Quaternion.identity);       //클리어 모션 생성
        player.gameObject.SetActive(false);                 //플레이어 비활성화 
        for(int i = 0;i<bossBackGroundMove.Length;i++)      //뒷 배경 이동 정지
        {
            bossBackGroundMove[i].GameOver();

        }
        //missionComplete.SetActive(true);
    }
}

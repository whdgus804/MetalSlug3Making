using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Attack : MonoBehaviour
{
    //플레이어의 근접공격 및 무기 발사 스크립트

    /// <summary>
    /// 공격가능한 레이어
    /// </summary>
    [SerializeField] LayerMask canHitLayer;
    /// <summary>
    /// 플레이어의 근접공격 위치
    /// </summary>
    [SerializeField] Transform attackPos;
    /// <summary>
    /// 플레이어의 근접공격 범위
    /// </summary>
    [SerializeField] Vector2 attackRange;

    /// <summary>
    /// 무기스크립트 및 오브젝트가 저장되어있는 오브젝트
    /// </summary>
    [SerializeField] GameObject weapon;

    /// <summary>
    /// 전방 발사위치
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform frontFirePos;
    /// <summary>
    /// 위 발사위치
    /// </summary>
    [SerializeField] Transform upFirePos;
    /// <summary>
    /// 앉아 발사 위치
    /// </summary>
    [SerializeField] Transform sitFirePos;
    /// <summary>
    /// 밑 발사위치
    /// </summary>
    [SerializeField] Transform downFirePos;


    /// <summary>
    /// 수류탄을 던질 위치
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform throwPosition;

    /// <summary>
    /// 일정 수 이상 화면에 보이면 안되는 총알 및 수류탄을 사용할때 사용할 수 있는 상태인지 나타내는 스크립트 변수
    /// </summary>
    AmmoCounter ammoCounter;

    /// <summary>
    /// 권총스크립트
    /// </summary>
    Pistol pistol;

    /// <summary>
    /// 기관총 스크립트
    /// </summary>
    Machain_Gun machain_Gun;
    ///// <summary>
    ///// 플레이어가 위를 바라보는지 나타내는 bool변수 true면 위를 보는중
    ///// </summary>
    //bool isLookUp;
    /// <summary>
    /// 플레이어의 무기 타입
    /// </summary>
    public enum WeaponType
    {
        Pistol,                  //기본 권총
        Machain_Gun              //기관총
    }
    public WeaponType weaponType;     //플레이어의 무기타입

    /// <summary>
    /// 플레이어 인풋
    /// </summary>
    BasicPlayerInput input;

    /// <summary>
    /// 총알의 회전값에 적용할 벡터값
    /// </summary>
    Vector3 aimVector = Vector3.zero;

    PlayerAnimation anim;

    /// <summary>
    /// 근접공격이 가능한지 나타내는 변수
    /// </summary>
    bool readToAtack=true;

    public int haveGrenade = 10;
    private void Awake()
    {
        input = new BasicPlayerInput();                         //인풋 지정
        pistol= weapon.GetComponentInChildren<Pistol>();        //권총 스크립트 지정
        machain_Gun= weapon.GetComponentInChildren<Machain_Gun>();        //권총 스크립트 지정
        ammoCounter=GameManager.Instance.AmmoCounter;
        anim=GetComponent<PlayerAnimation>();
    }

    /// <summary>
    /// 플레이어의 공격함수
    /// </summary>
    /// <param name="sign">0-정면 1-위 2-앉기 3-밑</param>
    public void OnAttack(int sign, Vector3 localScale)
    {
        bool sit=false;
        if(sign==2)
            sit=true;
           
        bool inEnemy = EnemySencer(sit);                            //총을 발사하기전 근접공격 사거리에 적 , 묶여있는 NPC 또는 부술 수 있는 물체가 있는지 확인
        if (!inEnemy)                                            //적이 없으면 총발사
        {
            anim.OnFire(sit);
            Transform aimPos = FirePosition(sign);
            switch (weaponType)                                  //현재 무기에 따라 발사함수 실행
            {
                case WeaponType.Pistol:                          //무기가 권총이면
                    pistol.OnFire(aimPos,aimVector,localScale,sign);             //권총발사 함수 실행
                    break;
                case WeaponType.Machain_Gun:
                    machain_Gun.OnFire(aimPos,aimVector,localScale, sign);
                    //machain_Gun.FireAnim(anim);
                    break;
            }
        }

    }

    /// <summary>
    /// 적이 근접공격 범위안에 있는지 나타내는 bool리턴 함수
    /// </summary>
    /// <returns></returns>
    private bool EnemySencer(bool sit)
    {
        bool inEnemy = false;                                                                   //반환값 생성
        Collider2D[] collider;                                                                  //적을 저장할 배열
        collider = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0, canHitLayer);     //근접공격 위치에 근접공격 범위만큼 0의 각도로 공격가능한 레이어를 가진 모든 콜라이더를 배열에 저장
        if (collider.Length == 0)                                                               //적이 없으면 false반환
        {
            inEnemy = false;
        }
        else
        {                                                                                       //적이 있으면
            if (readToAtack)
            {
                EnemyHP enemyHP;                                                                    //스크립트 생성
                for (int i = 0; i < collider.Length; i++)                                           //콜라이더 배열만큼 반복
                {
                    if (!collider[i].CompareTag("NPC"))                                             //NPC가 아니라 적 혹은 부술 수 있는 물체면
                    {
                        enemyHP = collider[i].GetComponent<EnemyHP>();                              //스크립트 저장 
                        enemyHP.HP = 1;                                                             //데미지 1 주기
                    }
                    else                                                                            //NPC면
                    {
                        //NPC포박 풀기
                        NPC_Base npc = collider[i].GetComponent<NPC_Base>();
                        npc.Hit();
                    }
                }
                StartCoroutine(AttackDelay());
                anim.OnAttack(sit);
            }
            readToAtack = false;
            inEnemy= true;                                                                      //반환값 수정
            

        }
        return inEnemy;                                                                         //값 반환
    }
    /// <summary>
    /// 근접공격 딜레이를 계산하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        readToAtack = true;
    }
    /// <summary>
    /// 현재 플레이어의 상태에 따라 총알의 생성위치를 지정해주는 함수
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    private Transform FirePosition(int sign)
    {
        Transform aimpos=null;                      //반환값 만들기
        if (sign <= 1)                              //sign이 0또는1이면
        {
            if (sign == 0)                          //앞이면
            { 
                aimpos = frontFirePos;              //발사위치는 전방
                aimVector = Vector3.zero;           //총알에 줄 회전값은 0
            }
            else                                    //위이면
            {
                aimpos = upFirePos;                 //발사위치는 위쪽
                aimVector.z = 90.0f;                //회전값은 90
            }
        }
        else                                        //sign이 2또는 3이면
        {
            if(sign == 2)                           //앉은 상태이면
            {
                aimpos = sitFirePos;                //앉은 상태의 발사 위치
                aimVector = Vector3.zero;           //회전값은 0
            }
            else                                    //공중에서 밑을 바라보는 상태이면
            {
                aimpos = downFirePos;               //밑의 발사위치
                aimVector.z = -90.0f;               //회전값은 -90 (270)
            }
        }
        return aimpos;
    }
    /// <summary>
    /// 슈류탄 발사함수
    /// </summary>
    public void OnGrenade()
    {
        if(ammoCounter.grenadeCount > 0 && haveGrenade >0)
        {
            PoolFactory.Instance.GetBoamStick(throwPosition.position, transform.localScale);
            ammoCounter.grenadeCount--;
            haveGrenade--;
            ammoCounter.OnGrenade(haveGrenade);
        }
    }
    /// <summary>
    /// 플레이어가 행동으로 조준점을 변경할때 실행되는 함수
    /// </summary>
    /// <param name="sign"></param>
    public void AimMoved(int sign)
    {
        //switch (weaponType)
        //{
        //    case (WeaponType.Pistol):
        //        break;
        //    case(WeaponType.Machain_Gun):
        //        break;
        //}
        if (weaponType == WeaponType.Machain_Gun)
        {
            machain_Gun.CrossFire(sign,transform.localScale);
        }
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        anim.ChangeWeapon(weaponType);
    }
    public void OnRespawn()
    {
        haveGrenade = 10;
        weaponType=WeaponType.Pistol;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //공격 지점과 공격 범위를 참조하여 기즈모 추가


        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(attackPos.position, attackRang);
        Gizmos.color = Color.yellow;

        
        Vector2 leftUp=new Vector2(attackPos.position.x - attackRange.x * 0.5f, attackPos.position.y + attackRange.y * 0.5f);      //값지정
        Vector2 rightUp=new Vector2( leftUp.x+attackRange.x,leftUp.y);
        Vector2 leftDown = new Vector2(leftUp.x,leftUp.y-attackRange.y);
        Vector2 rightDown = new Vector2(rightUp.x, rightUp.y - attackRange.y);

        Gizmos.DrawLine(leftUp, rightUp);                                                                                           //기즈모 그리기
        Gizmos.DrawLine(leftUp,leftDown);
        Gizmos.DrawLine (rightUp,rightDown);
        Gizmos.DrawLine(leftDown, rightDown);
        //Gizmos.DrawIcon(new Vector2(leftUpX, leftUpY),"Left");
    }

#endif
}

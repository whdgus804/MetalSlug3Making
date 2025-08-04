using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    /// 상체 스프라이트
    /// </summary>
    SpriteRenderer uppersprite;
    /// <summary>
    /// 하체 스프라이트
    /// </summary>
    SpriteRenderer lowerSprite;
    /// <summary>
    /// 상체 애니메이터
    /// </summary>
    Animator upperAnim;
    /// <summary>
    /// 하체 애니메이터
    /// </summary>
    Animator lowerAnim;
    /// <summary>
    /// 마지막에 도끼 공격을 하였는지 나타내는 변수 true면 이번 공격이 도끼 공격임 
    /// </summary>
    bool axeAttack = true;
    /// <summary>
    /// 몸을 회전하는 애니메이션
    /// </summary>
    int trun_To_Hash = Animator.StringToHash("Trun");
    /// <summary>
    /// 이동하는 애니메이션
    /// </summary>
    int move_To_Hash = Animator.StringToHash("Move");
    /// <summary>
    /// 떨어지는 애니메이션
    /// </summary>
    int fall_To_Hash = Animator.StringToHash("Fall");
    /// <summary>
    /// 발사하는 애니메이션
    /// </summary>
    int fire_To_Hash = Animator.StringToHash("Fire");
    /// <summary>
    /// 위를 보는 애니메이션
    /// </summary>
    int lookUp_To_Hash = Animator.StringToHash("LookUp");
    /// <summary>
    /// 앉거나 공중에서 하단을 보는 애니메이션
    /// </summary>
    int down_To_Hash = Animator.StringToHash("LookDown_Sit");
    /// <summary>
    /// 위로 뛰는 애니메이션
    /// </summary>
    int jump_To_Hash = Animator.StringToHash("Jump");
    /// <summary>
    /// 근접공격 애니메이션
    /// </summary>
    int attack_To_Hash = Animator.StringToHash("Attack");
    /// <summary>
    /// 도끼로 공격하는 애니메이션
    /// </summary>
    int Axe_To_Hash = Animator.StringToHash("Axe");
    /// <summary>
    /// 기관총이 발사중인지 나타내는 애니메이션
    /// </summary>
    int firing_To_Hash = Animator.StringToHash("Firing");
    /// <summary>
    /// 사망 애니메이션
    /// </summary>
    int die_To_Hash = Animator.StringToHash("Die");
    /// <summary>
    /// 부활 애니메이션
    /// </summary>
    int respawn_To_Hash = Animator.StringToHash("Respawn");


    /// <summary>
    /// 권청 상체 AC
    /// </summary>
    [Space(20)]
    [SerializeField] RuntimeAnimatorController pistolUpperAC;
    /// <summary>
    /// 권총 하체 AC
    /// </summary>
    [SerializeField] RuntimeAnimatorController pistolLowerAC;
    /// <summary>
    /// 기관총 상체 AC
    /// </summary>
    [Space(20)]
    [SerializeField] AnimatorOverrideController hmUpperAC;
    /// <summary>
    /// 기관총 하체 AC
    /// </summary>
    [SerializeField] AnimatorOverrideController hmLowerAC;


    /// <summary>
    /// 무기교체시 떨어뜨릴 오브젝트
    /// </summary>
    [SerializeField] GameObject dropGun;
    /// <summary>
    /// 공격 관련 스크립트
    /// </summary>
    Weapon_Attack weapon_Attack;
    private void Awake()
    {
        uppersprite = GetComponent<SpriteRenderer>();           
        upperAnim = GetComponent<Animator>();
        Transform tran = transform.GetChild(0);
        lowerSprite = tran.GetComponent<SpriteRenderer>();
        lowerAnim = tran.GetComponent<Animator>();
        weapon_Attack=GetComponent<Weapon_Attack>();
    }
    private void OnEnable()
    {
        hmLowerAC.runtimeAnimatorController = lowerAnim.runtimeAnimatorController;
        hmUpperAC.runtimeAnimatorController = upperAnim.runtimeAnimatorController;
    }
    /// <summary>
    /// 플레이어 이동 애니메이션
    /// </summary>
    /// <param name="moveValue"></param>
    public void OnMove(Vector2 moveValue)
    {
        upperAnim.SetFloat(move_To_Hash, moveValue.x);      //움직임에 변수 전달
        lowerAnim.SetFloat(move_To_Hash, moveValue.x);      //움직임에 변수 전달
    }
    /// <summary>
    /// 뒤를 돌아보는 애니메이션
    /// </summary>
    public void OnTrun()
    {
        lowerAnim.SetTrigger(trun_To_Hash);     //하체 애니메이션에 값전달
    }
    /// <summary>
    /// 떨어지는 변수
    /// </summary>
    /// <param name="fall"></param>
    public void OnFall(bool fall)
    {
        upperAnim.SetBool(fall_To_Hash, fall);      //떨어지는 변수 전달
        lowerAnim.SetBool(fall_To_Hash, fall);      //떨어지는 변수 전달

    }
    /// <summary>
    /// 무기 발사 함수
    /// </summary>
    /// <param name="sit">현재 앉아 있는지 나타내는 변수</param>
    public void OnFire(bool sit)
    {
        if (!sit)
        {
            upperAnim.SetTrigger(fire_To_Hash);  //서있으면 상체 발사      

        }
        else
        {
            lowerAnim.SetTrigger(fire_To_Hash);     //앉아있으면 하체 발사 

        }
    }
    /// <summary>
    /// 플레이어 현재 상테를 나타내는 함수
    /// </summary>
    /// <param name="sign"></param>
    public void ChangeLookPoint(int sign)
    {
        /// 0-정면
        /// 1-위
        /// 2-앉기
        /// 3-밑
        bool up = false;        //변수 생성
        bool down = false;      //변수 생성 
        switch (sign)
        {
            case 1:
                up = true;
                break;
            case 2:
                down = true;
                break;
            case 3:
                down = true;
                break;
        }
        upperAnim.SetBool(lookUp_To_Hash, up);
        upperAnim.SetBool(down_To_Hash, down);

        lowerAnim.SetBool(lookUp_To_Hash, up);
        lowerAnim.SetBool(down_To_Hash, down);
    }
    /// <summary>
    /// 위로 뛰어오르는 함수
    /// </summary>
    public void OnJump()
    {
        lowerAnim.SetTrigger(jump_To_Hash);     //점프애니메이션 값전달
    }
    /// <summary>
    /// 근법공격 함수
    /// </summary>
    /// <param name="sit">현재 앉아 있는지 나타내는 변수</param>
    public void OnAttack(bool sit)
    {
        if (!sit)
        {
            upperAnim.SetBool(Axe_To_Hash, axeAttack);      //공격 값전달
            upperAnim.SetTrigger(attack_To_Hash);           //하체 값전달

        }
        else
        {
            lowerAnim.SetTrigger(attack_To_Hash);   //하체 값전달
        }
        axeAttack = !axeAttack;     //변수 변경
        
    }
    public void OnFiring(bool nowfire)
    {
        upperAnim.SetBool(firing_To_Hash, nowfire); 
        
    }
    public void OnDie()
    {
        lowerAnim.SetTrigger(die_To_Hash);
    }
    public void OnDie_Lucost()
    {
        uppersprite.enabled = false;
        lowerAnim.SetTrigger("Lucost");
    }
    public void OnReapwn()
    {
        lowerAnim.SetTrigger(respawn_To_Hash);
    }
    /// <summary>
    /// 죽었을 때 애니메이션 변수를 초기화하는 함수
    /// </summary>
    public void OnDeadResetAnim()
    {
        upperAnim.SetBool(lookUp_To_Hash, false);       //상체 애니메이션 값 초기화
        lowerAnim.SetBool(lookUp_To_Hash, false);       
        upperAnim.SetBool(down_To_Hash, false);         //하체 애니메이션 값 초기화
        lowerAnim.SetBool(down_To_Hash, false);
    }
    /// <summary>
    /// 물에 빠져 죽는 애니메이션 
    /// </summary>
    public void Dead_Water()
    {
        lowerAnim.SetTrigger("Water");              //애니메이션 값전달
        
    }
    /// <summary>
    /// 무기교체 함수
    /// </summary>
    /// <param name="weaponType">교체할 무기 </param>
    public void ChangeWeapon(Weapon_Attack.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case Weapon_Attack.WeaponType.Pistol:       //권총이면
                upperAnim.SetTrigger("ChangeWeapon");       //교체 애니메이션 재생
                upperAnim.runtimeAnimatorController = pistolUpperAC;    //애니메이터 변경
                lowerAnim.runtimeAnimatorController= pistolLowerAC;     //하체 애니메이터 변경
                dropGun.transform.position = transform.position;        //무기위치 변경
                dropGun.SetActive(true);                                //무기 활성화
                break;
            case Weapon_Attack.WeaponType.Machain_Gun:                  //기관총이면

                if (weapon_Attack.weaponType != Weapon_Attack.WeaponType.Machain_Gun)
                {
                         
                    upperAnim.runtimeAnimatorController = hmUpperAC;        //애니메이터 컨트롤러 변경
                    
                    lowerAnim.runtimeAnimatorController = hmLowerAC;        //애니메이터 컨트롤러
                }
                else
                {

                }
                break;
        }
    }

}

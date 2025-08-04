using System.Collections;
using UnityEngine;

public class Machain_Gun : WeaponBase
{
    /// <summary>
    /// 마지막 발사 지점을 저장할 변수0앞1위2앉기3밑
    /// </summary>
    int beforSign = 0;                                  
    /// <summary>
    /// 마지막 발사한 총알의 각도를 저장할 변수
    /// </summary>
    Vector3 angle = Vector3.zero;
    /// <summary>
    /// 사선발사중임을 나타내는 변수 true면 발사중 
    /// </summary>
    bool crossFiring=false;

    /// <summary>
    /// 위 사선 발사 위치
    /// </summary>
    [SerializeField]Transform[] upCrossFirePos;
    /// <summary>
    /// 아래 사선 발사위치
    /// </summary>
    [SerializeField]Transform[] downCrossFirePos;

    PlayerAnimation anim;

    AmmoUI ammoUI;
    private void Awake()
    {
        anim=transform.parent.GetComponentInParent<PlayerAnimation>();
        ammoUI=FindFirstObjectByType<AmmoUI>();
    }



    public override void OnFire(Transform aimPositon, Vector3 aimVector, Vector3 localScale, int sign)
    {
        if (readyToFire)            //발사가능이면
        {
            angle = aimVector;      //각도저장
            beforSign = sign;       //마지막 발사위치 저장
            readyToFire = false;    //발사중 이므로 변수 변경
            anim.OnFiring(true);
            StartCoroutine(ShootCoroutin(aimPositon,aimVector,localScale)); //총알을 3발 발사하는 코루틴 시작
        }
    }
    //public void FireAnim(PlayerAnimation anim)
    //{
    //   StartCoroutine(NowFiring(anim));
    //}
    //IEnumerator NowFiring(PlayerAnimation anim)
    //{
    //    anim.OnFiring(true);
    //    yield return new WaitUntil(() => readyToFire);
    //    anim.OnFiring(false);
    //}
    /// <summary>
    /// 총알을 3발 발사하는 코루틴함수
    /// </summary>
    /// <param name="aimPositon">발사 위치</param>
    /// <param name="aimVector">발사각도</param>
    /// <param name="localScale">플레이어의 방향</param>
    /// <returns></returns>
    IEnumerator ShootCoroutin(Transform aimPositon, Vector3 aimVector, Vector3 localScale)
    {
        for (int i = 0; i < 3; i++)         //3번 반복
        {
            int randint = Random.Range(-3, 4);                                  //총알이 원뿔 모형으로 흩뿌리게끔 랜덤값 저장
            aimVector.z += randint;                                             //랜덤값을 총알각도에 더하기
            //Debug.Log(localScale);
            if(localScale.x < 0)        //플레이어가 왼쪽방향이면
            {
                //Debug.Log(aimVector);
                if (aimVector.z < -45 || aimVector.z > 45)  //총알이 뒤집히는것을 방지 뒤를 돈 상태로 수직 발사할때 총알 각도 수정
                {
                    localScale = Vector3.one;               //로컬 스케일값 수정
                }
            }
            PoolFactory.Instance.GetMachianGunBullet(aimPositon.position, aimVector, localScale);       //풀팩토리에서 총알 생성
            AmmoDiscount();
            yield return new WaitForSeconds(fireDelay);                                                 //발사 딜레이기다리기
        }
        readyToFire=true;                                                                           //총알이 발사되면 다시 발사 가능하도록 변수 변경
        yield return new WaitForSeconds(0.2f);
        anim.OnFiring(false);                                                                 //애니메이션 재생
        //총알 생성
    }
    /// <summary>
    /// 사선발사할때 총알을 생성하는 함수
    /// </summary>
    /// <param name="sign">발사 위치</param>
    /// <param name="localSclae">플레이어 방향</param>
    public void CrossFire(int sign,Vector3 localSclae)
    {
        if (!readyToFire && beforSign!=2 && !crossFiring&& sign!=2)         //발사중이고 현재발사 위치와 마지막 발사 위치가 앉는 자세가 아니며 사선 발사중이 아니면
        {
            
            float anglefloat = 18.0f;                           //변경할 총알의 각도 값
            crossFiring = true;                                 //중복 실행방지를 위해 사선 발사중 변수 변경
            Transform[] trans=new Transform[3];                 //사선 발사위치를 저장할 배열 생성
            StopAllCoroutines();                                //수직 수평 발사중지
            switch(sign)                                        //발사 위치에따른 값 저장
            {
                case 0:                             //앞일때
                    if (beforSign == 1)
                    {
                        //위에서 앞 방향 사선 방향일때
                        if (localSclae.x < 0)                                                //플레이어 방향이 왼쪽이면
                        {       
                            localSclae=Vector3.one;                                          //총알 방향 변경 (뒤로 날아가는 것을 방지)
                        }
                        else
                        {
                            anglefloat*= -1;                                                     //각도 변경 

                        }
                        for(int i = 0; i < upCrossFirePos.Length; i++)                       //발사위치에 위 발사위치를 역순으로 저장
                        {
                            trans[i]= upCrossFirePos[2-i];
                        }
                        StartCoroutine(CrossFireCoroutin(trans, localSclae, anglefloat));   //사선으로 총알을 발사하는 코루틴 함수 실행
                    }
                    else
                    {
                        //밑에서 앞 방향 사선일때 
                        if (localSclae.x < 0)                                            //플레이어 방향이 왼쪽이면
                        {
                            anglefloat *= -1;                                            //더할 각도 값 변경
                            angle.z *= -1;                                               //방향 변경(총알이 다른 방향으로 날아가는 것을 방지)
                        }
                        for (int i = 0; i < downCrossFirePos.Length; i++)                //발사위치에 아래 발사위치를 역순으로 저장
                        {
                            trans[i] = downCrossFirePos[2 - i]; 
                        }
                        StartCoroutine(CrossFireCoroutin(trans, localSclae, anglefloat)); //대각선 발사 코루틴함수 실행
                        //밑 앞
                    }
                    break;
                case 1:
                    //앞에서 위를 향에 발사할때
                    if (localSclae.x < 0)                                                  //플레이어 방향이 왼방향이면
                    {
                        anglefloat *= -1;                                                  //더할 각도값 변경
                    }
                    for (int i = 0; i < upCrossFirePos.Length; i++)                        //발사위치를 위발사 위치로 저장
                    {
                        trans[i] = upCrossFirePos[i];
                    }
                    StartCoroutine(CrossFireCoroutin(trans,localSclae, anglefloat));       //사선발사 코루틴 함수 실행
                    //앞 위
                    break;
                case 3:
                    //앞에서 밑방향을 향해 발사할때
                    if (localSclae.x < 0)                                           //플레이어가 왼쪽이면
                    {
                        angle.z *= -1;                                              //회전시작할 위치 값 변경(총알이 뒤로나가는 것을 방지)
                    }
                    else
                    {
                        anglefloat *= -1;                                               //더할 각도 값 변경

                    }
                    for (int i = 0; i < downCrossFirePos.Length; i++)               //발사위치를 아래 발사 위치로 변경
                    {
                        trans[i] = downCrossFirePos[i];
                    }
                    StartCoroutine(CrossFireCoroutin(trans,localSclae,anglefloat));     //사선 발사 코루틴 실행
                    //앞 밑
                    break;
            }
        }
    }
    /// <summary>
    /// 대각선 발사 총알을 만드는 코루틴 함수
    /// </summary>
    /// <param name="pos">발사 위치</param>
    /// <param name="localScale">플레이어 방향</param>
    /// <param name="angleflot">총알 각도</param>
    /// <returns></returns>
    IEnumerator CrossFireCoroutin(Transform[] pos, Vector3 localScale, float angleflot)
    {

        for (int i = 0; i < 3; i++)   //대각선으로 총알을 3번 발사
        {

            angle.z += angleflot;                                                           //각도 값 총알에 계속 더하기
            PoolFactory.Instance.GetMachianGunBullet(pos[i].position, angle, localScale);    //총알 생성
            AmmoDiscount();
            yield return new WaitForSeconds(fireDelay * 0.7f);                                     //딜레이 기다리기
        }
        readyToFire = true;                                                                 //총알발사가 끝났으므로 변수 변경
        crossFiring = false;                                                                //총알발사가 끝났으므로 변수 변경
        anim.OnFiring(false);                                                         //애니메이션 재생
    }
    protected override void EmptyAmmo()
    {
        StopAllCoroutines();
        readyToFire = false;
        anim.OnFiring(false);
        ammoUI.PistolAmmoUI(true);
        base.EmptyAmmo();

    }
    protected override void AmmoDiscount()
    {
        base.AmmoDiscount();
        ammoUI.AmmoDiscount(ammo);
    }
}

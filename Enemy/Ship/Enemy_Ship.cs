using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ship : EnemyHP
{
    /// <summary>
    /// 배의 애니매이테
    /// </summary>
    Animator anim;
    /// <summary>
    /// 배 밑의 물결 애니매이터
    /// </summary>
    Animator waterAnim;
    /// <summary>
    /// 움짐임을 구현하는 스크립트
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// 부숴진 형태의 배
    /// </summary>
    [SerializeField] GameObject brokenShip;


    [SerializeField] Transform explosionRangeA;
    [SerializeField] Transform explosionRangeB;
    /// <summary>
    /// 나무파편A
    /// </summary>
    [SerializeField] GameObject woodA;
    /// <summary>
    /// 나무파편
    /// </summary>
    [SerializeField] GameObject woodB;
    /// <summary>
    /// 나무파편
    /// </summary>
    [SerializeField] GameObject woodC;
    /// <summary>
    /// 나무파편
    /// </summary>
    [SerializeField] GameObject woodD;
    /// <summary>
    /// 연기 오브젝트
    /// </summary>
    [SerializeField] GameObject smoke;
    /// <summary>
    /// 연기가 생산될 위치
    /// </summary>
    [SerializeField] Transform smokePos;

    bool isAlive = true;
    private void Awake()
    {
        movePlatform = GetComponent<MovePlatform>();
        anim = GetComponent<Animator>();
        waterAnim=transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void OnDie()
    {
        if(isAlive)
        {
            isAlive = false;
            base.OnDie();
            anim.SetTrigger("Broken");
            StartCoroutine(Explosion());
        }
    }

    /// <summary>
    /// 배의 움직임을 수행하는 함수
    /// </summary>
    public void MoveStart()
    {

        movePlatform.PlatformStart(movePlatform.moveSpeed);     //움직임 시작
        StartCoroutine(smoking());
    }
    /// <summary>
    /// 배의 움짐임을 멈추는 함수
    /// </summary>
    public void MoveStop()
    {
        movePlatform.MoveStop();                //움직임 정지
    }
    /// <summary>
    /// 배가 부숴지는 함수 애니메이터에서 호출됨
    /// </summary>
    public void Borken()
    {
        gameObject.SetActive(false);            //오브젝트 비활성
        brokenShip.transform.parent = null;     //부숴진 배 부모해제
        brokenShip.SetActive(true);             //부숴진배 활성
        Destroy(gameObject);                    //오브젝트 삭제

    }

    /// <summary>
    /// 배가 부숴질때 폭발 밑 나무 파편 이펙트를 생성하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Explosion()
    {
        for(int i=0;i<8;i++)
        {
            int randint = Random.Range(0, 4);           //랜덤으로 생성할 나무 조각 받기
            GameObject obj = null;
            switch (randint)
            {
                case 0:
                    obj = woodA;
                    break;
                case 1:
                    obj= woodB;
                    break;
                case 2:
                    obj= woodC;
                    break;
                default:
                    obj = woodD;
                    break;
            }
            Vector2 vec = new Vector2(Random.Range(explosionRangeA.position.x, explosionRangeB.position.x),
                Random.Range(explosionRangeA.position.y, explosionRangeB.position.y));      //랜덤위치 받기
            yield return new WaitForSeconds(0.3f);
            Instantiate(obj,vec,Quaternion.identity);       //받은 위치에 나무파편 생성
            PoolFactory.Instance.Get_N_Explosion_M(vec);    //받은 위치에 폭발 이펙트 생성
        }
    }
    /// <summary>
    /// 연기를 생성하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator smoking()
    {
        Instantiate(smoke, smokePos.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(smoking());
    }
}

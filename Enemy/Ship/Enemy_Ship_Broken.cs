using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ship_Broken : EnemyHP
{
    [SerializeField] float brokenTime;
    [SerializeField] float sinkedTime;
    [SerializeField] float sinkSpeed;

    [SerializeField] Transform explosionA;
    [SerializeField] Transform explosionB;
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
    /// 플레이어가 타고갈 배의 스크립트
    /// </summary>
    [SerializeField] Ship ship;
    Animator wateranim;
    bool onBreak=false;
    private void Awake()
    {
        wateranim=GetComponentInChildren<Animator>();
    }
    protected override void OnDie()
    {
        if (!onBreak)
        {
            onBreak = true;
            wateranim.SetTrigger("Sink");
            StartCoroutine(Sink());
            StartCoroutine(Sinked());
        }
    }
    /// <summary>
    /// 가라앉는 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator Sink()
    {
        wateranim.transform.parent = null;                                  //배만 가라앉게 물결의 부모 해제 
        yield return new WaitForSeconds(brokenTime);                        
        Vector2 vec = Vector2.down;         
        while (true)                                                        //밑으로 천천히 이동
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(vec * sinkSpeed * Time.fixedDeltaTime);
        }
    }
    IEnumerator Sinked()
    {
        for(int i = 0; i <8; i++)                                               //부서질때 튕겨나갈 나무 부품 랜덤으로 받기
        {
            int randint = Random.Range(0, 4);
            GameObject obj = null;
            switch (randint)
            {
                case 0:
                    obj = woodA;
                    break;
                case 1:
                    obj = woodB;
                    break;
                case 2:
                    obj = woodC;
                    break;
                default:
                    obj = woodD;
                    break;
            }
            Vector2 vector2=new Vector2(Random.Range(explosionA.position.x,explosionB.position.x),Random.Range(explosionA.position.y,explosionB.position.y));       //위치 랜덤으로 받기
            yield return new WaitForSeconds(sinkedTime*0.1f);       
            PoolFactory.Instance.Get_N_Explosion_M(vector2);            //위 위치에 폭발 이펙트 생성
            Instantiate(obj,vector2,Quaternion.identity);               //폭발 이펙트 생성 위치에 나무 파편 생성

        }
        wateranim.SetTrigger("Sink");           //애니메이션 값 전달
        ship.MoveFront();                       //플레이어배가 앞으로 나아가게하기 
        Destroy(gameObject);
    }
}

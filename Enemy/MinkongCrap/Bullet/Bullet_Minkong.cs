using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Minkong : EnemyBulletBase
{
    /// <summary>
    /// y축으로 움직일 속도 
    /// </summary>
    [SerializeField] float ySpeed;   
    /// <summary>
    /// 애니메이션 커브
    /// </summary>
    [SerializeField] AnimationCurve curve;
    /// <summary>
    /// 총알의 생존시간
    /// </summary>
    [SerializeField] float lifeTime;
    /// <summary>
    /// 활성화 되었을 때 작용받을 힘의 양
    /// </summary>
    [SerializeField] float force;
    /// <summary>
    /// 객체의 리지드바디 
    /// </summary>
    Rigidbody2D rigid;

    CircleCollider2D circleCollider;

    float curTime=0.0f;
    protected override void Awake()
    {
        base.Awake();
        rigid= GetComponent<Rigidbody2D>();             //리지드바디 받기
        circleCollider=GetComponent<CircleCollider2D>();
    }
    private void OnEnable()
    {
        circleCollider.enabled = true;
        rigid.AddForce(new Vector2(transform.localScale.x*force, 0));       //로컬스케일 값에 따라 왼쪽 혹은 오른쪽으로 발사 
        StartCoroutine(ExplosionCoroutin());
    }

    private void FixedUpdate()
    {
        curTime += Time.deltaTime;                              //시간 추가
        float y=curve.Evaluate(curTime);                        //애니메이션 커브에 값전달 밑 저장
        transform.Translate(0, ySpeed *y* Time.deltaTime, 0);   //ySpeed랑 애니메이션 커브 값을 참조로 y축으로 이동
    }
    protected override void HitEffect()
    {
        Explosion();
    }
    /// <summary>
    /// 폭발 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplosionCoroutin()
    {
        yield return new WaitForSeconds(lifeTime);  //기다리기
        Explosion();
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Explosion();
    }
    void Explosion()
    {
        ySpeed = 0.0f;                              //y축 이동 정지
        circleCollider.enabled = false;
        rigid.drag = 1000;                          //x축 이동 정지
        anim.SetTrigger("Explosion");               //폭발 애니메이션 실행
    }
}

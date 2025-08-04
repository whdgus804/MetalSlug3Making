using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine_Morden : Morden
{
    /// <summary>
    /// 타고갈 덩굴의 스프링 조인트
    /// </summary>
    SpringJoint2D springJoint;
    /// <summary>
    /// 해당 객체의 리지드바디
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// 덩굴을 타고 갈때 떨어 뜨릴 잎
    /// </summary>
    [SerializeField] GameObject leaf;
    /// <summary>
    /// 떨어뜨릴 잎을 소환할 장소
    /// </summary>
    [SerializeField] Transform leafSpawnPosition;
    /// <summary>
    /// 처음에 소환된 적만 잎을 떨어뜨려야 함으로 처음으로 생성된 적인지 나타내는 변수
    /// </summary>
    [HideInInspector]public bool first = false;
    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();                    
        springJoint=GetComponentInParent<SpringJoint2D>();
    }
    private void Start()
    {
        secMoveSpeed = 0.0f;
    }
    private void OnEnable()
    {
        anim.SetTrigger("Vine");
        float time = anim.GetCurrentAnimatorStateInfo(0).length;            //애니메이션 시간 받기
        StartCoroutine(RideTime(time));                                     //받은 시간으로 코루틴 실행
        if(first)
            StartCoroutine(LeavesSpawn());
    }
    /// <summary>
    /// 적이 덩굴을 타는 시간
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator RideTime(float time)
    { 
        yield return new WaitForSeconds(time);
        Ejaculation();              //일정 시간 뒤에 덩굴에서 사출
    }
    /// <summary>
    /// 덩굴에서 떨어지는 함수
    /// </summary>
    public void Ejaculation()
    {
        springJoint.connectedBody = null;       //스프링 조인트 끊기 
        transform.parent = null;                //부모 해제
        rigid.drag = 3.0f;                      //저항 증가함으로 의도 된 위치에 떨어뜨리기
    }
    public override void Landing()
    {
        base.Landing();
        secMoveSpeed = 1.0f;                    //땅에 닿을때 움직이게 하기
    }
    /// <summary>
    /// 나뭇잎을 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator LeavesSpawn()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(leaf,leafSpawnPosition.position,Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

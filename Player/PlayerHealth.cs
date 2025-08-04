using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerHealth : MonoBehaviour
{
    //여러 타입의 플레이어가 상속할 HP
    /// <summary>
    /// 스코어 매니저 
    /// </summary>
    ScoreManager scoreManager;

    [SerializeField]protected SpriteRenderer sprite;

    [SerializeField]protected int hp;
    protected GroundSencer groundSencer;
    /// <summary>
    /// 플레이어가 리스폰할 위치
    /// </summary>
    [HideInInspector] public Transform spanwPosition;
    /// <summary>
    /// 플레이어가 현재 살아있는지 나타내는 변수
    /// </summary>
    protected bool isAlive = true;
    public int HP
    {
        get => hp;
        set
        {
            hp -= value;
            if (hp < 1)
            {
                if (!groundSencer.onGround)
                {
                    StartCoroutine(DeadDelay());

                }
                else
                {
                    
                    OnDie();

                }
            }
        }
    }
    bool readToRespawn = false;
    protected virtual void Awake()
    {
        groundSencer=GetComponentInChildren<GroundSencer>();
        scoreManager = GameManager.Instance.ScoreManager; 
    }

    /// <summary>
    /// 플레이어가 땅에 착지할때 실행할 함수
    /// </summary>
    public virtual void Landing()
    {
    }
    /// <summary>
    /// 플레이어가 땅에서 떨어질때 실행할 함수
    /// </summary>
    public virtual void Fly()
    {
    }
    /// <summary>
    /// 리스폰 함수
    /// </summary>
    protected virtual void ReSpawn()
    {
        isAlive = true;
    }
    /// <summary>
    /// 공중에서 죽었을때 싱행되는 함수
    /// </summary>
    protected virtual void FallingDead()
    {

    }
    IEnumerator DeadDelay()
    {
        FallingDead();                                                  //공중에서의 죽음 함수
        yield return new WaitUntil(() => groundSencer.onGround);        //땅에 닿을 때가지 기다리기
        //OnDie();  
        isAlive = false;
        TwinkleStartAndReSpawn();                                       //부활 함수 실행
    }

    /// <summary>
    /// 플레이어의 hp가 1미만으로 떨여질때 실행될 함수
    /// </summary>
    protected virtual void OnDie()
    {
        Debug.Log("dead");
        isAlive = false;
        TwinkleStartAndReSpawn();
    }
    /// <summary>
    /// 죽은 플레이어를 반짝인 뒤 리스폰하는 함수
    /// </summary>
    void TwinkleStartAndReSpawn()
    {

        scoreManager.ReSpawn(this.gameObject);                  
        StartCoroutine(Twinkle());
    }
    /// <summary>
    /// 반짝임 구현 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Twinkle()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 10; i++)
        {
            sprite.color = Vector4.zero;
            yield return new WaitForSeconds(0.05f);
            sprite.color = Vector4.one;
            yield return new WaitForSeconds(0.05f);
        }
        sprite.color = Vector4.zero;
        yield return new WaitForSeconds(1.0f);
        readToRespawn=true;
    }
    public void ReadyToResapwn()
    {
        StartCoroutine(ResapwnWait());  
    }
    IEnumerator ResapwnWait()
    {
        yield return new WaitUntil(() => readToRespawn);
        if (spanwPosition == null)
            spanwPosition = transform;

        transform.position= spanwPosition.position;
        readToRespawn = false;
        //spanwPosition = null;
        ReSpawn();
    }
}

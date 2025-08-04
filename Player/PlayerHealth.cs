using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerHealth : MonoBehaviour
{
    //���� Ÿ���� �÷��̾ ����� HP
    /// <summary>
    /// ���ھ� �Ŵ��� 
    /// </summary>
    ScoreManager scoreManager;

    [SerializeField]protected SpriteRenderer sprite;

    [SerializeField]protected int hp;
    protected GroundSencer groundSencer;
    /// <summary>
    /// �÷��̾ �������� ��ġ
    /// </summary>
    [HideInInspector] public Transform spanwPosition;
    /// <summary>
    /// �÷��̾ ���� ����ִ��� ��Ÿ���� ����
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
    /// �÷��̾ ���� �����Ҷ� ������ �Լ�
    /// </summary>
    public virtual void Landing()
    {
    }
    /// <summary>
    /// �÷��̾ ������ �������� ������ �Լ�
    /// </summary>
    public virtual void Fly()
    {
    }
    /// <summary>
    /// ������ �Լ�
    /// </summary>
    protected virtual void ReSpawn()
    {
        isAlive = true;
    }
    /// <summary>
    /// ���߿��� �׾����� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void FallingDead()
    {

    }
    IEnumerator DeadDelay()
    {
        FallingDead();                                                  //���߿����� ���� �Լ�
        yield return new WaitUntil(() => groundSencer.onGround);        //���� ���� ������ ��ٸ���
        //OnDie();  
        isAlive = false;
        TwinkleStartAndReSpawn();                                       //��Ȱ �Լ� ����
    }

    /// <summary>
    /// �÷��̾��� hp�� 1�̸����� �������� ����� �Լ�
    /// </summary>
    protected virtual void OnDie()
    {
        Debug.Log("dead");
        isAlive = false;
        TwinkleStartAndReSpawn();
    }
    /// <summary>
    /// ���� �÷��̾ ��¦�� �� �������ϴ� �Լ�
    /// </summary>
    void TwinkleStartAndReSpawn()
    {

        scoreManager.ReSpawn(this.gameObject);                  
        StartCoroutine(Twinkle());
    }
    /// <summary>
    /// ��¦�� ���� �ڷ�ƾ �Լ�
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

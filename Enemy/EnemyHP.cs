using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    /// <summary>
    /// 적 혹은 부숴지는 객체가 상속할 스크립트
    /// </summary>
    public int hp;

    public int HP
    {
        get => hp;
        set
        {
            hp -= value;
            if (hp < 1)
            {
                OnDie();
            }
            else
            {
                OnHit();
            }
        }
    }
    /// <summary>
    /// 적이  피격되었을 때 실행 되는 함수
    /// </summary>
    protected virtual void OnHit()
    {

    }
    /// <summary>
    /// 적이 수류탄에 피격되었을때 실행되었을때 실행되는 함수
    /// </summary>
    public  void GrenadeHit(int damage)
    {
        int sethp = hp-damage;                          //hp에 데미지를 전달하기 전에 죽을 피인지 계산
        if (sethp < 1)
        {
            gameObject.layer = 10;
            Explosioned();                              //죽으면 터지는 함수 실행 
            HP = damage;
            //gameObject.SetActive(false);
        }
        else
        {
            HP = damage;                                //아니면 데미지 전달
        }
        
    }
    /// <summary>
    /// 적이 수류탄에 맞고 터질때 실행되는 함수
    /// </summary>
    protected virtual void Explosioned()
    {

    }
    /// <summary>
    /// 적이 죽을 때 실행되는 함수
    /// </summary>
    protected virtual void OnDie()
    {

    }
    /// <summary>
    /// 적이 땅에 착지할때 실행되는 함수
    /// </summary>
    public virtual void Landing()
    {

    }
    /// <summary>
    /// 적이 공중에 있을때 실행되는 함수
    /// </summary>
    public virtual void Fly()
    {

    }
}

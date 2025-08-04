using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolFactory : Singleton<PoolFactory>   
{
    PistolBulletPool pistolBulletPool;      //권총총알 풀

    Effect_BulletGroundHitDownPool effect_BulletGroundHitDownPool;  //총알이 땅에 부딪히는 이펙트
    Effect_BulletGroundHitSidePool effect_BulletGroundHitSidePool;

    Effect_HitPool effect_HitPool;


    BoamStickPool boamStickPool;
    /// <summary>
    /// 수류탄 폭발 이펙트
    /// </summary>
    Explosion_APool explosion_APool;

    MachaingunBulletPool machaingunBulletPool;

    DustPool dustPool;

    N_Explosion_M_Pool n_Explosion_M_Pool;

    N_Explosion_S_Pool n_Explosion_S_Pool;
    private void Start()
    {
        OnInitaOnInitalize();                                           //스크립트를 받고 풀 생성함수 실행
    }
    /// <summary>
    /// 풀의 스크립트를 지정하고 풀링하는 함수
    /// </summary>
    private void OnInitaOnInitalize()
    {
        pistolBulletPool=GetComponentInChildren<PistolBulletPool>();        //스크립트 받기
        if (pistolBulletPool != null )                                       //스크립트가 있으면
            pistolBulletPool.Initalize();                                   //풀 만들기

        effect_BulletGroundHitDownPool = GetComponentInChildren<Effect_BulletGroundHitDownPool>();  
        if(effect_BulletGroundHitDownPool != null )
            effect_BulletGroundHitDownPool.Initalize();

        effect_BulletGroundHitSidePool = GetComponentInChildren<Effect_BulletGroundHitSidePool>();
        if(effect_BulletGroundHitSidePool != null )
            effect_BulletGroundHitSidePool.Initalize();

        effect_HitPool=GetComponentInChildren<Effect_HitPool>();
        if( effect_HitPool != null )
            effect_HitPool.Initalize();

        boamStickPool=GetComponentInChildren<BoamStickPool>();
        if(boamStickPool != null )
            boamStickPool.Initalize();


        explosion_APool=GetComponentInChildren<Explosion_APool>();  
        if(explosion_APool != null )
            explosion_APool.Initalize();

        machaingunBulletPool=GetComponentInChildren<MachaingunBulletPool>();
        if(machaingunBulletPool != null )
            machaingunBulletPool.Initalize();

        dustPool=GetComponentInChildren<DustPool>();
        if (dustPool != null )
            dustPool.Initalize();

        n_Explosion_M_Pool=GetComponentInChildren<N_Explosion_M_Pool>();
        if(n_Explosion_M_Pool != null )
            n_Explosion_M_Pool.Initalize();

        n_Explosion_S_Pool=GetComponentInChildren<N_Explosion_S_Pool>();
        if(n_Explosion_S_Pool != null )
            n_Explosion_S_Pool.Initalize();
        
    }
    /// <summary>
    /// 권총총알을 만드는 함수
    /// </summary>
    /// <param name="position">위치</param>
    /// <param name="rotation">회전값</param>
    /// <param name="scale">스케일</param>
    /// <returns></returns>
    public PistolBullet GetPistolBullet(Vector2 position,Vector3 rotation, Vector2 scale)
    {
        return pistolBulletPool.UseGameObject(position,rotation,scale); 
    }

    public Effect GetGroundHitDown(Vector2 position)
    {
        return effect_BulletGroundHitDownPool.UseGameObject(position, null, Vector3.one);
    }
    public Effect GetGroundHitSide(Vector2 position,Vector3 rotation, Vector3 scale)
    {
        return effect_BulletGroundHitSidePool.UseGameObject(position,rotation,scale);
    }
    public Effect GetBulletHitEffect(Vector2 position)
    {
        return effect_HitPool.UseGameObject(position, null, Vector3.one);
    }
    public BoamStick GetBoamStick(Vector2 position,Vector2 scale)
    {
        return boamStickPool.UseGameObject(position,null,scale);
    }

    public Effect GetExplosion_A(Vector2 position)
    {
        return explosion_APool.UseGameObject(position,null,Vector3.one);
    }
    public MachainBullet GetMachianGunBullet(Vector2 position,Vector3 rotation,Vector3 localScale)
    {
        return machaingunBulletPool.UseGameObject(position,rotation,localScale);
    }
    public Effect GetDust(Vector2 position)
    {
        return dustPool.UseGameObject(position, null, Vector3.one);
    }
    public RecycleObject Get_N_Explosion_M(Vector2 position)
    {
        return n_Explosion_M_Pool.UseGameObject(position,null,Vector2.one);
    }
    public RecycleObject Get_N_Explosion_S(Vector2 position)
    {
        return n_Explosion_S_Pool.UseGameObject (position,null,Vector2.one);
    }
}

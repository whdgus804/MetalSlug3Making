using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolFactory : Singleton<PoolFactory>   
{
    PistolBulletPool pistolBulletPool;      //�����Ѿ� Ǯ

    Effect_BulletGroundHitDownPool effect_BulletGroundHitDownPool;  //�Ѿ��� ���� �ε����� ����Ʈ
    Effect_BulletGroundHitSidePool effect_BulletGroundHitSidePool;

    Effect_HitPool effect_HitPool;


    BoamStickPool boamStickPool;
    /// <summary>
    /// ����ź ���� ����Ʈ
    /// </summary>
    Explosion_APool explosion_APool;

    MachaingunBulletPool machaingunBulletPool;

    DustPool dustPool;

    N_Explosion_M_Pool n_Explosion_M_Pool;

    N_Explosion_S_Pool n_Explosion_S_Pool;
    private void Start()
    {
        OnInitaOnInitalize();                                           //��ũ��Ʈ�� �ް� Ǯ �����Լ� ����
    }
    /// <summary>
    /// Ǯ�� ��ũ��Ʈ�� �����ϰ� Ǯ���ϴ� �Լ�
    /// </summary>
    private void OnInitaOnInitalize()
    {
        pistolBulletPool=GetComponentInChildren<PistolBulletPool>();        //��ũ��Ʈ �ޱ�
        if (pistolBulletPool != null )                                       //��ũ��Ʈ�� ������
            pistolBulletPool.Initalize();                                   //Ǯ �����

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
    /// �����Ѿ��� ����� �Լ�
    /// </summary>
    /// <param name="position">��ġ</param>
    /// <param name="rotation">ȸ����</param>
    /// <param name="scale">������</param>
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

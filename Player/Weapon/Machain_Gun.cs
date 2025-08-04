using System.Collections;
using UnityEngine;

public class Machain_Gun : WeaponBase
{
    /// <summary>
    /// ������ �߻� ������ ������ ����0��1��2�ɱ�3��
    /// </summary>
    int beforSign = 0;                                  
    /// <summary>
    /// ������ �߻��� �Ѿ��� ������ ������ ����
    /// </summary>
    Vector3 angle = Vector3.zero;
    /// <summary>
    /// �缱�߻������� ��Ÿ���� ���� true�� �߻��� 
    /// </summary>
    bool crossFiring=false;

    /// <summary>
    /// �� �缱 �߻� ��ġ
    /// </summary>
    [SerializeField]Transform[] upCrossFirePos;
    /// <summary>
    /// �Ʒ� �缱 �߻���ġ
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
        if (readyToFire)            //�߻簡���̸�
        {
            angle = aimVector;      //��������
            beforSign = sign;       //������ �߻���ġ ����
            readyToFire = false;    //�߻��� �̹Ƿ� ���� ����
            anim.OnFiring(true);
            StartCoroutine(ShootCoroutin(aimPositon,aimVector,localScale)); //�Ѿ��� 3�� �߻��ϴ� �ڷ�ƾ ����
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
    /// �Ѿ��� 3�� �߻��ϴ� �ڷ�ƾ�Լ�
    /// </summary>
    /// <param name="aimPositon">�߻� ��ġ</param>
    /// <param name="aimVector">�߻簢��</param>
    /// <param name="localScale">�÷��̾��� ����</param>
    /// <returns></returns>
    IEnumerator ShootCoroutin(Transform aimPositon, Vector3 aimVector, Vector3 localScale)
    {
        for (int i = 0; i < 3; i++)         //3�� �ݺ�
        {
            int randint = Random.Range(-3, 4);                                  //�Ѿ��� ���� �������� ��Ѹ��Բ� ������ ����
            aimVector.z += randint;                                             //�������� �Ѿ˰����� ���ϱ�
            //Debug.Log(localScale);
            if(localScale.x < 0)        //�÷��̾ ���ʹ����̸�
            {
                //Debug.Log(aimVector);
                if (aimVector.z < -45 || aimVector.z > 45)  //�Ѿ��� �������°��� ���� �ڸ� �� ���·� ���� �߻��Ҷ� �Ѿ� ���� ����
                {
                    localScale = Vector3.one;               //���� �����ϰ� ����
                }
            }
            PoolFactory.Instance.GetMachianGunBullet(aimPositon.position, aimVector, localScale);       //Ǯ���丮���� �Ѿ� ����
            AmmoDiscount();
            yield return new WaitForSeconds(fireDelay);                                                 //�߻� �����̱�ٸ���
        }
        readyToFire=true;                                                                           //�Ѿ��� �߻�Ǹ� �ٽ� �߻� �����ϵ��� ���� ����
        yield return new WaitForSeconds(0.2f);
        anim.OnFiring(false);                                                                 //�ִϸ��̼� ���
        //�Ѿ� ����
    }
    /// <summary>
    /// �缱�߻��Ҷ� �Ѿ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sign">�߻� ��ġ</param>
    /// <param name="localSclae">�÷��̾� ����</param>
    public void CrossFire(int sign,Vector3 localSclae)
    {
        if (!readyToFire && beforSign!=2 && !crossFiring&& sign!=2)         //�߻����̰� ����߻� ��ġ�� ������ �߻� ��ġ�� �ɴ� �ڼ��� �ƴϸ� �缱 �߻����� �ƴϸ�
        {
            
            float anglefloat = 18.0f;                           //������ �Ѿ��� ���� ��
            crossFiring = true;                                 //�ߺ� ��������� ���� �缱 �߻��� ���� ����
            Transform[] trans=new Transform[3];                 //�缱 �߻���ġ�� ������ �迭 ����
            StopAllCoroutines();                                //���� ���� �߻�����
            switch(sign)                                        //�߻� ��ġ������ �� ����
            {
                case 0:                             //���϶�
                    if (beforSign == 1)
                    {
                        //������ �� ���� �缱 �����϶�
                        if (localSclae.x < 0)                                                //�÷��̾� ������ �����̸�
                        {       
                            localSclae=Vector3.one;                                          //�Ѿ� ���� ���� (�ڷ� ���ư��� ���� ����)
                        }
                        else
                        {
                            anglefloat*= -1;                                                     //���� ���� 

                        }
                        for(int i = 0; i < upCrossFirePos.Length; i++)                       //�߻���ġ�� �� �߻���ġ�� �������� ����
                        {
                            trans[i]= upCrossFirePos[2-i];
                        }
                        StartCoroutine(CrossFireCoroutin(trans, localSclae, anglefloat));   //�缱���� �Ѿ��� �߻��ϴ� �ڷ�ƾ �Լ� ����
                    }
                    else
                    {
                        //�ؿ��� �� ���� �缱�϶� 
                        if (localSclae.x < 0)                                            //�÷��̾� ������ �����̸�
                        {
                            anglefloat *= -1;                                            //���� ���� �� ����
                            angle.z *= -1;                                               //���� ����(�Ѿ��� �ٸ� �������� ���ư��� ���� ����)
                        }
                        for (int i = 0; i < downCrossFirePos.Length; i++)                //�߻���ġ�� �Ʒ� �߻���ġ�� �������� ����
                        {
                            trans[i] = downCrossFirePos[2 - i]; 
                        }
                        StartCoroutine(CrossFireCoroutin(trans, localSclae, anglefloat)); //�밢�� �߻� �ڷ�ƾ�Լ� ����
                        //�� ��
                    }
                    break;
                case 1:
                    //�տ��� ���� �⿡ �߻��Ҷ�
                    if (localSclae.x < 0)                                                  //�÷��̾� ������ �޹����̸�
                    {
                        anglefloat *= -1;                                                  //���� ������ ����
                    }
                    for (int i = 0; i < upCrossFirePos.Length; i++)                        //�߻���ġ�� ���߻� ��ġ�� ����
                    {
                        trans[i] = upCrossFirePos[i];
                    }
                    StartCoroutine(CrossFireCoroutin(trans,localSclae, anglefloat));       //�缱�߻� �ڷ�ƾ �Լ� ����
                    //�� ��
                    break;
                case 3:
                    //�տ��� �ع����� ���� �߻��Ҷ�
                    if (localSclae.x < 0)                                           //�÷��̾ �����̸�
                    {
                        angle.z *= -1;                                              //ȸ�������� ��ġ �� ����(�Ѿ��� �ڷγ����� ���� ����)
                    }
                    else
                    {
                        anglefloat *= -1;                                               //���� ���� �� ����

                    }
                    for (int i = 0; i < downCrossFirePos.Length; i++)               //�߻���ġ�� �Ʒ� �߻� ��ġ�� ����
                    {
                        trans[i] = downCrossFirePos[i];
                    }
                    StartCoroutine(CrossFireCoroutin(trans,localSclae,anglefloat));     //�缱 �߻� �ڷ�ƾ ����
                    //�� ��
                    break;
            }
        }
    }
    /// <summary>
    /// �밢�� �߻� �Ѿ��� ����� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="pos">�߻� ��ġ</param>
    /// <param name="localScale">�÷��̾� ����</param>
    /// <param name="angleflot">�Ѿ� ����</param>
    /// <returns></returns>
    IEnumerator CrossFireCoroutin(Transform[] pos, Vector3 localScale, float angleflot)
    {

        for (int i = 0; i < 3; i++)   //�밢������ �Ѿ��� 3�� �߻�
        {

            angle.z += angleflot;                                                           //���� �� �Ѿ˿� ��� ���ϱ�
            PoolFactory.Instance.GetMachianGunBullet(pos[i].position, angle, localScale);    //�Ѿ� ����
            AmmoDiscount();
            yield return new WaitForSeconds(fireDelay * 0.7f);                                     //������ ��ٸ���
        }
        readyToFire = true;                                                                 //�Ѿ˹߻簡 �������Ƿ� ���� ����
        crossFiring = false;                                                                //�Ѿ˹߻簡 �������Ƿ� ���� ����
        anim.OnFiring(false);                                                         //�ִϸ��̼� ���
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

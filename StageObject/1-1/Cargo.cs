using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    //�÷��̾ �����̿��� ������
    //�� ��ȯ 2��
    //���� �ð� ���� ��������

    /// <summary>
    /// �ൿ ������ �˸� Ʈ����
    /// </summary>
    [SerializeField] BoxCollider2D trigger;

    /// <summary>
    /// �� �ʿ��� ������ ��
    /// </summary>
    [SerializeField] GameObject upEnemy;
    /// <summary>
    /// ���� ���� ������ ��ġ
    /// </summary>
    [SerializeField] Transform upPos;
    /// <summary>
    /// �Ʒ��ʿ��� ������ ��
    /// </summary>
    [SerializeField] GameObject downEnemy;
    /// <summary>
    /// �Ʒ� ���� ������ ��ġ
    /// </summary>
    [SerializeField] Transform downPos;
    /// <summary>
    /// ���� ������ ��
    /// </summary>
    [SerializeField] int count;
    /// <summary>
    /// ���� ������ ������
    /// </summary>
    [SerializeField] float spawnDelay;

    /// <summary>
    /// ��������Ʈ
    /// </summary>
    [SerializeField] GameObject explosion;
    /// <summary>
    /// ���� ��ġ
    /// </summary>
    [SerializeField] Transform[] explosionPos;
    /// <summary>
    /// �ν����� ������ npc
    /// </summary>
    [SerializeField] GameObject supplyNPC;
    /// <summary>
    /// �ν����� ������ npc�� ������ġ
    /// </summary>
    [SerializeField] Transform supplyPos;

    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;
    /// <summary>
    /// �÷��̾ ����ٴϴ� ī�޶�
    /// </summary>
    CameraPointReset playerCamera;
    PlayerHealth player;
    private void Awake()
    {
        anim = GetComponent<Animator>();        //�ִϸ����� �ޱ�
        playerCamera = GetComponentInChildren<CameraPointReset>();
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))             //�÷��̾ �����̿���
        {
            trigger.enabled = false;                    //�ߺ� ���� ����
            anim.SetTrigger("Player");                  //��������
        }
         
    }

    /// <summary>
    /// ���� ��ȯ�ϴ� �ڷ�ƾ �Լ��� �����ϴ� �Լ� 
    /// </summary>
    public void EnemySpawn()
    {

        StartCoroutine(EnemySpawnCoroutin());               //�� �����ϴ� �ڷ�ƾ ����
    }

    /// <summary>
    /// ���� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemySpawnCoroutin()
    {
        WaitUntil waitHPPlayer = new WaitUntil(() => player.HP > 0);
        yield return new WaitForSeconds(1.0f);                                              //��ٸ���
        for(int i = 0; i < count; i++)                                                      //������ ����ŭ �ݺ� ����ȯ
        {
            yield return waitHPPlayer;
            Instantiate(upEnemy, upPos.position, Quaternion.identity);                      //����ȯ
            Instantiate(downEnemy, downPos.position, Quaternion.identity);                  //����ȯ
            yield return new WaitForSeconds(spawnDelay*2.0f);                                    //���� ������ ��ٸ���
            
            if (i == count-1)                                                               //���� ���� ��� �����Ǹ�
            {
                for (int j = 0; j < explosionPos.Length; j++)                               //���� ��ġ ��ŭ
                {
                    Instantiate(explosion, explosionPos[j].position, Quaternion.identity);  //���� ����Ʈ ����
                }
                Instantiate(supplyNPC, supplyPos.position, Quaternion.identity);            //npc����
                yield return waitHPPlayer;
                playerCamera.ResetCameraPos();
                gameObject.SetActive(false);                                                //���ӿ�����Ʈ ��Ȱ��ȭ
            }
        }
    }


}

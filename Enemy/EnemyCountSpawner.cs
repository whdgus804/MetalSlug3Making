using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCountSpawner : MonoBehaviour
{

    /// <summary>
    /// ������ ��
    /// </summary>
    [SerializeField] int spawnCount;
    /// <summary>
    /// ���� ������
    /// </summary>
    [SerializeField] float spawnDelay;

    /// <summary>
    /// ������ ��
    /// </summary>
    [SerializeField] GameObject spawnEnemy;

    [SerializeField] GameObject cameraPoint;

    [HideInInspector] public int count;
    /// <summary>
    /// Ʈ����
    /// </summary>
    BoxCollider2D boxCollider2D;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();          //Ʈ���� �ޱ�
    }
    private void OnEnable()
    {
        count = spawnCount;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SapnwEnemy();
            boxCollider2D.enabled = false;
            StartCoroutine(Spawn());
            StartCoroutine(WaitCameraSet());
        }
    }
    void SapnwEnemy()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject obj = Instantiate(spawnEnemy, transform);
            obj.SetActive(false);
            //Instantiate(spawnEnemy, transform);
        }
    }
    IEnumerator Spawn()
    {
        WaitForSeconds wait=new WaitForSeconds(spawnDelay);
        for (int i = 0;i < spawnCount;i++)
        {
            GameObject obj = transform.GetChild(0).gameObject;
            obj.SetActive(true);
            obj.transform.parent = null;
            yield return wait;
        }
    }
    IEnumerator WaitCameraSet()
    {
        WaitUntil wait = new WaitUntil(() => count < 1);
        yield return wait;
        CameraPointReset cameraPointReset=cameraPoint.GetComponent<CameraPointReset>();
        cameraPointReset.ResetCameraPos();
        cameraPoint.gameObject.SetActive(false);    
    }
}

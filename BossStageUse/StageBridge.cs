using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBridge : MonoBehaviour
{
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// ���� ��ǰ
    /// </summary>
    [SerializeField] GameObject woodParts;
    /// <summary>
    /// ��������Ʈ
    /// </summary>
    [SerializeField] GameObject woodEffect;
    /// <summary>
    /// �� ����Ʈ
    /// </summary>
    [SerializeField] GameObject barrel;
    /// <summary>
    /// ����Ʈ ��ȯ��ġ
    /// </summary>
    [SerializeField] Transform[] effectPoses;

    [SerializeField]
    Transform[] dustPos;
    /// <summary>
    /// �̵� ����
    /// </summary>
    Vector2 force = Vector2.left;

    BossStageManager stageManager;

    [SerializeField] bool onStartOBJ = false;
    private void Awake()
    {
        stageManager = FindAnyObjectByType<BossStageManager>();
    }

    private void OnEnable()
    {
        if (onStartOBJ)
        {
            StartCoroutine(WaitStart());

        }
        else
        {
            StartCoroutine(GameOver());
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(force*moveSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroied"))
        {
            Destroied();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destroied"))
        {
            Destroied();
        }
    }
    void Destroied()
    {
        for (int i = 0; i < dustPos.Length; i++)
        {
            PoolFactory.Instance.GetDust(dustPos[i].position);
        }
        Instantiate(woodParts, transform.position, Quaternion.identity);
        Instantiate(barrel, transform.position, Quaternion.identity);
        for(int i = 0; i < effectPoses.Length; i++)
        {
            Instantiate(woodEffect, effectPoses[i].position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    IEnumerator WaitStart()
    {
        float set = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(3.0f);
        moveSpeed = set;
        StartCoroutine(GameOver());
    }
    IEnumerator GameOver()
    {
        WaitUntil wait = new WaitUntil(()=>stageManager.gameover);
        yield return wait;
        moveSpeed = 0.0f;
    }
}

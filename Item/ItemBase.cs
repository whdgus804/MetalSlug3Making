using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// ȹ��� ���� ����
    /// </summary>
    [SerializeField] int itemScore;

    /// <summary>
    /// ������ ����� ���ھ� �Ŵ�����ũ��Ʈ
    /// </summary>
    protected ScoreManager scoreManager;
    /// <summary>
    /// �������� ��������Ʈ
    /// </summary>
    SpriteRenderer sprite;
    protected virtual void Awake()
    {
        scoreManager=GameManager.Instance.ScoreManager;         //���ھ� �Ŵ��� �ޱ�
        sprite=GetComponent<SpriteRenderer>();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))             //Ʋ���Ű� �÷��̾�� �ε����� ���� �߰� �Լ� ���� �� ��������
        {
            GetItem(collision.gameObject);                                  //�Լ� ����
        }
    }
    /// <summary>
    /// �÷��̾ �������� ������ ����Ǵ� �Լ�
    /// </summary>
    protected virtual void GetItem(GameObject player)
    {
        scoreManager.Score += itemScore;
        gameObject.SetActive(false);                //������Ʈ ��Ȱ��ȭ

    }
    /// <summary>
    /// �������� ���� �ð� �̻� ���� �� ������� �Ҷ� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="time">������ �ð�</param>
    protected void DisableTime(float time)
    {
        StartCoroutine(DisableCoroutine(time));
    } 
    /// <summary>
    /// ���� �ð� �� �������� ���ִ� �ڷ�ƾ
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DisableCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < 10; i++)
        {
            sprite.color = Vector4.zero;
            yield return new WaitForSeconds(0.05f);
            sprite.color = Vector4.one;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}

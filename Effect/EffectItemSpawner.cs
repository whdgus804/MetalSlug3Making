using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItemSpawner : MonoBehaviour
{
    /// <summary>
    /// ������ ����Ʈ
    /// </summary>
    [SerializeField]GameObject[] effect;

    [SerializeField] int count;

    [SerializeField] Transform spawnPos;


    private void Start()
    {
        if(spawnPos == null)
        {
            spawnPos = transform;           //������ ��ġ�� ������ �ش� ������Ʈ�� ��ġ�� ����
        }
    }

    /// <summary>
    /// �������� ��ȯ�ϴ� �Լ�
    /// </summary>
    public void ItemSpawner()
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < effect.Length; j++)
            {
                Instantiate(effect[j], spawnPos.position, Quaternion.identity);     //����� Ƚ����ŭ ��� ����Ʈ�� ��ȯ
            }
        }


    }
}

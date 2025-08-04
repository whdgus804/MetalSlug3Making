using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItemSpawner : MonoBehaviour
{
    /// <summary>
    /// 생성될 이펙트
    /// </summary>
    [SerializeField]GameObject[] effect;

    [SerializeField] int count;

    [SerializeField] Transform spawnPos;


    private void Start()
    {
        if(spawnPos == null)
        {
            spawnPos = transform;           //지정된 위치가 없으면 해당 오브젝트의 위치를 저장
        }
    }

    /// <summary>
    /// 아이템을 소환하는 함수
    /// </summary>
    public void ItemSpawner()
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < effect.Length; j++)
            {
                Instantiate(effect[j], spawnPos.position, Quaternion.identity);     //저장된 횟수만큼 모든 이펙트를 소환
            }
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpanwer : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject bridge;
    [SerializeField] GameObject[] slopBrige;

    bool onslop = false;
    private void OnEnable()
    {
        StartCoroutine(WaitSlop());
    }

    public void SpawnStart()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        if (onslop)
        {
            for (int i = 0; i < slopBrige.Length; i++)
            {
                Instantiate(slopBrige[i], spawnPos.position,Quaternion.identity);
                yield return new WaitForSeconds(0.9f);
            }
            onslop = false;
            StartCoroutine(WaitSlop());
        }
        else
        {

            Spawn();
            yield return new WaitForSeconds(0.9f);
        }
        StartCoroutine(SpawnCoroutine());
    }
    void Spawn()
    {

        Instantiate(bridge, spawnPos.position,Quaternion.identity);
    }

    IEnumerator WaitSlop()
    {
        yield return new WaitForSeconds(6.0f);
        onslop= true;
    }
    public void SpawnOver()
    {
        StopAllCoroutines();
    }
}

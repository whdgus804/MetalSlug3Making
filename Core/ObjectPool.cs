using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//RecycleObject 해당 오브젯트를 상속받아야 가능
public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{

    /// <summary>
    /// 풀사이즈
    /// </summary>
    [SerializeField] int poolSize;
    /// <summary>
    /// 풀링할 오브젝트 
    /// </summary>
    [SerializeField] GameObject poolObject;

    /// <summary>
    /// 풀 T로 되어있는 배열
    /// </summary>
    T[] pool;
    /// <summary>
    /// T 로 되었는 Queue 배열
    /// </summary>
    Queue<T> queue;
    /// <summary>
    /// pool이 없으면 풀링을 만드는 함수
    /// </summary>
    public virtual void Initalize()
    {
        if (pool == null)
        {
            pool = new T[poolSize];             //풀사이즈 만큼 배열 초기화
            queue = new Queue<T>(poolSize);       //큐에 풀사이즈만큼 초기화
            SpawnGameObject(0, poolSize, pool);     //처음 부터 만들기 0 전달
        }
    }
    private void SpawnGameObject(int startIndex, int Size, T[] t)
    {
        for (int i = 0; i < Size; i++)
        {
            GameObject obj = Instantiate(poolObject, transform);     //자식으로 오브젝트 하나 생성
            obj.name = $"{poolObject.name}_{i}";                        //오브젝트 이름 변경 
            T comp = obj.GetComponent<T>();                              //해당 스크립트 가져오기 Recleobject
            comp.onDisable += () =>
            {
                queue.Enqueue(comp);                                //델리게이트에 내용 추가: 큐에 하나 추가
            };
            SpawnAwake();
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// 풀이 생성될때 처음 실행될 빈함수
    /// </summary>
    protected virtual void SpawnAwake()
    {

    }
    /// <summary>
    /// 오브젝트를 사용할때 실행하는 함수
    /// </summary>
    /// <param name="position">위치</param>
    /// <param name="rotation"> 회전값</param>
    /// <returns></returns>
    public T UseGameObject(Vector3? position = null, Vector3? rotation = null,Vector3? localScale=null)
    {
        if (queue.Count > 0)
        {
            T compe = queue.Dequeue();                                                  //큐에서 제거

            compe.transform.position = position.GetValueOrDefault();                          //위치 변경 값을 받았으면 해당 위치에 없으면 초기 위치에
            compe.transform.rotation = Quaternion.Euler(rotation.GetValueOrDefault());        //회전값을 받은 백터값을 오일러값에서 쿼터니언값으로 변경 해당 값을 로테이션값에 넣음 
            compe.transform.localScale= localScale.GetValueOrDefault();


            compe.gameObject.SetActive(true);                                                 //오브젝트 활성화 
            return compe;
        }
        else
        {
            QueueMaxAdd();
            return UseGameObject();
        }
    }
    /// <summary>
    /// 풀사이즈 보다 많은 오브젝트가 활성화 되었을때 풀사이즈를 2배로늘리는 함수
    /// </summary>
    private void QueueMaxAdd()
    {
        int size = poolSize * 2;                                                        //풀사이즈 2배 만큼 저장
        T[] newPool = new T[size];                                                      //배열 초기화
        for (int i = 0; i < poolSize; i++)                                              //기본 사이즈 만킄 반복
        {
            newPool[i] = pool[i];                                                       //새 배열에 기존 배열내용 추가
        }
        SpawnGameObject(poolSize, size, newPool);                                       //풀링
        pool = newPool;                                                                 //풀 재지정
        poolSize = size;                                                                //풀사이즈 재지정
    }
}

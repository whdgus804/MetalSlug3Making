using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//RecycleObject �ش� ������Ʈ�� ��ӹ޾ƾ� ����
public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{

    /// <summary>
    /// Ǯ������
    /// </summary>
    [SerializeField] int poolSize;
    /// <summary>
    /// Ǯ���� ������Ʈ 
    /// </summary>
    [SerializeField] GameObject poolObject;

    /// <summary>
    /// Ǯ T�� �Ǿ��ִ� �迭
    /// </summary>
    T[] pool;
    /// <summary>
    /// T �� �Ǿ��� Queue �迭
    /// </summary>
    Queue<T> queue;
    /// <summary>
    /// pool�� ������ Ǯ���� ����� �Լ�
    /// </summary>
    public virtual void Initalize()
    {
        if (pool == null)
        {
            pool = new T[poolSize];             //Ǯ������ ��ŭ �迭 �ʱ�ȭ
            queue = new Queue<T>(poolSize);       //ť�� Ǯ�����ŭ �ʱ�ȭ
            SpawnGameObject(0, poolSize, pool);     //ó�� ���� ����� 0 ����
        }
    }
    private void SpawnGameObject(int startIndex, int Size, T[] t)
    {
        for (int i = 0; i < Size; i++)
        {
            GameObject obj = Instantiate(poolObject, transform);     //�ڽ����� ������Ʈ �ϳ� ����
            obj.name = $"{poolObject.name}_{i}";                        //������Ʈ �̸� ���� 
            T comp = obj.GetComponent<T>();                              //�ش� ��ũ��Ʈ �������� Recleobject
            comp.onDisable += () =>
            {
                queue.Enqueue(comp);                                //��������Ʈ�� ���� �߰�: ť�� �ϳ� �߰�
            };
            SpawnAwake();
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// Ǯ�� �����ɶ� ó�� ����� ���Լ�
    /// </summary>
    protected virtual void SpawnAwake()
    {

    }
    /// <summary>
    /// ������Ʈ�� ����Ҷ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">��ġ</param>
    /// <param name="rotation"> ȸ����</param>
    /// <returns></returns>
    public T UseGameObject(Vector3? position = null, Vector3? rotation = null,Vector3? localScale=null)
    {
        if (queue.Count > 0)
        {
            T compe = queue.Dequeue();                                                  //ť���� ����

            compe.transform.position = position.GetValueOrDefault();                          //��ġ ���� ���� �޾����� �ش� ��ġ�� ������ �ʱ� ��ġ��
            compe.transform.rotation = Quaternion.Euler(rotation.GetValueOrDefault());        //ȸ������ ���� ���Ͱ��� ���Ϸ������� ���ʹϾ����� ���� �ش� ���� �����̼ǰ��� ���� 
            compe.transform.localScale= localScale.GetValueOrDefault();


            compe.gameObject.SetActive(true);                                                 //������Ʈ Ȱ��ȭ 
            return compe;
        }
        else
        {
            QueueMaxAdd();
            return UseGameObject();
        }
    }
    /// <summary>
    /// Ǯ������ ���� ���� ������Ʈ�� Ȱ��ȭ �Ǿ����� Ǯ����� 2��δø��� �Լ�
    /// </summary>
    private void QueueMaxAdd()
    {
        int size = poolSize * 2;                                                        //Ǯ������ 2�� ��ŭ ����
        T[] newPool = new T[size];                                                      //�迭 �ʱ�ȭ
        for (int i = 0; i < poolSize; i++)                                              //�⺻ ������ ���� �ݺ�
        {
            newPool[i] = pool[i];                                                       //�� �迭�� ���� �迭���� �߰�
        }
        SpawnGameObject(poolSize, size, newPool);                                       //Ǯ��
        pool = newPool;                                                                 //Ǯ ������
        poolSize = size;                                                                //Ǯ������ ������
    }
}

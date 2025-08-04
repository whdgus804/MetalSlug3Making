using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)                                        //�ν��Ͻ��� ������
            {
                T singleton = FindAnyObjectByType<T>();                   //������Ʈ ã��
                if (singleton == null)                                  //���� ���ٸ�
                {
                    GameObject obj = new GameObject();                  //������Ʈ����
                    obj.name = $"{typeof(T)}_Singleton";                //�̸� ����
                    singleton = obj.AddComponent<T>();                    //������Ʈ �߰�
                }
                instance = singleton;                                   //�ν��Ͻ��� �� �ֱ�
                DontDestroyOnLoad(instance.gameObject);                 //���� �ٲ� �������� �ʰ� �ϱ� 
            }
            return instance;                                            //�ν��Ͻ� ��ȯ
        }
    }
    private void Awake()
    {
        if (instance == null)                                           //�ν��Ͻ��� ������
        {
            instance = this as T;                                       //ĳ����
            DontDestroyOnLoad(instance.gameObject);                     //���� �ٲ� �������� �ʰ� �ϱ�
        }
        else
        {
            if (instance != this)                                       //���� �ν��Ͻ��� �� ��ü�� �ƴϸ�
            {
                Destroy(this.gameObject);                               //�� ��ü ����
            }
        }
    }
}
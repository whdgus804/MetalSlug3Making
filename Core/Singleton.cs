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
            if (instance == null)                                        //인스턴스가 없으면
            {
                T singleton = FindAnyObjectByType<T>();                   //오브젝트 찾기
                if (singleton == null)                                  //만약 없다면
                {
                    GameObject obj = new GameObject();                  //오브젝트생성
                    obj.name = $"{typeof(T)}_Singleton";                //이름 변경
                    singleton = obj.AddComponent<T>();                    //컴포넌트 추가
                }
                instance = singleton;                                   //인스턴스에 값 넣기
                DontDestroyOnLoad(instance.gameObject);                 //씬이 바뀌어도 없어지지 않게 하기 
            }
            return instance;                                            //인스턴스 반환
        }
    }
    private void Awake()
    {
        if (instance == null)                                           //인스턴스가 없으면
        {
            instance = this as T;                                       //캐스팅
            DontDestroyOnLoad(instance.gameObject);                     //씬이 바뀌어도 없어지지 않게 하기
        }
        else
        {
            if (instance != this)                                       //만약 인스턴스가 이 객체가 아니면
            {
                Destroy(this.gameObject);                               //이 객체 제거
            }
        }
    }
}
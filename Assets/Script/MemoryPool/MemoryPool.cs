using System.Collections.Generic;
using UnityEngine;

public abstract class MemoryPool : MonoBehaviour
{
    //public static MemoryPool Instance { get; private set; }
    protected abstract void MakeSingleTone();
    //{
    //    // 싱글톤
    //    if (Instance == null)
    //    {
    //        Instance = this; // 스크립트 컴포넌트
    //    }
    //    else
    //    {   // 이미 인스턴스, 즉 싱글톤이 완성됐으면 현재의 객체는 싱글톤을 방해하는 객체이니 소멸시킴
    //        Destroy(gameObject); // 가비지컬렉터에 메모리 반납
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject); // 다른 씬으로 갈때 파괴되지 않음
    //    // 씬을 옮길때 객체가 파괴되지 않으면 원래 씬으로 복귀할때 새로운 객체가 만들어진다.
    //    // 싱글톤에서 이 경우 Destroy(gameObject);로 복사가 되는 불상사를 막을 수 있다.
    //}

    public GameObject Prefab;

    protected Queue<GameObject> memoryPool;

    
    protected virtual void Awake()
    {
        MakeSingleTone();
        memoryPool = new Queue<GameObject>();
    }


    public virtual void InitializePool(int size = 10)
    {
        memoryPool.Clear();
        for (int i = 0; i < size; i++)
            CreateNewObject();
    }

    protected virtual void CreateNewObject()
    {
        GameObject obj = Instantiate(Prefab);
        if (obj != null)
        {
            obj.SetActive(false);

            if (memoryPool != null)
            { 
                memoryPool.Enqueue(obj); 
            }
            else
            {
                Debug.LogError("메모리풀(Queue) 초기화 안됐음");
            }
        }
        else
        {
            Debug.LogError("프리팹이 없음");
        }
        
    }

    public virtual GameObject GetObject(Vector3 position)
    {
        if (memoryPool != null)
        {
            if (memoryPool.Count == 0)
            {
                CreateNewObject();
            }

            GameObject obj = memoryPool.Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            return obj;
        }
        else
        {
            Debug.LogError("memoryPool == null");
            return null;
        }
    }


    public virtual void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
    }
}

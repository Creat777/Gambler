using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class MemoryPool : Singleton<MemoryPool>
{
    public GameObject Prefab;

    protected Queue<GameObject> memoryPool;


    protected virtual void Awake()
    {
        base.Awake();
        memoryPool = new Queue<GameObject>();
    }


    public virtual void InitializePool(int size = 10)
    {
        // 객체를 생성할 때 DontDestroyOnLoad(obj); 를 사용했으니 클리어할 필요 없음 -> 메모리 단편화 방지됨
        //memoryPool.Clear();
        for (int i = 0; i < size; i++)
            CreateNewObject();
    }

    protected virtual void CreateNewObject()
    {
        GameObject obj = Instantiate(Prefab);
        if (obj != null)
        {
            // 메모리풀에 속하는 객체는 직접적인 삭제가 있지 않는 이상 보존함
            DontDestroyOnLoad(obj);
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

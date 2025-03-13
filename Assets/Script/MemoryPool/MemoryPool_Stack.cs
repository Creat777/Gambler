using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class MemoryPool_Stack : Singleton<MemoryPool_Stack>
{
    public GameObject prefab;

    protected Stack<GameObject> memoryPool;


    protected override void Awake()
    {
        base.Awake();
    }


    public virtual void InitializePool(int size)
    {
        if (memoryPool == null) memoryPool = new Stack<GameObject>();
        else memoryPool.Clear();

        for (int i = 0; i < size; i++)
            CreateNewObject(i);
    }

    protected virtual void CreateNewObject(int orderInPool)
    {
        GameObject obj = Instantiate(prefab);
        if (obj != null)
        {
            // 메모리풀의 대상을 메모리풀 안에서 관리함
            // 메모리풀이 donDestroy이면 객체도 donDestroy
            obj.transform.SetParent(transform, false);
            obj.transform.SetSiblingIndex(0);

            obj.SetActive(false);

            if (memoryPool != null)
            {
                memoryPool.Push(obj);
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
                CreateNewObject(transform.childCount);
            }

            GameObject obj = memoryPool.Pop();
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
        memoryPool.Push(obj);
    }
}

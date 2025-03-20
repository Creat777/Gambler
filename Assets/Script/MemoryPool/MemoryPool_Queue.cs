using System.Collections.Generic;
using UnityEngine;

public abstract class MemoryPool_Queue<T_Class> : Singleton<T_Class>
    where T_Class : MonoBehaviour
{
    public GameObject prefab;

    protected Queue<GameObject> memoryPool;


    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Start에서 호출할 것
    /// </summary>
    /// <param name="size"></param>
    public virtual void InitializePool(int size)
    {
        if (memoryPool == null) memoryPool = new Queue<GameObject>();
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

    /// <summary>
    /// 위치를 지정한 객체의 활성화
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual GameObject GetObject(Vector3 position)
    {
        if (memoryPool != null)
        {
            if (memoryPool.Count == 0)
            {
                CreateNewObject(transform.childCount);
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

    /// <summary>
    /// 위치를 지정하지 않는 객체의 활성화
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual GameObject GetObject()
    {
        if (memoryPool != null)
        {
            if (memoryPool.Count == 0)
            {
                CreateNewObject(transform.childCount);
            }

            GameObject obj = memoryPool.Dequeue();
            obj.SetActive(true);
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

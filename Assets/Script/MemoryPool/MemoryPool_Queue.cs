using System.Collections.Generic;
using UnityEngine;

public abstract class MemoryPool_Queue<T_Class> : Singleton<T_Class>
    where T_Class : MonoBehaviour
{
    public GameObject prefab;

    // 비활성화된 목록의 관리
    private Queue<GameObject> _memoryPool;
    protected Queue<GameObject> memoryPool
    {
        get
        {
            if (_memoryPool == null) _memoryPool = new Queue<GameObject>();
            return _memoryPool;
        }
    }

    // 활성화된 목록의 관리
    private List<GameObject> _ActiveObjList;
    public List<GameObject> ActiveObjList 
    {
        get
        {
            if (_ActiveObjList == null) _ActiveObjList = new List<GameObject>();
            return _ActiveObjList;
        }
    }

    /// <summary>
    /// Start에서 호출할 것
    /// </summary>
    /// <param name="size"></param>
    public virtual void InitializePool(int size)
    {
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
        if (memoryPool.Count == 0)
        {
            CreateNewObject(transform.childCount);
        }

        GameObject obj = memoryPool.Dequeue();
        obj.SetActive(true);
        ActiveObjList.Add(obj);

        obj.transform.position = position;
        return obj;
    }

    /// <summary>
    /// 위치를 지정하지 않는 객체의 활성화
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual GameObject GetObject()
    {
        if (memoryPool.Count == 0)
        {
            CreateNewObject(transform.childCount);
        }

        GameObject obj = memoryPool.Dequeue();
        obj.SetActive(true);
        ActiveObjList.Add(obj);
        return obj;
    }

    public virtual void ReturnObject(GameObject obj)
    {
        Debug.Log($"반환되는 객체 : {obj.name}");
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
        ActiveObjList.Remove(obj);
    }

    public virtual void ReturnAllObject()
    {
        int activeObjCount = ActiveObjList.Count;
        Debug.Log($"반환시작, 반환될 객체의 개수 : {ActiveObjList.Count}");
        for (int i = 0; i < activeObjCount; i++)
        {
            ReturnObject(ActiveObjList[0]);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public abstract class MemoryPool_Queue<T_Class> : Singleton<T_Class>
    where T_Class : MonoBehaviour
{
    public GameObject prefab;

    // ��Ȱ��ȭ�� ����� ����
    private Queue<GameObject> _memoryPool;
    protected Queue<GameObject> memoryPool
    {
        get
        {
            if (_memoryPool == null) _memoryPool = new Queue<GameObject>();
            return _memoryPool;
        }
    }

    // Ȱ��ȭ�� ����� ����
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
    /// Start���� ȣ���� ��
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
            // �޸�Ǯ�� ����� �޸�Ǯ �ȿ��� ������
            // �޸�Ǯ�� donDestroy�̸� ��ü�� donDestroy
            obj.transform.SetParent(transform, false);

            obj.SetActive(false);

            if (memoryPool != null)
            {
                memoryPool.Enqueue(obj);
            }
            else
            {
                Debug.LogError("�޸�Ǯ(Queue) �ʱ�ȭ �ȵ���");
            }
        }
        else
        {
            Debug.LogError("�������� ����");
        }
    }

    /// <summary>
    /// ��ġ�� ������ ��ü�� Ȱ��ȭ
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
    /// ��ġ�� �������� �ʴ� ��ü�� Ȱ��ȭ
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
        Debug.Log($"��ȯ�Ǵ� ��ü : {obj.name}");
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
        ActiveObjList.Remove(obj);
    }

    public virtual void ReturnAllObject()
    {
        int activeObjCount = ActiveObjList.Count;
        Debug.Log($"��ȯ����, ��ȯ�� ��ü�� ���� : {ActiveObjList.Count}");
        for (int i = 0; i < activeObjCount; i++)
        {
            ReturnObject(ActiveObjList[0]);
        }
    }
}

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
    /// Start���� ȣ���� ��
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
            // �޸�Ǯ�� ����� �޸�Ǯ �ȿ��� ������
            // �޸�Ǯ�� donDestroy�̸� ��ü�� donDestroy
            obj.transform.SetParent(transform, false);
            obj.transform.SetSiblingIndex(0);

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
    /// ��ġ�� �������� �ʴ� ��ü�� Ȱ��ȭ
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

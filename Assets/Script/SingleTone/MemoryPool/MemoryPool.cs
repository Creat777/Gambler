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
        // ��ü�� ������ �� DontDestroyOnLoad(obj); �� ��������� Ŭ������ �ʿ� ���� -> �޸� ����ȭ ������
        //memoryPool.Clear();
        for (int i = 0; i < size; i++)
            CreateNewObject();
    }

    protected virtual void CreateNewObject()
    {
        GameObject obj = Instantiate(Prefab);
        if (obj != null)
        {
            // �޸�Ǯ�� ���ϴ� ��ü�� �������� ������ ���� �ʴ� �̻� ������
            DontDestroyOnLoad(obj);
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

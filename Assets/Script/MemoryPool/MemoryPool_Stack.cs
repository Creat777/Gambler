using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class MemoryPool_Stack<T_Class> : Singleton<T_Class>
    where T_Class : MonoBehaviour
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
            // �޸�Ǯ�� ����� �޸�Ǯ �ȿ��� ������
            // �޸�Ǯ�� donDestroy�̸� ��ü�� donDestroy
            obj.transform.SetParent(transform, false);
            obj.transform.SetSiblingIndex(0);

            obj.SetActive(false);

            if (memoryPool != null)
            {
                memoryPool.Push(obj);
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

using UnityEngine;

public class ChangeMemoryPool : MemoryPool_Queue<ChangeMemoryPool>
{
    private void Start()
    {
        InitializePool(5);
    }

    public override GameObject GetObject()
    {
        if (memoryPool.Count == 0)
        {
            CreateNewObject(transform.childCount);
        }

        GameObject obj = memoryPool.Dequeue();
        obj.SetActive(true);
        ActiveObjList.Add(obj);

        // 생성 위치를 고정시킴
        obj.transform.position = transform.position;
        return obj;
    }
}

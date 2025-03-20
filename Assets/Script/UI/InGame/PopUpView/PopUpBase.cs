using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopUpBase<T_Class> : MemoryPool_Queue<T_Class>
    where T_Class : MonoBehaviour
{
    // ������ ����
    public RectTransform contentTrans;
    public GridLayoutGroup contentGrid;
    public ScrollRect scrollRect;  // ScrollRect ������Ʈ�� ����

    // ��ũ��Ʈ ����
    public List<GameObject> ActiveObjList { get; protected set; } // Ȱ��ȭ�� ����� �ϰ�����
    

    protected override void Awake()
    {
        base.Awake();
        if (ActiveObjList == null) ActiveObjList = new List<GameObject>();
    }
    private void OnEnable()
    {
        ChangeContentRectTransform();
    }

    protected virtual void InitContentAnchor()
    {
        contentTrans.anchorMin = new Vector2(0.5f, 0.5f);
        contentTrans.anchorMax = new Vector2(0.5f, 0.5f);
    }

    protected void ChangeContentRectTransform()
    {
        InitContentAnchor();

        if (contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            //Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            //Debug.Log($"ColumnCount : {ColumnCount}");

            // ���� �¿� ���� �� �� ���� ���� x�� ũ�� * ���� ����, �� ������ ������ ����
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * (ColumnCount - 1);
            //Debug.Log($"size.x : {size.x}");

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * rowCount +
                contentGrid.spacing.y * (rowCount - 1);
            //Debug.Log($"size.y : {size.y}");

            contentTrans.sizeDelta = size;

            ScrollToTop();

        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }

    }

    // ��ũ���� ���� �ø��� �Լ�
    public void ScrollToTop()
    {
        // ��ũ�ѹٸ� ���� ����Ʈ�� ����
        //scrollRect.verticalScrollbar.value = 1;

        // ����Ʈ�� �����Ͽ� ��ũ�ѹٸ� ����
        scrollRect.verticalNormalizedPosition = 1f;  // 1f: �� ��
    }

    protected override void CreateNewObject(int orderInPool)
    {
        GameObject obj = Instantiate(prefab);

        if (obj != null)
        {
            // ������ ��ü�� popUp�� content�� �ְ� ����
            obj.transform.SetParent(contentTrans.transform, false);

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

    public override GameObject GetObject()
    {
        if (memoryPool != null)
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
        else
        {
            Debug.LogError("memoryPool == null");
            return null;
        }
    }


    public override void ReturnObject(GameObject obj)
    {
        Debug.Log($"��ȯ�Ǵ� ��ü : {obj.name}");
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
        ActiveObjList.Remove(obj);
    }
}

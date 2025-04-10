using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �˾�â ������ ���� �߻�Ŭ����, MakeSingleTone�� ���� ȣ���ؾ���
/// </summary>
/// <typeparam name="T_Class">��ӹ޴� Ŭ����</typeparam>
public abstract class PopUpBase<T_Class> : MemoryPool_Queue<T_Class>
    where T_Class : MonoBehaviour
{
    // ������ ����
    public RectTransform contentTrans;
    public GridLayoutGroup contentGrid;
    public ScrollRect scrollRect;  // ScrollRect ������Ʈ�� ����

    protected override void Awake()
    {
        Debug.LogWarning("PopUp�� Awake���� �̱��� ���� ����");
    }



    private void InitAnchor()
    {
        contentTrans.anchorMin = new Vector2(0.5f, 1f);
        contentTrans.anchorMax = new Vector2(0.5f, 1f);
    }

    protected virtual void ChangeContentRectTransform()
    {
        InitAnchor();
        //InitGridRayout();

        if (contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = ((ActiveObjList.Count - 1) / (contentGrid.constraintCount)) + 1;
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

            //Debug.Log("CONTENT ���㼳�� �Ϸ�");

        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }

    }

    // ��ũ���� ���� �ø��� �Լ�
    private void ScrollToTop()
    {
        // ��ũ�ѹٸ� ���� ����Ʈ�� ����
        //scrollRect.verticalScrollbar.value = 1;

        // ����Ʈ�� �����Ͽ� ��ũ�ѹٸ� ����
        scrollRect.verticalNormalizedPosition = 1f;  // 1f: �� ��
    }

    public virtual void RefreshPopUp()
    {
        // ���̵�����
        int objCount = 1;
        RefreshPopUp(objCount,
            () =>
        {

        });
    }
    public virtual void RefreshPopUp(int totalCount, Action InitElementCallback)
    {
        // �ʿ��� ��ü�� ����
        int NeededCount = totalCount - ActiveObjList.Count;

        // ��ü�� �� �ʿ��� ��� �޸�Ǯ���� ����
        if (NeededCount > 0)
        {
            Debug.Log($"��ü {NeededCount}�� Ȱ��ȭ");
            for (int i = 0; i < NeededCount; i++)
            {
                GetObject();
            }
        }
        // �ʿ���� ��ŭ ȯ����
        else if (NeededCount < 0)
        {
            NeededCount = (-NeededCount);

            Debug.Log($"��ü {NeededCount}�� ��Ȱ��ȭ");
            for (int i = 0; i < NeededCount; i++)
            {
                ReturnObject(ActiveObjList[0]);
            }
        }

        // �׸񰳼��� �°� ����� ����
        ChangeContentRectTransform();

        // ���� Ȱ��ȭ�� ��ü�� ������ �ʱ�ȭ
        InitElementCallback();
    }
    

    protected override void CreateNewObject(int orderInPool)
    {
        GameObject obj = Instantiate(prefab);

        if (obj != null)
        {
            // �޸�Ǯ�� ����� �޸�Ǯ �ȿ��� ������
            // �޸�Ǯ�� donDestroy�̸� ��ü�� donDestroy
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
    public override GameObject GetObject(Vector3 position)
    {
        if (memoryPool.Count == 0)
        {
            CreateNewObject(contentTrans.transform.childCount);
        }

        GameObject obj = memoryPool.Dequeue();
        obj.SetActive(true);
        ActiveObjList.Add(obj);

        obj.transform.position = position;
        return obj;
    }
    public override GameObject GetObject()
    {
        if (memoryPool.Count == 0)
        {
            CreateNewObject(contentTrans.transform.childCount);
        }

        GameObject obj = memoryPool.Dequeue();
        obj.SetActive(true);
        ActiveObjList.Add(obj);
        return obj;
    }

    public override void ReturnObject(GameObject obj)
    {
        if(ActiveObjList.Contains(obj))
        {
            Debug.Log($"��ȯ�Ǵ� ��ü : {obj.name}");
            obj.SetActive(false);
            memoryPool.Enqueue(obj);
            ActiveObjList.Remove(obj);
            ChangeContentRectTransform(); // ��ü�� ȸ�������� �׸�ŭ ��ũ�� ������ ����
        }
        else
        {
            Debug.LogAssertion("�߸��� ��ȯ");
        }
    }

    public override void ReturnAllObject()
    {
        int activeObjCount = ActiveObjList.Count;
        Debug.Log($"��ȯ����, ��ȯ�� ��ü�� ���� : {ActiveObjList.Count}");
        for (int i = 0; i < activeObjCount; i++)
        {
            ReturnObject(ActiveObjList[0]);
        }
        ChangeContentRectTransform();
    }
}

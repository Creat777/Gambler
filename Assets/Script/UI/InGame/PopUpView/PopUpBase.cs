using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopUpBase<T_Class> : MemoryPool_Queue<T_Class>
    where T_Class : MonoBehaviour
{
    // 에디터 연결
    public RectTransform contentTrans;
    public GridLayoutGroup contentGrid;
    public ScrollRect scrollRect;  // ScrollRect 컴포넌트를 연결

    // 스크립트 편집
    public List<GameObject> ActiveObjList { get; protected set; } // 활성화된 목록의 일괄관리
    

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

            // 행개수 : ex) (자식개수가 4, 제약개수가 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            //Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            //Debug.Log($"ColumnCount : {ColumnCount}");

            // 셀의 좌우 여백 및 한 행의 셀의 x축 크기 * 셀의 개수, 셀 사이의 여백을 더함
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

    // 스크롤을 위로 올리는 함수
    public void ScrollToTop()
    {
        // 스크롤바를 통해 콘텐트를 제어
        //scrollRect.verticalScrollbar.value = 1;

        // 콘텐트를 제어하여 스크롤바를 제어
        scrollRect.verticalNormalizedPosition = 1f;  // 1f: 맨 위
    }

    protected override void CreateNewObject(int orderInPool)
    {
        GameObject obj = Instantiate(prefab);

        if (obj != null)
        {
            // 생성된 객체를 popUp의 content에 넣고 관리
            obj.transform.SetParent(contentTrans.transform, false);

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
        Debug.Log($"반환되는 객체 : {obj.name}");
        obj.SetActive(false);
        memoryPool.Enqueue(obj);
        ActiveObjList.Remove(obj);
    }
}

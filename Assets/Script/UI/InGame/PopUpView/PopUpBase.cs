using UnityEngine;
using UnityEngine.UI;

public class PopUpBase : MonoBehaviour
{
    // 에디터 연결
    public RectTransform contentTrans;
    public ScrollRect scrollRect;  // ScrollRect 컴포넌트를 연결


    // 스크립트 편집
    private GridLayoutGroup contentGrid;

    private void Awake()
    {
        contentGrid = contentTrans.gameObject.GetComponent<GridLayoutGroup>();
        contentTrans.anchorMin = new Vector2(0.5f, 0.5f);
        contentTrans.anchorMax = new Vector2(0.5f, 0.5f);
        ChangeContentRectTransform();
    }

    protected void ChangeContentRectTransform()
    {
        if (contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // 행개수 : ex) (자식개수가 4, 제약개수가 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            Debug.Log($"ColumnCount : {ColumnCount}");

            // 셀의 좌우 여백 및 한 행의 셀의 x축 크기 * 셀의 개수, 셀 사이의 여백을 더함
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * (ColumnCount - 1);
            Debug.Log($"size.x : {size.x}");

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * rowCount +
                contentGrid.spacing.y * (rowCount - 1);
            Debug.Log($"size.y : {size.y}");

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
}

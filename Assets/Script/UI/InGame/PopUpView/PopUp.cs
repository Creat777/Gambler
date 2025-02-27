using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    // 에디터 연결
    public RectTransform contentTrans;


    // 스크립트 편집
    private GridLayoutGroup contentGrid;

    private void Awake()
    {
        contentGrid = contentTrans.gameObject.GetComponent<GridLayoutGroup>();
    }

    protected void ChangeContentRectTransform()
    {
        if(contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // 행개수 : ex) (자식개수가 4, 제약개수가 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            int ColumnCount = contentGrid.constraintCount;

            // 셀의 좌우 여백 및 한 행의 셀의 x축 크기 * 셀의 개수, 셀 사이의 여백을 더함
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * ColumnCount - 1;

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * +rowCount +
                contentGrid.spacing.y * rowCount - 1;

            contentTrans.sizeDelta = size;
        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }
        
    }
}

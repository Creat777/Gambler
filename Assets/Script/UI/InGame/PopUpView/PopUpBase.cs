using UnityEngine;
using UnityEngine.UI;

public class PopUpBase : MonoBehaviour
{
    // ������ ����
    public RectTransform contentTrans;
    public ScrollRect scrollRect;  // ScrollRect ������Ʈ�� ����


    // ��ũ��Ʈ ����
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

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            Debug.Log($"ColumnCount : {ColumnCount}");

            // ���� �¿� ���� �� �� ���� ���� x�� ũ�� * ���� ����, �� ������ ������ ����
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

    // ��ũ���� ���� �ø��� �Լ�
    public void ScrollToTop()
    {
        // ��ũ�ѹٸ� ���� ����Ʈ�� ����
        //scrollRect.verticalScrollbar.value = 1;

        // ����Ʈ�� �����Ͽ� ��ũ�ѹٸ� ����
        scrollRect.verticalNormalizedPosition = 1f;  // 1f: �� ��
    }
}

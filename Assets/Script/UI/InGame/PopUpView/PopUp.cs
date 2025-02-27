using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    // ������ ����
    public RectTransform contentTrans;


    // ��ũ��Ʈ ����
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

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = (contentTrans.childCount - 1 / contentGrid.constraintCount) + 1;
            int ColumnCount = contentGrid.constraintCount;

            // ���� �¿� ���� �� �� ���� ���� x�� ũ�� * ���� ����, �� ������ ������ ����
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

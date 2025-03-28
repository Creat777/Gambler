using PublicSet;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class PortraitImage : MonoBehaviour
{
    [SerializeField] private RectTransform rectTrans;
    [SerializeField] private Image image;

    private void Awake()
    {
        MakeImageBoxSquare();
    }

    public void MakeImageBoxSquare()
    {
        Vector2 rectSize = rectTrans.rect.size;
        rectSize.x = rectSize.y; // y���� ��Ŀ�� ������ ���̰� �� ���� x�� ���� �����Ͽ� ���簢���� ����
        rectSize.y = rectTrans.sizeDelta.y; // sizeDelta�� ��Ŀ�� ��ġ�� ���� ������� ���̹Ƿ� strech�� ����� y���� ������ ���� ���
        rectTrans.sizeDelta = rectSize;
    }

    public bool TryChangePortraitImage(eCharacterType characterIndex, int iconIndex)
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitImageResource.Instance.TryGetImage(characterIndex, iconIndex, out isSueccessed);
        if(isSueccessed)
        {
            image.sprite = sprite;
            //Debug.Log($"�̹��� ��ȯ ����, ���� �̹��� : {sprite.name}");
        }
        else
        {
            Debug.LogAssertion("�̹��� ��ȯ ����");
        }

        return isSueccessed;
    }
}

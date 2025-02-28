using UnityEngine;
using UnityEngine.UI;

public class RawImageButtonGenerator : MonoBehaviour
{
    public RawImage rawImage;         // RawImage ��ü
    public Canvas canvas;             // UI Canvas
    public GameObject buttonPrefab;   // ��ư ������

    void Start()
    {
        // ����: ī��(��ü)�� ��ġ�� ũ�� ����
        RectTransform[] cardTransforms = GetCardTransforms(); // ī��� ��ġ ���� (�̰� �����Դϴ�.)

        // �� ī�忡 ���� ��ư�� ����
        foreach (var cardTransform in cardTransforms)
        {
            CreateButtonForCard(cardTransform);
        }
    }

    // ī�� ��ġ ���� (���⼭�� �ӽ÷� ī�� ������ ��ȯ)
    RectTransform[] GetCardTransforms()
    {
        // ����: �� ī���� ��ġ�� ũ�� ���� (�� �����͸� ������ �޾ƿ;� �մϴ�.)
        RectTransform[] cards = new RectTransform[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject cardObject = new GameObject("Card" + i); // ī�� ������Ʈ ����
            RectTransform rectTransform = cardObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 150); // ���� ũ��
            rectTransform.localPosition = new Vector3(100 * i, 0, 0); // ���� ��ġ
            cards[i] = rectTransform;
        }
        return cards;
    }

    // �� ī�忡 �´� ��ư�� ����
    void CreateButtonForCard(RectTransform cardTransform)
    {
        // ��ư �������� Canvas�� �߰�
        GameObject buttonObject = Instantiate(buttonPrefab, canvas.transform);

        // ��ư�� RectTransform�� �����ͼ� ũ��� ��ġ�� ����
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = cardTransform.sizeDelta; // ī�� ũ�⿡ ���߱�
        buttonRect.localPosition = cardTransform.localPosition; // ī�� ��ġ�� ���߱�

        // ��ư Ŭ�� �� �̺�Ʈ ���
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(cardTransform));
    }

    // ��ư Ŭ�� �� ȣ��� �̺�Ʈ
    void OnButtonClick(RectTransform cardTransform)
    {
        Debug.Log("Button clicked for card at position: " + cardTransform.localPosition);
        // ���⼭ ��ư Ŭ�� �� �� ������ �߰�
    }
}

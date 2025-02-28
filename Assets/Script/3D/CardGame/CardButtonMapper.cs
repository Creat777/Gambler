using UnityEngine;
using UnityEngine.UI;

public class RawImageButtonGenerator : MonoBehaviour
{
    public RawImage rawImage;         // RawImage 객체
    public Canvas canvas;             // UI Canvas
    public GameObject buttonPrefab;   // 버튼 프리팹

    void Start()
    {
        // 예시: 카드(객체)의 위치와 크기 정보
        RectTransform[] cardTransforms = GetCardTransforms(); // 카드들 위치 정보 (이건 예시입니다.)

        // 각 카드에 대해 버튼을 생성
        foreach (var cardTransform in cardTransforms)
        {
            CreateButtonForCard(cardTransform);
        }
    }

    // 카드 위치 정보 (여기서는 임시로 카드 정보를 반환)
    RectTransform[] GetCardTransforms()
    {
        // 예시: 각 카드의 위치와 크기 정보 (이 데이터를 실제로 받아와야 합니다.)
        RectTransform[] cards = new RectTransform[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject cardObject = new GameObject("Card" + i); // 카드 오브젝트 생성
            RectTransform rectTransform = cardObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 150); // 예시 크기
            rectTransform.localPosition = new Vector3(100 * i, 0, 0); // 예시 위치
            cards[i] = rectTransform;
        }
        return cards;
    }

    // 각 카드에 맞는 버튼을 생성
    void CreateButtonForCard(RectTransform cardTransform)
    {
        // 버튼 프리팹을 Canvas에 추가
        GameObject buttonObject = Instantiate(buttonPrefab, canvas.transform);

        // 버튼의 RectTransform을 가져와서 크기와 위치를 설정
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = cardTransform.sizeDelta; // 카드 크기에 맞추기
        buttonRect.localPosition = cardTransform.localPosition; // 카드 위치에 맞추기

        // 버튼 클릭 시 이벤트 등록
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(cardTransform));
    }

    // 버튼 클릭 시 호출될 이벤트
    void OnButtonClick(RectTransform cardTransform)
    {
        Debug.Log("Button clicked for card at position: " + cardTransform.localPosition);
        // 여기서 버튼 클릭 시 할 동작을 추가
    }
}

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
        rectSize.x = rectSize.y; // y값은 앵커로 고정된 값이고 이 값을 x축 값에 적용하여 정사각형을 만듬
        rectSize.y = rectTrans.sizeDelta.y; // sizeDelta는 앵커의 위치에 따른 상대적인 값이므로 strech가 적용된 y축은 기존의 값을 사용
        rectTrans.sizeDelta = rectSize;
    }

    public bool TryChangePortraitImage(eCharacter characterIndex)
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitResource.Instance.TryGetPortraitImage(characterIndex, out isSueccessed);
        if(isSueccessed)
        {
            image.sprite = sprite;
            Debug.Log("이미지 전환 성공");
        }
        else
        {
            Debug.LogAssertion("이미지 전환 실패");
        }

        return isSueccessed;
    }
}

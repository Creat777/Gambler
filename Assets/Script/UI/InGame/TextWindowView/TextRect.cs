using UnityEngine;

public class TextRect : MonoBehaviour
{
    public RectTransform rectTransform;

    public RectTransform TextWindowRectTrans;
    public RectTransform PortraitFrameRectTrans;
    public RectTransform SkipButtonRectTrans;


    private void Start()
    {
        MakeRectFit();
    }
    
    /// <summary>
    /// PortraitBox와 skip 박스 사이에 딱 맞도록 조정,
    /// 반드시 Awake 다음 시간에 실행
    /// </summary>
    private void MakeRectFit()
    {
        Vector2 size = rectTransform.sizeDelta;

        float spaceBetweenPortraitAndTextWindow = Mathf.Abs(PortraitFrameRectTrans.anchoredPosition.x);
        float spaceBetweenPortraitAndThis = Mathf.Abs(rectTransform.anchoredPosition.x);
        float spaceBetweenSkipButtonAndTextWindow = Mathf.Abs(PortraitFrameRectTrans.anchoredPosition.x);

        Debug.Log(spaceBetweenPortraitAndTextWindow);
        Debug.Log(spaceBetweenPortraitAndThis);
        Debug.Log(spaceBetweenSkipButtonAndTextWindow);

        // y축은 고정됐으니 x축만 변경
        size.x = TextWindowRectTrans.rect.size.x // 전체 상자 크기
                - spaceBetweenPortraitAndTextWindow // 좌쪽 여백
                - PortraitFrameRectTrans.rect.size.x // portrait 상자 x축 크기
                - spaceBetweenPortraitAndThis // portrait과 textWindow사이 간격
                - spaceBetweenSkipButtonAndTextWindow // 우측 여백
                - SkipButtonRectTrans.rect.size.x // skipBox의 x축 크기
                - spaceBetweenSkipButtonAndTextWindow; // skipBox과 textWindow사이 간격

        rectTransform.sizeDelta = size;
    }
}

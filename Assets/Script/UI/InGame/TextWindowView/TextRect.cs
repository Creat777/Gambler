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
    /// PortraitBox�� skip �ڽ� ���̿� �� �µ��� ����,
    /// �ݵ�� Awake ���� �ð��� ����
    /// </summary>
    private void MakeRectFit()
    {
        Vector2 size = rectTransform.sizeDelta;

        float spaceBetweenPortraitAndTextWindow = Mathf.Abs(PortraitFrameRectTrans.anchoredPosition.x);
        float spaceBetweenPortraitAndThis = Mathf.Abs(rectTransform.anchoredPosition.x);
        float spaceBetweenSkipButtonAndTextWindow = Mathf.Abs(PortraitFrameRectTrans.anchoredPosition.x);

        //Debug.Log(spaceBetweenPortraitAndTextWindow);
        //Debug.Log(spaceBetweenPortraitAndThis);
        //Debug.Log(spaceBetweenSkipButtonAndTextWindow);

        // y���� ���������� x�ุ ����
        size.x = TextWindowRectTrans.rect.size.x // ��ü ���� ũ��
                - spaceBetweenPortraitAndTextWindow // ���� ����
                - PortraitFrameRectTrans.rect.size.x // portrait ���� x�� ũ��
                - spaceBetweenPortraitAndThis // portrait�� textWindow���� ����
                - spaceBetweenSkipButtonAndTextWindow // ���� ����
                - SkipButtonRectTrans.rect.size.x // skipBox�� x�� ũ��
                - spaceBetweenSkipButtonAndTextWindow; // skipBox�� textWindow���� ����

        rectTransform.sizeDelta = size;
    }
}

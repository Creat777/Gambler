using UnityEngine;
using UnityEngine.UI;

public class LoupeOnOffButton : Deactivatable_Button_Base
{
    public GameObject Loupe;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        LoupeOn();
    }

    public void LoupeOn()
    {
        if(Loupe == null)
        {
            Debug.LogAssertion("Loupe 연결 안됨");
            return;
        }

        // 버튼 전환
        Loupe.SetActive(true);
        image.color = Color.white;
        SetButtonCallback(LoupeOff);
    }

    public void LoupeOff()
    {
        if (Loupe == null)
        {
            Debug.LogAssertion("Loupe 연결 안됨");
            return;
        }

        Loupe.SetActive(false);
        image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        SetButtonCallback(LoupeOn);
    }


}

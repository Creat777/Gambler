using UnityEngine;
using UnityEngine.UI;

public class LoupeOnOffButton : ImageChange_ButtonBase
{
    public GameObject Loupe;


    private void Start()
    {
        LoupeOff();
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
        ChangeOn();
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
        ChangeOff();
        SetButtonCallback(LoupeOn);
    }


}

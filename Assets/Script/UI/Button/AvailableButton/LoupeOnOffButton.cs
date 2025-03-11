using UnityEngine;
using UnityEngine.UI;

public class LoupeOnOffButton : ImageOnOff_ButtonBase
{
    public GameObject Loupe;


    private void Start()
    {
        LoupeOn();
    }

    public void LoupeOn()
    {
        if(Loupe == null)
        {
            Debug.LogAssertion("Loupe ���� �ȵ�");
            return;
        }

        // ��ư ��ȯ
        Loupe.SetActive(true);
        ChangeOnColor();
        SetButtonCallback(LoupeOff);
    }

    public void LoupeOff()
    {
        if (Loupe == null)
        {
            Debug.LogAssertion("Loupe ���� �ȵ�");
            return;
        }

        Loupe.SetActive(false);
        ChangeOffColor();
        SetButtonCallback(LoupeOn);
    }


}

using UnityEngine;

public class GameAssistantButton : Deactivatable_ButtonBase
{
    public PopUpView PopUpView;

    private void Start()
    {
        if (PopUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(PopUpView.GameAssistantPopUpOpen_OnlyOneLives);
    }


}

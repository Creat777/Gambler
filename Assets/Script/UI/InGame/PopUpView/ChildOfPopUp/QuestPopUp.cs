using UnityEngine;
using PublicSet;

public class QuestPopUp : PopUp
{
    private void OnEnable()
    {
        //GameManager.Connector.iconView_Script.SetPopUpState(eIcon.Quest, ePopUpState.Open);
    }

    private void OnDisable()
    {
        //GameManager.Connector.iconView_Script.SetPopUpState(eIcon.Quest, ePopUpState.Close);
    }
}

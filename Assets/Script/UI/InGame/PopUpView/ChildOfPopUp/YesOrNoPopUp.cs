using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YesOrNoPopUp : SimplePopUpBase
{
    public Button YesButton;
    
    public void SetYesButtonCallBack(UnityAction callback )
    {
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener( callback );
    }
}

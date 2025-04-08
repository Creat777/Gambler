using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YesOrNoPopUp : SimplePopUpBase
{
    public Button YesButton;
    public Text yesText;
    public Text noText;
    
    public void SetYesButtonCallBack(UnityAction callback )
    {
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(callback);
        YesButton.onClick.AddListener(()=>gameObject.SetActive(false));
    }

    public void SetYesText(string text)
    {
        yesText.text = text;
    }

    public void SetNoText(string text)
    {
        noText.text = text;
    }
}

using UnityEngine;

public class Pause_Button : MonoBehaviour
{
    public GameObject PopUpView;
    public GameObject OptionPopUp;
    public void OptionPopUpOpen()
    {
        if(PopUpView == null)
        {
            PopUpView = GameObject.Find("PopUpView");
            OptionPopUp = GameObject.Find("OptionPopUp");
            if(OptionPopUp == null)
            {
                OptionPopUp = GameObject.Find("OptionPopUpScroll");
            }
        }

        PopUpView.SetActive(true);
        OptionPopUp.SetActive(true);
        GameManager.Instance.Continue_theGame();
    }

    public void OptionopUpClose()
    {
        PopUpView.SetActive(false);
        OptionPopUp.SetActive(false);
        GameManager.Instance.Continue_theGame();
    }
}

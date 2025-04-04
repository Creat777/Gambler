using UnityEngine;
public class PopUPView_Lobby : PopUpViewBase
{
    public ContinuePopUp continuePopUp;
    public override void MakePopUpSingleTone()
    {
        continuePopUp.MakeSingleTone();
    }

    public void ContinuePopUpOpen()
    {
        continuePopUp.gameObject.SetActive(true);
        continuePopUp.transform.SetAsLastSibling();
    }

    
}

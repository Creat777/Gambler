using UnityEngine;

public enum checkCase
{
    @default,
    QuestComplete
}
public class CheckPopUp : SimplePopUpBase
{
    public CaseQuest caseQuest;
    public void PopUpUpChange(checkCase checkCase)
    {
        switch(checkCase)
        {
            case checkCase.@default:
                caseQuest.gameObject.SetActive(false); 
                mainDescription.gameObject.SetActive(true);
                break;

            case checkCase.QuestComplete:
                caseQuest.gameObject.SetActive(true);
                mainDescription.gameObject.SetActive(false);
                break;
        }
    }
}

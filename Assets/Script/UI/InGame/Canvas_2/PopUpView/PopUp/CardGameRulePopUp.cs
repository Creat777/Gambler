using UnityEngine;

public class CardGameRulePopUp : PopUpBase<CardGameRulePopUp>
{
    public override void RefreshPopUp()
    {
        Debug.LogWarning("재정의 되지 않았음");
    }

    void Start()
    {
        ChangeContentRectTransform();
    }
}

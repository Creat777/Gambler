using UnityEngine;

public class CardGameRulePopUp : PopUpBase<CardGameRulePopUp>
{
    public override void RefreshPopUp()
    {
        Debug.LogWarning("������ ���� �ʾ���");
    }

    void Start()
    {
        ChangeContentRectTransform();
    }
}

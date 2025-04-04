using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
    public override void RefreshPopUp()
    {
        throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePool(1);
    }
    private void Start()
    {
        GameObject obj = GetObject();
        ChangeContentRectTransform();
    }
}

using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
    public override void RefreshPopUp()
    {
        Debug.LogWarning("재정의되지 않았음");
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

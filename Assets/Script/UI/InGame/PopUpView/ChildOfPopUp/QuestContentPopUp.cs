using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
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

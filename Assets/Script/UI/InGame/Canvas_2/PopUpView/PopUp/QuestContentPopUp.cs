using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
    public override void RefreshPopUp()
    {
        Debug.LogWarning("�����ǵ��� �ʾ���");
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

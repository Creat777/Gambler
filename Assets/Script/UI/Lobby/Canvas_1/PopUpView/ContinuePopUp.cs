using PublicSet;
using System;
using UnityEngine;

public class ContinuePopUp : PopUpBase<ContinuePopUp>
{
    protected override void Awake()
    {
        InitializePool(4);
    }

    protected override void OnEnable()
    {
        // RefreshPopUp���� ChangeContentRectTransform�� ȣ�������� base�� �Ϻη� �Ⱦ�
        RefreshPopUp();
    }
    public override void RefreshPopUp()
    {
        RefreshPopUp(4,
            () =>
            {
                int index = 0;
                foreach (ePlayerSaveKey saveKey in Enum.GetValues(typeof(ePlayerSaveKey)))
                {
                    if (saveKey == ePlayerSaveKey.None) continue;

                    SavedPlayerDataPanel panel = ActiveObjList[index++].GetComponent<SavedPlayerDataPanel>();
                    if (panel != null)
                    {
                        panel.SetPanel(saveKey);
                    }
                    else
                    {
                        Debug.LogAssertion($"{gameObject.name}�� ��ũ��Ʈ Ȯ�� �ٶ�");
                    }

                }
            });
    }
}
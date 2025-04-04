using PublicSet;
using System;
using UnityEngine;

public class SaveDataPopUp : PopUpBase<SavedPlayerDataPanel>
{
    protected override void Awake()
    {
        InitializePool(4);
    }
    private void Start()
    {
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
                        Debug.LogAssertion($"{gameObject.name}의 스크립트 확인 바람");
                    }

                }
            });
    }
}
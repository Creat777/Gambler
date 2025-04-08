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
        // RefreshPopUp에서 ChangeContentRectTransform를 호출했으니 base는 일부러 안씀
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
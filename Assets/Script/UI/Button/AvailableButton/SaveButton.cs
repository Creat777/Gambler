

public class SaveButton : SaveAndContinue_ButtonBase
{
    public override void Callback()
    {
        string savedDate = PlayerPrefsManager.Instance.LoadSavedDate(saveKey);
        if (savedDate != string.Empty) // 이미 저장된 데이터가 있는 경우
        {
            popUpView.YesOrNoPopUpOpen();
            yesOrNoPopUp.UpdateMainDescription("이미 저장된 데이터가 있습니다.\n데이터를 새롭게 등록하시겠습니까?");
            yesOrNoPopUp.SetYesText("예");
            yesOrNoPopUp.SetYesButtonCallBack(
                () =>
                {
                    SaveDataProcess();
                });
        }
        else //저장된 데이터가 없는 경우
        {
            SaveDataProcess();
        }
        (popUpView as PopUpView_InGame).saveDataPopUp.RefreshPopUp();
    }

    private void SaveDataProcess()
    {
        GameManager.Instance.SetPlayerSaveKey(saveKey);
        PlayerPrefsManager.Instance.SaveTotalData();

        popUpView.CheckPopUpOpen();
        checkPopUp.UpdateMainDescription("플레이어 정보가 저장되었습니다.");
        GameManager.connector_InGame.popUpView_Script.saveDataPopUp.RefreshPopUp();
    }
}

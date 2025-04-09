

public class SaveButton : SaveAndContinue_ButtonBase
{
    public override void Callback()
    {
        string savedDate = PlayerSaveManager.Instance.LoadSavedDate(saveKey);
        if (savedDate != string.Empty) // �̹� ����� �����Ͱ� �ִ� ���
        {
            popUpView.YesOrNoPopUpOpen();
            yesOrNoPopUp.UpdateMainDescription("�̹� ����� �����Ͱ� �ֽ��ϴ�.\n�����͸� ���Ӱ� ����Ͻðڽ��ϱ�?");
            yesOrNoPopUp.SetYesText("��");
            yesOrNoPopUp.SetYesButtonCallBack(
                () =>
                {
                    SaveDataProcess();
                });
        }
        else //����� �����Ͱ� ���� ���
        {
            SaveDataProcess();
        }
        (popUpView as PopUpView_InGame).saveDataPopUp.RefreshPopUp();
    }

    private void SaveDataProcess()
    {
        GameManager.Instance.SetPlayerSaveKey(saveKey);
        PlayerSaveManager.Instance.SaveTotalData();

        popUpView.CheckPopUpOpen();
        checkPopUp.UpdateMainDescription("�÷��̾� ������ ����Ǿ����ϴ�.");
        GameManager.connector_InGame.popUpView_Script.saveDataPopUp.RefreshPopUp();
    }
}

using UnityEngine.SceneManagement;

public class ContinueButton : SaveAndContinue_ButtonBase
{

    //float delay = 2.0f;
    public override void Callback()
    {
        //float delay = 2.0f;

        string savedDate =  PlayerSaveManager.Instance.LoadSavedDate(saveKey);
        if(savedDate != string.Empty)
        {
            popUpView.YesOrNoPopUpOpen();

            yesOrNoPopUp.UpdateMainDescription($"{saveKey.ToString()}\n�ش� �÷��̸� ����Ͻðڽ��ϱ�?");

            yesOrNoPopUp.SetYesButtonCallBack(
                () =>
                {
                    (popUpView as PopUPView_Lobby).gameObject.SetActive(false);
                    GameManager.Instance.SetPlayerSaveKey(saveKey);
                    GameManager.Instance.SceneUnloadView(() => SceneManager.LoadScene("InGame"));
                });
        }
        else
        {
            popUpView.CheckPopUpOpen();

            checkPopUp.UpdateMainDescription("���� ������ �����ϴ�!");
        }
    }
}

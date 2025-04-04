using UnityEngine.SceneManagement;

public class ContinueButton : SaveAndContinue_ButtonBase
{

    float delay = 2.0f;
    public override void Callback()
    {
        float delay = 2.0f;

        string savedDate =  PlayerPrefsManager.Instance.LoadSavedDate(saveKey);
        if(savedDate != string.Empty)
        {
            popUpView.YesOrNoPopUpOpen();

            yesOrNoPopUp.UpdateMainDescription($"{saveKey.ToString()}\n해당 플레이를 계속하시겠습니까?");

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

            checkPopUp.UpdateMainDescription("저장 정보가 없습니다!");
        }
    }
}

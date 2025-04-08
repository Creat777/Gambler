using PublicSet;
using UnityEngine;

public abstract class SaveAndContinue_ButtonBase : ButtonBase
{
    protected PopUpViewBase popUpView
    {
        get
        {
            switch (GameManager.Instance.currentScene)
            {
                case eScene.Lobby: return GameManager.connector_Lobby.popUpView_Script;
                case eScene.InGame: return GameManager.connector_InGame.popUpView_Script;
            }
            Debug.LogAssertion("잘못된 접근");
            return null;
        }
    }
    protected YesOrNoPopUp yesOrNoPopUp
    {
        get
        {
            return popUpView.yesOrNoPopUp;
        }
    }
    protected CheckPopUp checkPopUp
    {
        get
        {
            return popUpView.checkPopUp;
        }
    }
    protected ePlayerSaveKey saveKey = ePlayerSaveKey.None;

    protected virtual void Start()
    {
        SetButtonCallback(Callback);
    }

    public virtual void SetPlayerSaveKey(ePlayerSaveKey value)
    {
        saveKey = value;
    }

    public abstract void Callback();
}

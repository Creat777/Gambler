using PublicSet;
using UnityEngine;

public abstract class GameEnterButtonBase : ButtonBase
{
    private void Start()
    {
        InitButtonCallback();
    }
    public void InitButtonCallback()
    {
        SetButtonCallback(EnterGame);
    }
    public abstract void EnterGame();

    public void SetPlayerCantPlayThis()
    {
        SetButtonCallback(
            () => GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eTextScriptFile.PlayerCantPlayThis)
            );
    }
}

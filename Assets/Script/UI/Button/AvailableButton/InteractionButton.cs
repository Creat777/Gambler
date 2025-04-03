using UnityEngine;

public class InteractionButton : Deactivatable_ButtonBase
{
    private void Start()
    {
        SetButtonCallback(StartInteraction);
    }
    public void StartInteraction()
    {
        CallbackManager.Instance.TextWindowPopUp_Open();
        TextWindowView textView = (GameManager.connector as Connector_InGame).textWindowView.GetComponent<TextWindowView>();
        textView.StartTextWindow();
    }
}

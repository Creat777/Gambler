using UnityEngine;

public class OnlyOneLives : Deactivatable_ButtonBase
{
    public CardGameView CardGameView;
    public CardGamePlayManager cardGamePlayManager;

    // 버튼콜백
    public void StartOnleOneLives()
    {
        if(CardGameView == null)
        {
            Debug.LogAssertion("CardGameView == null");
            return;
        }
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("PlayManager == null");
            return;
        }

        CallbackManager.Instance.BlackViewProcess(2.0f,
            () => GameManager.Connector.MainCanvas_script.CloseAllOfView(),
            () => CardGameView.gameObject.SetActive(true)
            );
        
    }
}

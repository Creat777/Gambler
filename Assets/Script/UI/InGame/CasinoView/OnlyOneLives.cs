using UnityEngine;

public class OnlyOneLives : Deactivatable_ButtonBase
{
    public CardGameView CardGameView;
    public CardGamePlayManager cardGamePlayManager;

    // 버튼콜백
    public void StartOnlyOneLives()
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

        CallbackManager.Instance.PlaySequnce_BlackViewProcess(2.0f,
            () =>
            {
                GameManager.connector.MainCanvas_script.CloseAllOfView();
                CardGameView.gameObject.SetActive(true);
            }
            );
        
    }
}

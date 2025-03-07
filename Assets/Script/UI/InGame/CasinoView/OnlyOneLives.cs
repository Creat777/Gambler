using UnityEngine;

public class OnlyOneLives : MonoBehaviour
{
    public GameObject CardGameView;
    public CardGamePlayManager cardGamePlayManager;

    // 버튼콜백
    public void StartOnleOneLives()
    {
        if(CardGameView == null)
        {
            Debug.LogAssertion("CardGameView == null");
        }
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("PlayManager == null");
        }


        CallbackManager.Instance.BlackViewProcess(2.0f,
            () => GameManager.Connector.MainCanvas_script.CloseAllOfView(),
            () => CardGameView.SetActive(true)
            );
        
    }
}

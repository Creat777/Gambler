using PublicSet;
using UnityEngine;

public class CasinoView : MonoBehaviour
{
    public GameObject CardGameView;
    public void StartDealerDialogue()
    {
        TextWindowView textViewScript = GameManager.Connector.textWindowView.GetComponent<TextWindowView>();
        if (textViewScript != null)
        {
            //Debug.Log("딜러 텍스트 시작");
            textViewScript.gameObject.SetActive(true);
            //if (textViewScript.gameObject.activeSelf == true) Debug.Log("텍스트 박스 켜졌음");
            textViewScript.StartTextWindow(eTextScriptFile.CasinoDealer);
        }
    }

    public void StartOnleOneLives()
    {
        
        CallbackManager.Instance.BlackViewProcess(2.0f,
            () => GameManager.Connector.MainCanvas_script.CloseAllOfView(),
            () => CardGameView.SetActive(true)
            );
    }
}

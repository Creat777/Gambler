using PublicSet;
using UnityEngine;

public class CasinoView : MonoBehaviour
{
    [SerializeField] private OnlyOneLivesButton _onlyOneLivesButton;
    public OnlyOneLivesButton onlyOneLivesButton { get { return _onlyOneLivesButton; } }

    public void InitGameButtonCallback()
    {
        onlyOneLivesButton.InitButtonCallback();
    }



    public void StartDealerDialogue()
    {
        TextWindowView textViewScript = (GameManager.connector as Connector_InGame).textWindowView.GetComponent<TextWindowView>();
        if (textViewScript != null)
        {
            //Debug.Log("딜러 텍스트 시작");
            textViewScript.gameObject.SetActive(true);
            //if (textViewScript.gameObject.activeSelf == true) Debug.Log("텍스트 박스 켜졌음");
            textViewScript.StartTextWindow(eTextScriptFile.GameMaster);
        }
    }
}

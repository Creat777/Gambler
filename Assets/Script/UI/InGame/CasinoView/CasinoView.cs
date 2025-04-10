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
            //Debug.Log("���� �ؽ�Ʈ ����");
            textViewScript.gameObject.SetActive(true);
            //if (textViewScript.gameObject.activeSelf == true) Debug.Log("�ؽ�Ʈ �ڽ� ������");
            textViewScript.StartTextWindow(eTextScriptFile.GameMaster);
        }
    }
}

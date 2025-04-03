using PublicSet;
using UnityEngine;

public class CasinoView : MonoBehaviour
{
    
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

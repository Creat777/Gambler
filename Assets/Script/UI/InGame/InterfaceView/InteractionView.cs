using PublicSet;
using UnityEngine;


public class InteractionView : MonoBehaviour
{
    // ��ũ��Ʈ ����
    public bool isInteractiveOn;

    // �����Ϳ��� ����
    public GameObject InteractButton;

    private void Awake()
    {
        InteractButton_Off();
    }

    private void Start()
    {

    }

    public void InteractButton_Off()
    {
        InteractButton.SetActive(false);
        isInteractiveOn = false;
    }

    public void InteractButton_On()
    {
        InteractButton.SetActive(true);
        isInteractiveOn = true;
    }

    public void StartInteraction()
    {
        CallbackManager.Instance.TextWindowPopUp_Open();
        TextWindowView textView = GameManager.Connector.textWindowView.GetComponent<TextWindowView>();
        textView.StartTestWindow();
    }
}

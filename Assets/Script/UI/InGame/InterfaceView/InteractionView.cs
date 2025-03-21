using PublicSet;
using UnityEngine;


public class InteractionView : MonoBehaviour
{
    // 스크립트 편집
    public bool isInteractiveOn;

    // 에디터에서 연결
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
        TextWindowView textView = GameManager.connector.textWindowView.GetComponent<TextWindowView>();
        textView.StartTextWindow();
    }
}

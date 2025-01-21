using UnityEngine;

public class Canvas_UserInterface : MonoBehaviour
{
    public KeyBoardView keyBoardView;
    public OptionView optionView;
    bool isPopUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OptionViewClose();
    }

    // Update is called once per frame
    void Update()
    {
        PopUpOrganize();
    }

    public void PopUpOrganize()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPopUp == false)
            {
                OptionViewOpen();
            }
            else if(isPopUp == true)
            {
                OptionViewClose();
            }
        }
    }

    public void OptionViewOpen()
    {
        keyBoardView.gameObject.SetActive(false);
        optionView.gameObject.SetActive(true);
        GameManager.Instance.Pause_theGame();
        isPopUp = true;
    }

    public void OptionViewClose()
    {
        keyBoardView.gameObject.SetActive(true);
        optionView.gameObject.SetActive(false);
        GameManager.Instance.Unpause_theGame();
        isPopUp = false;
    }
}

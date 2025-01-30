using UnityEngine;

public class Canvas_UserInterface : MonoBehaviour
{
    public GameObject input_InterfaceView;
    public GameObject optionView;
    bool isOptionViewPopUp;

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
            if (isOptionViewPopUp == false)
            {
                OptionViewOpen();
            }
            else if(isOptionViewPopUp == true)
            {
                OptionViewClose();
            }
        }
    }

    public void OptionViewOpen()
    {
        if(input_InterfaceView != null)
        {
            input_InterfaceView.SetActive(false);
            optionView.SetActive(true);
            GameManager.Instance.Pause_theGame();
            isOptionViewPopUp = true;
        }
        
    }

    public void OptionViewClose()
    {
        if (input_InterfaceView != null)
        {
            input_InterfaceView.SetActive(true);
            optionView.SetActive(false);
            GameManager.Instance.Continue_theGame();
            isOptionViewPopUp = false;
        }
            
    }
}

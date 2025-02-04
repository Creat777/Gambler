using UnityEngine;

public class Canvas_InGame : MonoBehaviour
{
    public GameObject interfaceView;
    public GameObject textWindowView;
    public GameObject PopUPView;
    public GameObject BlackView;

    void Start()
    {
        interfaceView.SetActive(true);
        textWindowView.SetActive(false);
        PopUpViewDisable();
        BlackView.SetActive(false);
    }

    private void PopUpViewDisable()
    {
        foreach (Transform child in PopUPView.transform)
        {
            if(child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
        }
        PopUPView.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    PopUpOrganize();
    //}

    //public void PopUpOrganize()
    //{
    //    if(Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        if (isOptionViewPopUp == false)
    //        {
    //            OptionViewOpen();
    //        }
    //        else if(isOptionViewPopUp == true)
    //        {
    //            OptionViewClose();
    //        }
    //    }
    //}

    //public void OptionViewOpen()
    //{
    //    if(interfaceView != null)
    //    {
    //        interfaceView.SetActive(false);
    //        optionView.SetActive(true);
    //        GameManager.Instance.Pause_theGame();
    //        isOptionViewPopUp = true;
    //    }
    //}

    //public void OptionViewClose()
    //{
    //    if (interfaceView != null)
    //    {
    //        interfaceView.SetActive(true);
    //        optionView.SetActive(false);
    //        GameManager.Instance.Continue_theGame();
    //        isOptionViewPopUp = false;
    //    } 
    //}
}

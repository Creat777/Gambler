using UnityEngine;

public class Canvas_InGame : MonoBehaviour
{
    public GameObject interfaceView;
    public GameObject textWindowView;
    public GameObject PopUPView;

    void Start()
    {
        interfaceView.SetActive(true);
        textWindowView.SetActive(false);
        PopUpViewDisable();
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

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView : MonoBehaviour
{
    public GameObject optionPopUp;
    public GameObject inventoryPopUp;
    public GameObject yesOrNoPopUp;
    public GameObject checkPopUp;
    public GameObject questPopUp;

    

    void Start()
    {
        
    }

    

    public void OptionPopUpOpen()
    {
        gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    public void OptionPopUpClose()
    {
        optionPopUp.SetActive(false);
    }

    
    public void InventoryPopUpOpen()
    {
        gameObject.SetActive(true);
        inventoryPopUp.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    public void InventoryPopUpClose()
    {
        inventoryPopUp.SetActive(false);

    }

    public void YesOrNoPopUpOpen()
    {
        gameObject.SetActive(true);
        yesOrNoPopUp.SetActive(true);
        yesOrNoPopUp.transform.SetAsLastSibling();
    }
    public void YesOrNoPopUpClose()
    {
        yesOrNoPopUp.SetActive(false);

    }

    public void CheckPopUpOpen()
    {
        gameObject.SetActive(true);
        checkPopUp.SetActive(true);
        checkPopUp.transform.SetAsLastSibling();
    }
    public void CheckPopUpClose()
    {
        checkPopUp.SetActive(false);
    }


    public void QuestPopUpOpen()
    {
        gameObject.SetActive(true);
        questPopUp.SetActive(true);
        questPopUp.transform.SetAsLastSibling();

    }
    public void QuestPopUpClose()
    {
        questPopUp.SetActive(false);
    }
}

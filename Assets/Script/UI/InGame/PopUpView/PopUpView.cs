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
    public GameObject CardGameRulePopUp;

    

    void Start()
    {
        
    }

    
    // 버튼 콜백
    public void OptionPopUpOpen()
    {
        gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void InventoryPopUpOpen()
    {
        gameObject.SetActive(true);
        inventoryPopUp.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    // 버튼 콜백
    public void YesOrNoPopUpOpen()
    {
        gameObject.SetActive(true);
        yesOrNoPopUp.SetActive(true);
        yesOrNoPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void CheckPopUpOpen()
    {
        gameObject.SetActive(true);
        checkPopUp.SetActive(true);
        checkPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void QuestPopUpOpen()
    {
        gameObject.SetActive(true);
        questPopUp.SetActive(true);
        questPopUp.transform.SetAsLastSibling();

    }

    // 버튼 콜백
    public void CardGameRulePopUpOpen()
    {
        gameObject.SetActive(true);
        CardGameRulePopUp.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

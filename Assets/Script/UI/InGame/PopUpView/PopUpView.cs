using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView : MonoBehaviour
{
    // 에디터 연결

    // 아이콘 팝업
    public GameObject optionPopUp;
    public GameObject inventoryPopUp;
    public GameObject questListPopUp;
    public GameObject questContentPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;

    // 콜백팝업
    public GameObject yesOrNoPopUp;
    public GameObject checkPopUp;
    
    // cardGameView 전용 팝업
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
    public void QuestListPopUpOpen()
    {
        gameObject.SetActive(true);
        questListPopUp.SetActive(true);
        questListPopUp.transform.SetAsLastSibling();
    }

    public void QuestContentPopUpOpen()
    {
        gameObject.SetActive(true);
        questContentPopUp.SetActive(true);
        questContentPopUp.transform.SetAsLastSibling();
    }

    public void GameAssistantPopUpOpen_OnlyOneLives()
    {
        gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.transform.SetAsLastSibling();
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
    public void CardGameRulePopUpOpen()
    {
        gameObject.SetActive(true);
        CardGameRulePopUp.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView_InGame : PopUpViewBase
{
    // 에디터 연결

    // 아이콘 팝업
    public GameObject optionPopUp;
    public InventoryPopUp inventoryPopUp;
    public QuestListPopUp questListPopUp;
    public QuestContentPopUp questContentPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;
    public SaveDataPopUp saveDataPopUp;

    
    
    // cardGameView 전용 팝업
    public CardGameRulePopUp CardGameRulePopUp;


    

    /// <summary>
    /// 팝업은 awake에서 싱글톤생성 호출을 하지 않음
    /// </summary>
    public override void MakePopUpSingleTone()
    {
        inventoryPopUp.MakeSingleTone();
        questListPopUp.MakeSingleTone();
        questContentPopUp.MakeSingleTone();
        gameAssistantPopUp_OnlyOneLives.MakeSingleTone();
        CardGameRulePopUp.MakeSingleTone();
    }

    
    // 버튼 콜백
    public void OptionPopUpOpen()
    {
        //gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void InventoryPopUpOpen()
    {
        //gameObject.SetActive(true);
        inventoryPopUp.gameObject.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    // 버튼 콜백
    public void QuestListPopUpOpen()
    {
        //gameObject.SetActive(true);
        questListPopUp.gameObject.SetActive(true);
        questListPopUp.transform.SetAsLastSibling();
    }

    public void QuestContentPopUpOpen()
    {
        //gameObject.SetActive(true);
        questContentPopUp.gameObject.SetActive(true);
        questContentPopUp.transform.SetAsLastSibling();
    }

    public void GameAssistantPopUpOpen_OnlyOneLives()
    {
        //gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.transform.SetAsLastSibling();
    }

    public void SaveDataPopUpOpen()
    {
        //gameObject.SetActive(true);
        saveDataPopUp.gameObject.SetActive(true);
        saveDataPopUp.transform.SetAsLastSibling();
    }

    

    

    // 버튼 콜백
    public void CardGameRulePopUpOpen()
    {
        //gameObject.SetActive(true);
        CardGameRulePopUp.gameObject.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView : MonoBehaviour
{
    // 에디터 연결

    // 아이콘 팝업
    public GameObject optionPopUp;
    public InventoryPopUp inventoryPopUp;
    public QuestListPopUp questListPopUp;
    public QuestContentPopUp questContentPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;

    // 콜백팝업
    public YesOrNoPopUp yesOrNoPopUp;
    public CheckPopUp checkPopUp;
    
    // cardGameView 전용 팝업
    public CardGameRulePopUp CardGameRulePopUp;


    private void Awake()
    {
        MakePopUpSingleTone();
    }

    /// <summary>
    /// 팝업은 awake에서 싱글톤생성 호출을 하지 않음
    /// </summary>
    public void MakePopUpSingleTone()
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
        gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void InventoryPopUpOpen()
    {
        gameObject.SetActive(true);
        inventoryPopUp.gameObject.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    // 버튼 콜백
    public void QuestListPopUpOpen()
    {
        gameObject.SetActive(true);
        questListPopUp.gameObject.SetActive(true);
        questListPopUp.transform.SetAsLastSibling();
    }

    public void QuestContentPopUpOpen()
    {
        gameObject.SetActive(true);
        questContentPopUp.gameObject.SetActive(true);
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
        yesOrNoPopUp.gameObject.SetActive(true);
        yesOrNoPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void CheckPopUpOpen()
    {
        gameObject.SetActive(true);
        checkPopUp.gameObject.SetActive(true);
        checkPopUp.transform.SetAsLastSibling();
    }

    

    // 버튼 콜백
    public void CardGameRulePopUpOpen()
    {
        gameObject.SetActive(true);
        CardGameRulePopUp.gameObject.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

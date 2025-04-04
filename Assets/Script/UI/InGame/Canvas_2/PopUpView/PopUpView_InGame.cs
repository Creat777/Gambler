using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView_InGame : PopUpViewBase
{
    // ������ ����

    // ������ �˾�
    public GameObject optionPopUp;
    public InventoryPopUp inventoryPopUp;
    public QuestListPopUp questListPopUp;
    public QuestContentPopUp questContentPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;
    public SaveDataPopUp saveDataPopUp;

    
    
    // cardGameView ���� �˾�
    public CardGameRulePopUp CardGameRulePopUp;


    

    /// <summary>
    /// �˾��� awake���� �̱������ ȣ���� ���� ����
    /// </summary>
    public override void MakePopUpSingleTone()
    {
        inventoryPopUp.MakeSingleTone();
        questListPopUp.MakeSingleTone();
        questContentPopUp.MakeSingleTone();
        gameAssistantPopUp_OnlyOneLives.MakeSingleTone();
        CardGameRulePopUp.MakeSingleTone();
    }

    
    // ��ư �ݹ�
    public void OptionPopUpOpen()
    {
        //gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    // ��ư �ݹ�
    public void InventoryPopUpOpen()
    {
        //gameObject.SetActive(true);
        inventoryPopUp.gameObject.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    // ��ư �ݹ�
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

    

    

    // ��ư �ݹ�
    public void CardGameRulePopUpOpen()
    {
        //gameObject.SetActive(true);
        CardGameRulePopUp.gameObject.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

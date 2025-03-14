using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView : MonoBehaviour
{
    // ������ ����

    // ������ �˾�
    public GameObject optionPopUp;
    public GameObject inventoryPopUp;
    public GameObject questPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;

    // �ݹ��˾�
    public GameObject yesOrNoPopUp;
    public GameObject checkPopUp;
    
    // cardGameView ���� �˾�
    public GameObject CardGameRulePopUp;

    

    void Start()
    {
        
    }

    
    // ��ư �ݹ�
    public void OptionPopUpOpen()
    {
        gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    // ��ư �ݹ�
    public void InventoryPopUpOpen()
    {
        gameObject.SetActive(true);
        inventoryPopUp.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();

    }

    // ��ư �ݹ�
    public void QuestPopUpOpen()
    {
        gameObject.SetActive(true);
        questPopUp.SetActive(true);
        questPopUp.transform.SetAsLastSibling();

    }

    public void GameAssistantPopUpOpen_OnlyOneLives()
    {
        gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.gameObject.SetActive(true);
        gameAssistantPopUp_OnlyOneLives.transform.SetAsLastSibling();
    }

    // ��ư �ݹ�
    public void YesOrNoPopUpOpen()
    {
        gameObject.SetActive(true);
        yesOrNoPopUp.SetActive(true);
        yesOrNoPopUp.transform.SetAsLastSibling();
    }

    // ��ư �ݹ�
    public void CheckPopUpOpen()
    {
        gameObject.SetActive(true);
        checkPopUp.SetActive(true);
        checkPopUp.transform.SetAsLastSibling();
    }

    

    // ��ư �ݹ�
    public void CardGameRulePopUpOpen()
    {
        gameObject.SetActive(true);
        CardGameRulePopUp.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}

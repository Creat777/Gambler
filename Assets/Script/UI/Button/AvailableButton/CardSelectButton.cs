using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : ImageChange_ButtonBase
{
    // ������ ����
    public Sprite[] sprites;

    // ��ũ��Ʈ ����
    public CardButtonMemoryPool parent {  get; private set; }
    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    // �޸�Ǯ���� ������������ ����
    private void OnEnable()
    {
        ChangeOn();
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        //Debug.Log($"{orderInHnad}�� ī���� ��ư ��������");
        if(image != null)
        {
            if(orderInHnad < sprites.Length)
            {
                image.sprite = sprites[orderInHnad];
            }
            else
            {
                Debug.LogAssertion("sprite Ȯ�� �ʿ�");
            }
        }
        else
        {
            Debug.LogAssertion("image == null");
        }
    }

    public void MappingButtonWithCard(GameObject card)
    {
        TrumpCardDefault trumpCardDefault = card.GetComponent<TrumpCardDefault>();

        if(trumpCardDefault != null)
        {
            ButtonToCard = card;
            trumpCardScript = trumpCardDefault;
            //UnselectThisCard();
        }
        else
        {
            Debug.LogAssertion($"{card.name}�� card�� �ƴ�");
        }
        
    }

    public void CantSelectThisCard(TrumpCardDefault trumpCardScript)
    {
        Debug.Log($"ī��{trumpCardScript.trumpCardInfo.cardName}�� ������ �� �����ϴ�. ��� ������ ���;� ��");
    }

    private void CheckProperties()
    {
        if (ButtonToCard == null)
        {
            Debug.LogAssertion($"{ButtonToCard.gameObject.name}�� trumpCardScript == null");
            return;
        }
        if (trumpCardScript == null)
        {
            Debug.LogAssertion("ButtonToCard == null");
            return;
        }
        if (parent == null)
        {
            parent = transform.parent.GetComponent<CardButtonMemoryPool>();
        }
    }


    public void SelectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (parent != null)
        {
            if (trumpCardScript.TrySelectThisCard_OnGameSetting(parent.playerMe))
            {
                // ��ư ��ȯ
                ChangeOff();
                SetButtonCallback(UnselectThisCard_OnGameSetting);
            }
            else
            {
                CantSelectThisCard(trumpCardScript);
                return;
            }
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }
    }
    public void UnselectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (parent != null)
        {
            trumpCardScript.UnselectThisCard_OnStartTime(parent.playerMe);

            // ��ư ��ȯ
            ChangeOn();
            SetButtonCallback(SelectThisCard_OnGameSetting);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }

    }

    public void SelectThisCard_OnPlayTime()
    {
        CheckProperties();

        if (trumpCardScript.TrySelectThisCard_OnPlayTime(parent.playerMe))
        {
            // ��ư ��ȯ
            ChangeOff();
            SetButtonCallback(UnselectThisCard_OnPlayTime);

            CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryActivate_Button();
        }
        else
        {
            CantSelectThisCard(trumpCardScript);
            return;
        }
        
    }

    public void UnselectThisCard_OnPlayTime()
    {
        CheckProperties();

        trumpCardScript.UnselectThisCard_OnPlayTime(parent.playerMe);
        // ��ư ��ȯ
        ChangeOn();
        SetButtonCallback(SelectThisCard_OnPlayTime);

        CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryDeactivate_Button();
    }
}

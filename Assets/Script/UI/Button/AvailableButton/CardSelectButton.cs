using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : ImageOnOff_ButtonBase
{
    // ������ ����
    public Sprite[] sprites;

    // ��ũ��Ʈ ����
    private CardButtonSet parent;
    

    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    private void Start()
    {
        Debug.LogWarning("ī�� ������ ��ư�� ������� �� �ݹ� ������ �ȵǴ� ������ ����");
        SetButtonCallback(SelectThisCard_OnStartTime);
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        Debug.Log($"{orderInHnad}�� ī���� ��ư ��������");
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

    public void CantSelectThisCard()
    {
        Debug.Log("ī�带 ������ �� �����ϴ�. ��� ������ ���;� ��");
    }


    public void SelectThisCard_OnStartTime()
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
            parent = transform.parent.GetComponent<CardButtonSet>();
        }

        if (parent != null)
        {
            if (trumpCardScript.TrySelectThisCard_OnStartTime(parent.playerMe))
            {
                // ��ư ��ȯ
                ChangeOffColor();
                SetButtonCallback(UnselectThisCard_OnStartTime);
            }
            else
            {
                CantSelectThisCard();
                return;
            }
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }
    }

    public void SelectThisCard_OnPlayTime()
    {
        Debug.Log("�Լ� ���� ����");
    }

    public void UnselectThisCard_OnStartTime()
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
            parent = transform.parent.GetComponent<CardButtonSet>();
        }

        if (parent != null)
        {
            trumpCardScript.UnselectThisCard_OnStartTime(parent.playerMe);

            // ��ư ��ȯ
            ChangeOnColor();
            SetButtonCallback(SelectThisCard_OnStartTime);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }

    }

    public void UnselectThisCard_OnPlayTime()
    {
        Debug.Log("�Լ� ���� ����");
    }
}

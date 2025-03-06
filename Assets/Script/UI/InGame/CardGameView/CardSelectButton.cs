using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : MonoBehaviour
{
    // ������ ����
    public Sprite[] sprites;
    public Image image;
    public Button button;

    // ��ũ��Ʈ ����

    

    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    private void Awake()
    {
        SetCardButtonCallback(SelectThisCard);
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
            UnselectThisCard();
        }
        else
        {
            Debug.LogAssertion($"{card.name}�� card�� �ƴ�");
        }
        
    }

    public void SetCardButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            if (callback != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(callback);
            }
            else
            {
                Debug.LogAssertion("callback == null");
            }
        }
        else
        {
            Debug.LogAssertion("button == null");
        }
    }

    public void CantSelectThisCard()
    {
        Debug.Log("ī�带 ������ �� �����ϴ�. ��� ������ ���;� ��");
    }


    public void SelectThisCard()
    {
        if (ButtonToCard != null)
        {
            if (trumpCardScript == null)
            {
                trumpCardScript = ButtonToCard.GetComponent<TrumpCardDefault>();
            }

            // ī�� ���ÿ��θ� Ȯ��
            if (trumpCardScript != null)
            {

                trumpCardScript.SelectThisCard();

                // ��ư ��ȯ
                image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                SetCardButtonCallback(UnselectThisCard);
            }
            else
            {
                Debug.LogAssertion($"{ButtonToCard.gameObject.name}�� trumpCardScript == null");
            }
        }
        else
        {
            Debug.LogAssertion("ButtonToCard == null");
        }
    }

    public void UnselectThisCard()
    {
        if (ButtonToCard != null)
        {
            if (trumpCardScript == null)
            {
                trumpCardScript = ButtonToCard.GetComponent<TrumpCardDefault>();
            }

            

            // ī�� ���ÿ��θ� Ȯ��
            if (trumpCardScript != null)
            {

                trumpCardScript.UnselectThisCard();

                // ��ư ��ȯ
                image.color = Color.white;
                SetCardButtonCallback(SelectThisCard);            }
            else
            {
                Debug.LogAssertion($"{ButtonToCard.name}�� TrumpCardDefault == null");
            }
        }
        else
        {
            Debug.LogAssertion($"{ButtonToCard.name}�� ButtonToCard == null");
        }
    }
}

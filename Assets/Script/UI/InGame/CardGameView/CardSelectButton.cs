using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : MonoBehaviour
{
    // 에디터 연결
    public Sprite[] sprites;
    public Image image;
    public Button button;

    // 스크립트 편집

    

    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    private void Awake()
    {
        SetCardButtonCallback(SelectThisCard);
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        Debug.Log($"{orderInHnad}번 카드의 버튼 생성예정");
        if(image != null)
        {
            if(orderInHnad < sprites.Length)
            {
                image.sprite = sprites[orderInHnad];
            }
            else
            {
                Debug.LogAssertion("sprite 확인 필요");
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
            Debug.LogAssertion($"{card.name}은 card가 아님");
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
        Debug.Log("카드를 선택할 수 없습니다. 라는 문구가 나와야 됨");
    }


    public void SelectThisCard()
    {
        if (ButtonToCard != null)
        {
            if (trumpCardScript == null)
            {
                trumpCardScript = ButtonToCard.GetComponent<TrumpCardDefault>();
            }

            // 카드 선택여부를 확인
            if (trumpCardScript != null)
            {

                trumpCardScript.SelectThisCard();

                // 버튼 전환
                image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                SetCardButtonCallback(UnselectThisCard);
            }
            else
            {
                Debug.LogAssertion($"{ButtonToCard.gameObject.name}의 trumpCardScript == null");
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

            

            // 카드 선택여부를 확인
            if (trumpCardScript != null)
            {

                trumpCardScript.UnselectThisCard();

                // 버튼 전환
                image.color = Color.white;
                SetCardButtonCallback(SelectThisCard);            }
            else
            {
                Debug.LogAssertion($"{ButtonToCard.name}의 TrumpCardDefault == null");
            }
        }
        else
        {
            Debug.LogAssertion($"{ButtonToCard.name}의 ButtonToCard == null");
        }
    }
}

using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : ImageOnOff_ButtonBase
{
    // 에디터 연결
    public Sprite[] sprites;

    // 스크립트 편집
    private CardButtonSet parent;
    

    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    private void Start()
    {
        Debug.LogWarning("카드 선택후 버튼이 재생성될 때 콜백 변경이 안되는 문제가 있음");
        SetButtonCallback(SelectThisCard_OnStartTime);
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
            trumpCardScript = trumpCardDefault;
            //UnselectThisCard();
        }
        else
        {
            Debug.LogAssertion($"{card.name}은 card가 아님");
        }
        
    }

    public void CantSelectThisCard()
    {
        Debug.Log("카드를 선택할 수 없습니다. 라는 문구가 나와야 됨");
    }


    public void SelectThisCard_OnStartTime()
    {
        if (ButtonToCard == null)
        {
            Debug.LogAssertion($"{ButtonToCard.gameObject.name}의 trumpCardScript == null");
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
                // 버튼 전환
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
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }
    }

    public void SelectThisCard_OnPlayTime()
    {
        Debug.Log("함수 수정 요함");
    }

    public void UnselectThisCard_OnStartTime()
    {
        if (ButtonToCard == null)
        {
            Debug.LogAssertion($"{ButtonToCard.gameObject.name}의 trumpCardScript == null");
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

            // 버튼 전환
            ChangeOnColor();
            SetButtonCallback(SelectThisCard_OnStartTime);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }

    }

    public void UnselectThisCard_OnPlayTime()
    {
        Debug.Log("함수 수정 요함");
    }
}

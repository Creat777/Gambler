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
    public CardButtonMemoryPool parent {  get; private set; }
    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    // 메모리풀에서 꺼내질때마다 실행
    private void OnEnable()
    {
        ChangeOnColor();
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        //Debug.Log($"{orderInHnad}번 카드의 버튼 생성예정");
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

    public void CantSelectThisCard(TrumpCardDefault trumpCardScript)
    {
        Debug.Log($"카드{trumpCardScript.trumpCardInfo.cardName}를 선택할 수 없습니다. 라는 문구가 나와야 됨");
    }

    private void CheckProperties()
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
                // 버튼 전환
                ChangeOffColor();
                SetButtonCallback(UnselectThisCard_OnStartTime);
            }
            else
            {
                CantSelectThisCard(trumpCardScript);
                return;
            }
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }
    }
    public void UnselectThisCard_OnStartTime()
    {
        CheckProperties();

        if (parent != null)
        {
            trumpCardScript.UnselectThisCard_OnStartTime(parent.playerMe);

            // 버튼 전환
            ChangeOnColor();
            SetButtonCallback(SelectThisCard_OnGameSetting);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }

    }

    public void SelectThisCard_OnPlayTime()
    {
        CheckProperties();

        if (trumpCardScript.TrySelectThisCard_OnPlayTime(parent.playerMe))
        {
            // 버튼 전환
            ChangeOffColor();
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
        // 버튼 전환
        ChangeOnColor();
        SetButtonCallback(SelectThisCard_OnPlayTime);

        CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryDeactivate_Button();
    }
}

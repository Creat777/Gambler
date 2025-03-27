using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // 에디터 연결
    //public CardGamePlayManager cardGamePlayManager;
    public PlayerMe playerMe;
    public CardButtonMemoryPool cardButtonMemoryPool;
    public Text textComp;

    // 스크립트 편집
    public readonly string onFirstButtonText = "선택한 카드\n오픈하기";
    public readonly string onAttackOrDeffenceButtonText = "선택한 카드\n제시하기";

    public void InitAttribute()
    {
        SetButtonCallback(CompleteCardSelect_ChooseCardsToReveal);
        TryDeactivate_Button();
        ChangeText(onFirstButtonText);
    }

    private void Start()
    {
        if(CardGamePlayManager.Instance == null)
        {
            Debug.LogAssertion("카드게임매니저 연결 안됐음");
        }
        if(cardButtonMemoryPool == null)
        {
            Debug.LogAssertion("카드버튼상자 연결 안됐음");
        }
        if(playerMe == null)
        {
            Debug.LogAssertion("플레이어 연결 안됐음");
        }
        if(textComp == null)
        {
            Debug.LogAssertion("자식객체의 text가 연결 안됐음");
        }
    }

    // 실행하는 시점에서 컴퓨터는 이미 선택을 완료했어야함
    public void CheckCompleteSelect_OnChooseCardsToReveal(Dictionary<eCardType, int> cardCountPerType)
    {
        int enumLength = Enum.GetValues(typeof(eCardType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            // 남은 카드의 개수가 1보다 많으면 선택이 완료된 것이 아님
            if (cardCountPerType[(eCardType)i] > 1)
            {
                playerMe.Set_isCompleteSelect_OnGameSetting(false);
                Debug.Log("카드 선택이 완료되지 않음");
                TryDeactivate_Button();
                return;
            }
        }
        playerMe.Set_isCompleteSelect_OnGameSetting(true);
        // 선택이 완료됐으면 버튼을 활성화 시도
        TryActivate_Button();
    }

    public void ChangeText(string text)
    {
        textComp.text = text;
    }

    public void SetButtonCallback(int Select)
    {
        switch(Select)
        {
            case 0:
                ChangeText(onFirstButtonText);
                SetButtonCallback(CompleteCardSelect_ChooseCardsToReveal);break;
            case 1:
                ChangeText(onAttackOrDeffenceButtonText);
                SetButtonCallback(CompleteCardSelect_OnAttack_Or_OnDeffence); break;
        }
    }

    public void CompleteCardSelect_ChooseCardsToReveal()
    {
        Debug.Log("CompleteCardSelect_OnStartTime 실행");

        // 애니메이션 실행
        Sequence sequence = DOTween.Sequence();

        // 서브스크린 닫기
        CardGamePlayManager.Instance.cardGameView.GetSequnce_CardScrrenClose(sequence);

        // 카드 이동 및 공개
        CardGameAnimationManager.Instance.GetSequnce_ChooseCardsToReveal(sequence);

        // 애니메이션이 끝난 후 본게임에 진입
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// 모두 끝난후 다음상태를 진행
        sequence.SetLoops(1);
        sequence.Play();

        // 버튼이 클릭됐으면 똑같은 액션은 할 수 없음
        TryDeactivate_Button();
    }
    public void CompleteCardSelect_OnAttack_Or_OnDeffence()
    {
        Debug.Log($"CompleteCardSelect_OnAttack_Or_OnDeffence 실행");

        // 애니메이션 실행
        Sequence sequence = DOTween.Sequence();

        // 서브스크린 닫기
        CardGamePlayManager.Instance.cardGameView.GetSequnce_CardScrrenClose(sequence);

        // 카드 제시하기
        playerMe.GetSequnce_PresentCard(sequence,playerMe.isAttack);

        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// 모두 끝난후 다음상태를 진행
        sequence.SetLoops(1);
        sequence.Play();

        // 버튼이 클릭됐으면 똑같은 액션은 할 수 없음
        TryDeactivate_Button();
    }

    public override bool TryActivate_Button()
    {
        switch (CardGamePlayManager.Instance.currentProgress)
        {
            case eOOLProgress.num102_BeforeRotateDiceAndDistribution:
            case eOOLProgress.num103_BeforeChooseCardsToReveal:
                {
                    if (CardGamePlayManager.Instance.isCotributionCompleted && playerMe.isCompleteSelect_OnGameSetting)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case eOOLProgress.num201_AttackTurnPlayer:
                {
                    if(playerMe.isCompleteSelect_OnPlayTime)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case eOOLProgress.num301_DefenseTrun_Player:
                {
                    if (playerMe.isCompleteSelect_OnPlayTime)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                {
                    Debug.LogAssertion("잘못된 접근");
                    return false;
                }

        }
    }

    
}

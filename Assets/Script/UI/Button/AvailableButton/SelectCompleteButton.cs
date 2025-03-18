using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // 에디터 연결
    public CardGamePlayManager cardGamePlayManager;
    public PlayerMe playerMe;
    public CardButtonMemoryPool cardButtonMemoryPool;
    public Text textComp;

    // 스크립트 편집
    public readonly string onFirstButtonText = "선택한 카드\n오픈하기";
    public readonly string onPlayButtonText = "선택한 카드\n제시하기";

    public void InitAttribute()
    {
        SetButtonCallback(CompleteCardSelect_OnGameSetting);
        TryDeactivate_Button();
        ChangeText(onFirstButtonText);
    }

    private void Start()
    {
        if(cardGamePlayManager == null)
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
    public void CheckCompleteSelect_OnGameSetting(Dictionary<eCardType, int> cardCountPerType)
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

    public void CompleteCardSelect_OnGameSetting()
    {
        Debug.Log("CompleteCardSelect_OnStartTime 실행");

        if (cardGamePlayManager == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        float returnDelay = 0;

        // 시퀀스를 매개변수로 넣어 같은 시퀀스에 콜백을 추가함
        Sequence sequence = DOTween.Sequence();
        cardGamePlayManager.cardGameView.GetSequnce_CardScrrenClose(sequence);
        sequence.AppendCallback(()=>ChangeText(onPlayButtonText));

        // 모든 플레이어의 선택카드를 확정하고 카드를 정리함
        foreach (var player in cardGamePlayManager.playersList)
        {
            if (player.gameObject.tag == "Player")
            {
                // 기존 카드버튼의 개수와 연결을 초기화
                sequence.AppendCallback(() => cardButtonMemoryPool.InitCardButton(player.closeBox.transform));
            }
            returnDelay += player.GetSequnce_CompleteSelectCard(sequence); // 여기서 카드의 부모객체가 변경됨 (close To open)
            returnDelay += player.GetSequnece_CardSpread(sequence);
        }

        // 컴퓨터의 경우 카드 정리가 끝나면 즉시 공격대상과 제시할 카드를 선택함
        //foreach (var player in cardGamePlayManager.playersList)
        //{
        //    if (player.gameObject.tag == "Player")
        //    {
        //        continue;
        //    }
        //    sequence.AppendCallback(()=>
        //    (player as PlayerEtc).SelectCardAndTarget_OnPlayTime(cardGamePlayManager.playersList)
        //    );
            
        //}
            
        Debug.Log($"애니메이션 총 시간 : {returnDelay}");

        // 모든 플레이어가 카드 정리를 끝냈으면 본게임에 진입하여 공격을 시작
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(
            ()=>
            {
                // 주사위값이 가장 큰 플레이어부터 시계순으로 공격 시작
                List<CardGamePlayerBase> OrderedPlayerList = cardGamePlayManager.GetOrderedPlayerList();

                // 공격할 대상이 있으면 실행
                if (OrderedPlayerList.Count > 1)
                {
                    if (OrderedPlayerList[0] != null)
                    { 
                        OrderedPlayerList[0].AttackOtherPlayers(0, OrderedPlayerList); 
                    }
                    else
                    {
                        Debug.LogAssertion("List의 참조변수가 null참조");
                    }
                    
                }
            });

        sequence.SetLoops(1);
        sequence.Play();

        // 버튼이 클릭됐으면 똑같은 액션은 할 수 없음
        TryDeactivate_Button();

        // 기존 선택완료버튼의 콜백을 변경
        SetButtonCallback(CompleteCardSelect_OnPlayTime);

        // 게임 진행도를 변경
        CardGamePlayManager.Instance.ChangeGameProgress(true);
    }

    public override bool TryActivate_Button()
    {
        switch (CardGamePlayManager.Instance.currentProgress)
        {
            case eCardGameProgress.GameSetting:
                {
                    if (cardGamePlayManager.isCotributionCompleted && playerMe.isCompleteSelect_OnGameSetting)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case eCardGameProgress.PlayTime:
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
            default:
                {
                    Debug.LogAssertion("잘못된 접근");
                    return false;
                }

        }
    }

    public void CompleteCardSelect_OnPlayTime()
    {

        Debug.Log("함수 수정 요함");
    }
}

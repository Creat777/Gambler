using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerEtc : CardGamePlayerBase
{
    public void SelectCard_OnStartTime()
    {
        for (int i = 0; i <CardList.Count; i++)
        {
            TrumpCardDefault card = CardList[i].GetComponent<TrumpCardDefault>();
            if(card != null)
            {
                card.TrySelectThisCard_OnGameSetting(this);
            }
        }
    }

    public void SelectTarget_OnPlayTime(List<CardGamePlayerBase> playerList)
    {
        Debug.Log($"컴퓨터 \"{gameObject.name}\"가 공격할 대상을 선택합니다.");
        int randomPlayerIndex;

        // 자신 이외의 다른 플레이어 찾기
        do{
            randomPlayerIndex = Random.Range(0, playerList.Count);
        } while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // 세팅에 실패했으면 반복
    }

    public void SelectCard_OnPlayTime()
    {
        Debug.Log($"컴퓨터 \"{gameObject.name}\"가 사용할 카드를 선택합니다.");
        int randomCardIndex;

        // 공개되지 않은 카드중에 공격할 카드 고르기
        TrumpCardDefault cardScript;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            cardScript = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((cardScript.TrySelectThisCard_OnPlayTime(this)) == false); // 세팅에 실패하면 반복
        PresentedCardScript = cardScript;

        Debug.Log($"사용된 카드 : {PresentedCardScript}");
    }

    public override void AttackOtherPlayers(int currentOrder, List<CardGamePlayerBase> orderdPlayerList)
    {
        // 컴퓨터가 공격대상 및 사용할 카드를 선택
        SelectTarget_OnPlayTime(orderdPlayerList);
        SelectCard_OnPlayTime();

        Sequence sequence = DOTween.Sequence();
        float returnDelay;

        // DOTO 컴퓨터는 상대를 지목하여 대화를 시작

        // 카드를 제시하는 애니메이션
        returnDelay = GetSequnce_PresentCard(sequence, true);
        

        // 내 공격 끝내기
        Debug.Log($"{gameObject.name}이 공격을 실행함");
        AttackDone = true;

        // 상대 수비 시작
        sequence.AppendInterval(2f);
        sequence.AppendCallback(()=>AttackTarget.DefenceFromOtherPlayers(this));
        
        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"애니메이션 시간 : {returnDelay}");
    }


    public override void DefenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // 수비에 사용할 카드를 선택
        SelectCard_OnPlayTime();

        Sequence sequence = DOTween.Sequence();
        float returnDelay;

        // 카드를 제시하는 애니메이션
        returnDelay = GetSequnce_PresentCard(sequence, false);

        // 내 수비 끝내기
        Debug.Log($"{gameObject.name}이 수비를 실행함");

        // 양쪽 카드를 오픈
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(()=> CardGamePlayManager.Instance.CardOpenAtTheSameTime(AttackerScript, this));


        sequence.AppendInterval(1f);
        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"애니메이션 시간 : {returnDelay}");
    }
}

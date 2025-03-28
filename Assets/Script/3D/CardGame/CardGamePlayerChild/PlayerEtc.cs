using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerEtc : CardGamePlayerBase
{
    public OnlyOneLivesPlayerPanel AsisstantPanel {  get; private set; }

    public void SetAsisstantPanel(OnlyOneLivesPlayerPanel value)
    {
        AsisstantPanel = value;
    }
    public override void AddCoin(int value)
    {
        coin += value;
        AsisstantPanel.PlayerBalanceUpdate();
    }

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
        //do{
        //    randomPlayerIndex = Random.Range(0, playerList.Count);
        //} while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // 세팅에 실패했으면 반복

        Debug.Log("테스트용 상대선택");
        TrySetAttackTarget(playerList[0]);
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
        TyrSetPresentedCard(cardScript);

        Debug.Log($"사용된 카드 : {PresentedCardScript}");
    }


    
    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // 컴퓨터의 공격대상 선택
        SelectTarget_OnPlayTime(PlayerList);

        // 공격에 사용할 카드 선택
        SelectCard_OnPlayTime();

        // 모두 완료되었으면 애니메이션 실행후 다음으로 진행
        PlaySequnce_PresentCard(true);
    }

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // 수비에 사용할 카드를 선택
        SelectCard_OnPlayTime();

        // 모두 완료되었으면 애니메이션 실행후 다음으로 진행
        PlaySequnce_PresentCard(false);


    }

    

    //public void PlaySequnce_Deffence()
    //{
    //    Sequence sequence = DOTween.Sequence();
    //    float returnDelay;

    //    // 카드를 제시하는 애니메이션
    //    returnDelay = GetSequnce_PresentCard(sequence, false);

    //    // 내 수비 끝내기
    //    Debug.Log($"{gameObject.name}이 수비를 실행함");

    //    // 양쪽 카드를 오픈
    //    sequence.AppendInterval(progressDelay);
    //    sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress); // progress 302 실행
    //    //sequence.AppendCallback(()=> CardGamePlayManager.Instance.CardOpenAtTheSameTime(AttackerScript, this));

    //    sequence.SetLoops(1);
    //    sequence.Play();
    //    //Debug.Log($"애니메이션 시간 : {returnDelay}");
    //}
}

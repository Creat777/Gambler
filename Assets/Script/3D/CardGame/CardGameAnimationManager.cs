using DG.Tweening;
using UnityEngine;

public class CardGameAnimationManager : Singleton<CardGameAnimationManager>
{

    public void GetSequnce_OrganizeCardAnimaition(Sequence sequence, CardGamePlayerBase player)
    {
        if (player.gameObject.tag == "Player")
        {
            // 기존 카드버튼의 개수와 연결을 초기화
            sequence.AppendCallback(() => CardButtonMemoryPool.Instance.InitCardButton(player.closeBox.transform));
        }
        Sequence appendSequnce;

        appendSequnce = DOTween.Sequence();
        player.GetSequnce_OpenAndCloseCards(appendSequnce); // 여기서 카드의 부모객체가 변경됨
        sequence.Append(appendSequnce);

        appendSequnce = DOTween.Sequence();
        player.GetSequnece_CardSpread(appendSequnce);
        sequence.Append(appendSequnce);
    }
    /// <summary>
    /// 게임 시작을 위해 모든 게임 참가자가 각 문양을 1장씩만 남기고 오픈하는 애니메이션
    /// </summary>
    public void GetSequnce_ChooseCardsToReveal_Aniamaition(Sequence sequence)
    {
        if (CardGamePlayManager.Instance == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        // 모든 플레이어의 선택카드를 확정하고 카드를 정리함
        foreach (var player in CardGamePlayManager.Instance.playerList)
        {
            GetSequnce_OrganizeCardAnimaition(sequence, player);
        }
    }

}

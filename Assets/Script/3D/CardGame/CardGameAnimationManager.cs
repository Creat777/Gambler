using DG.Tweening;
using UnityEngine;

public class CardGameAnimationManager : Singleton<CardGameAnimationManager>
{

    public void GetSequnce_OrganizeCardAnimaition(Sequence sequence, CardGamePlayerBase player)
    {
        if (player.gameObject.tag == "Player")
        {
            // ���� ī���ư�� ������ ������ �ʱ�ȭ
            sequence.AppendCallback(() => CardButtonMemoryPool.Instance.InitCardButton(player.closeBox.transform));
        }
        Sequence appendSequnce;

        appendSequnce = DOTween.Sequence();
        player.GetSequnce_OpenAndCloseCards(appendSequnce); // ���⼭ ī���� �θ�ü�� �����
        sequence.Append(appendSequnce);

        appendSequnce = DOTween.Sequence();
        player.GetSequnece_CardSpread(appendSequnce);
        sequence.Append(appendSequnce);
    }
    /// <summary>
    /// ���� ������ ���� ��� ���� �����ڰ� �� ������ 1�徿�� ����� �����ϴ� �ִϸ��̼�
    /// </summary>
    public void GetSequnce_ChooseCardsToReveal_Aniamaition(Sequence sequence)
    {
        if (CardGamePlayManager.Instance == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        // ��� �÷��̾��� ����ī�带 Ȯ���ϰ� ī�带 ������
        foreach (var player in CardGamePlayManager.Instance.playerList)
        {
            GetSequnce_OrganizeCardAnimaition(sequence, player);
        }
    }

}

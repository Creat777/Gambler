using DG.Tweening;
using UnityEngine;

public class CardGameAnimationManager : Singleton<CardGameAnimationManager>
{
    /// <summary>
    /// ���� ������ ���� ��� ���� �����ڰ� �� ������ 1�徿�� ����� �����ϴ� �ִϸ��̼�
    /// </summary>
    public void GetSequnce_ChooseCardsToReveal(Sequence sequence)
    {
        if (CardGamePlayManager.Instance == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        float returnDelay = 0;

        // ��� �÷��̾��� ����ī�带 Ȯ���ϰ� ī�带 ������
        foreach (var player in CardGamePlayManager.Instance.playerList)
        {
            if (player.gameObject.tag == "Player")
            {
                // ���� ī���ư�� ������ ������ �ʱ�ȭ
                sequence.AppendCallback(() => CardButtonMemoryPool.Instance.InitCardButton(player.closeBox.transform));
            }

            Sequence appendSequnce;

            appendSequnce = DOTween.Sequence();
            returnDelay += player.GetSequnce_CompleteSelectCard(appendSequnce); // ���⼭ ī���� �θ�ü�� ����� (close To open)
            sequence.Append(appendSequnce);

            appendSequnce = DOTween.Sequence();
            returnDelay += player.GetSequnece_CardSpread(appendSequnce);
            sequence.Append(appendSequnce);
        }

        //Debug.Log($"�ִϸ��̼� �� �ð� : {returnDelay}");

        
    }
}

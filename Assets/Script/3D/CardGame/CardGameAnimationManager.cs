using DG.Tweening;
using System;
using UnityEditor.Animations;
using UnityEngine;

public class CardGameAnimationManager : Singleton<CardGameAnimationManager>
{

    public void GetSequnce_OrganizeCardAnimaition(Sequence sequence, CardGamePlayerBase player, bool isCardReturn = false)
    {
        float delay = 0.5f;

        if (player.gameObject.tag == "Player")
        {
            // ���� ī���ư�� ������ ������ �ʱ�ȭ
            sequence.AppendCallback(() => CardButtonMemoryPool.Instance.InitCardButton(player.closeBox.transform));
        }

        Sequence appendSequnce;

        appendSequnce = DOTween.Sequence();
        player.GetSequnce_OpenAndCloseCards(appendSequnce); // ���⼭ ī���� �θ�ü�� �����
        sequence.Append(appendSequnce);


        // ī�带 ����

        if (isCardReturn) player.ShuffleCard_CloseBox(); // ī�尡 �����Ǿ��� �����ϴ°�� ī�带 ��� ����

        appendSequnce = DOTween.Sequence();
        player.GetSequnece_CardSpread(appendSequnce, delay);
        sequence.Append(appendSequnce);

        if (isCardReturn) // ī�尡 �� ��ġ�� �����ϴ� ��� ���ÿ� ȸ���� ����
        {
            Sequence joinSequnce;
            joinSequnce = DOTween.Sequence();

            // �����ߴ� ȸ������ �ٽ� �÷��̾� �������� ����
            Vector3 targetRotation = player.transform.rotation.eulerAngles;
            joinSequnce.Append(player.PresentedCardScript.transform.DORotate(targetRotation, delay, RotateMode.WorldAxisAdd));
            sequence.Join(joinSequnce);
        }
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

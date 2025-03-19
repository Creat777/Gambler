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
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ������ ����� �����մϴ�.");
        int randomPlayerIndex;

        // �ڽ� �̿��� �ٸ� �÷��̾� ã��
        do{
            randomPlayerIndex = Random.Range(0, playerList.Count);
        } while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // ���ÿ� ���������� �ݺ�
    }

    public void SelectCard_OnPlayTime()
    {
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ����� ī�带 �����մϴ�.");
        int randomCardIndex;

        // �������� ���� ī���߿� ������ ī�� ����
        TrumpCardDefault cardScript;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            cardScript = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((cardScript.TrySelectThisCard_OnPlayTime(this)) == false); // ���ÿ� �����ϸ� �ݺ�
        PresentedCardScript = cardScript;

        Debug.Log($"���� ī�� : {PresentedCardScript}");
    }

    public override void AttackOtherPlayers(int currentOrder, List<CardGamePlayerBase> orderdPlayerList)
    {
        // ��ǻ�Ͱ� ���ݴ�� �� ����� ī�带 ����
        SelectTarget_OnPlayTime(orderdPlayerList);
        SelectCard_OnPlayTime();

        Sequence sequence = DOTween.Sequence();
        float returnDelay;

        // DOTO ��ǻ�ʹ� ��븦 �����Ͽ� ��ȭ�� ����

        // ī�带 �����ϴ� �ִϸ��̼�
        returnDelay = GetSequnce_PresentCard(sequence, true);
        

        // �� ���� ������
        Debug.Log($"{gameObject.name}�� ������ ������");
        AttackDone = true;

        // ��� ���� ����
        sequence.AppendInterval(2f);
        sequence.AppendCallback(()=>AttackTarget.DefenceFromOtherPlayers(this));
        
        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"�ִϸ��̼� �ð� : {returnDelay}");
    }


    public override void DefenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // ���� ����� ī�带 ����
        SelectCard_OnPlayTime();

        Sequence sequence = DOTween.Sequence();
        float returnDelay;

        // ī�带 �����ϴ� �ִϸ��̼�
        returnDelay = GetSequnce_PresentCard(sequence, false);

        // �� ���� ������
        Debug.Log($"{gameObject.name}�� ���� ������");

        // ���� ī�带 ����
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(()=> CardGamePlayManager.Instance.CardOpenAtTheSameTime(AttackerScript, this));


        sequence.AppendInterval(1f);
        sequence.SetLoops(1);
        sequence.Play();
        //Debug.Log($"�ִϸ��̼� �ð� : {returnDelay}");
    }
}

using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.ComponentModel;

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

    public override int TryMinusCoin(int value, out bool isBankrupt)
    {
        if (coin <= value)
        {
            value = coin; // �������� ���Ͽ� ���濡�� ������
            coin = 0;
            isBankrupt = true;
        }
        else
        {
            coin = coin - value;
            isBankrupt = false;
        }
        AsisstantPanel.PlayerBalanceUpdate();
        return value;
    }

    public void SelectCard_OnStartTime()
    {
        for (int i = 0; i <closedCardList.Count; i++)
        {
            TrumpCardDefault card = closedCardList[i].GetComponent<TrumpCardDefault>();
            if(card != null)
            {
                card.TrySelectThisCard_OnGameSetting(this);
            }
        }
    }

    public void SelectTarget_OnPlayTime(List<CardGamePlayerBase> playerList)
    {
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ������ ����� �����մϴ�.");

        // ���հ��� ������ ������� ����
        if(CardGamePlayManager.Instance.Prey != null) // �ڽ��� ���հ��̸� ���� �õ��� ����
        {
            if (TrySetAttackTarget(CardGamePlayManager.Instance.Prey)) return; // ���������� ���⼭ ����
            else Debug.LogWarning("���հ��� ������ ���� ������");
        }

        // �ڽ� �̿��� �ٸ� �÷��̾� ã��
        CardGamePlayerBase weekPlayer;
        int i = 0;
        if (playerList[i] != this) weekPlayer = playerList[i];
        else weekPlayer = playerList[++i];
        for (i++ ; i <playerList.Count; i++)
        {
            if (playerList[i] == this) continue;

            if(weekPlayer.closedCardList.Count > playerList[i].closedCardList.Count) // ���а� �� ���� ���� ����
            {
                weekPlayer = playerList[i];
            }
            else if(weekPlayer.closedCardList.Count == playerList[i].closedCardList.Count &&
                weekPlayer.revealedCardList.Count < playerList[i].revealedCardList.Count) // ���а� ���� ��� ������ ī�尡 �� ���� ���� ����
            {
                weekPlayer = playerList[i];
            }
        }

        // �ùٸ��� ã�����ٸ� ���⼭ ����
        if (TrySetAttackTarget(weekPlayer)) return;

        // ������ ����� ��������� ����
        int randomPlayerIndex;
        do{
            randomPlayerIndex = Random.Range(0, playerList.Count);
        } while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // ���ÿ� ���������� �ݺ�
    }

    public void SelectCard_OnPlayTime(bool isAttack)
    {
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ����� ī�带 �����մϴ�.");

        if (closedCardList.Count <= 0)
        {
            Debug.LogWarning("player {gameObject.name}�� ����� �� �ִ� ī�尡 ����");
            return;
        }

        TrumpCardDefault selectedCard = null;

        // ����� �����߿� ������ ī�带 �켱�ؼ� ����
        if(isAttack)
        {
            TrumpCardDefault revealedCardScript = null;
            foreach (GameObject revealedCard in AttackTarget.revealedCardList)
            {
                if(closedCardList.Contains(revealedCard))
                {
                    revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                    foreach (var myCard in closedCardList)
                    {
                        selectedCard = myCard.GetComponent<TrumpCardDefault>();
                        if (revealedCardScript.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType)
                        {
                            if (selectedCard.TrySelectThisCard_OnPlayTime(this))
                            {
                                if (TyrSetPresentedCard(selectedCard))
                                {
                                    Debug.Log($"���� ī�� : {PresentedCardScript}");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        // ����� ������ �߿� �� ���߿� �ִ� ī��� ���� Ÿ���� �ִٸ� �ش� ī�带 ����
        if (isAttack)
        {
            TrumpCardDefault OpenedCardOfTarget = null;
            foreach (var targetCard in AttackTarget.openedCardList)
            {
                OpenedCardOfTarget = targetCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();
                    if (OpenedCardOfTarget.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this))
                        {
                            if (TyrSetPresentedCard(selectedCard))
                            {
                                Debug.Log($"���� ī�� : {PresentedCardScript}");
                                return;
                            }
                        }
                    }
                }

            }
        }
        

        // ���� or ���� : ��Ŀ�� �ִ� ��� ��Ŀ�� ����
        foreach (var myCard in closedCardList)
        {
            selectedCard = myCard.GetComponent<TrumpCardDefault>();
            if (selectedCard.trumpCardInfo.cardType == PublicSet.eCardType.Joker)
            {
                if(selectedCard.TrySelectThisCard_OnPlayTime(this))
                {
                    if (TyrSetPresentedCard(selectedCard))
                    {
                        Debug.Log($"���� ī�� : {PresentedCardScript}");
                        return;
                    }
                }
            }
        }


        // ī�� ������ ���� ��� �������� ����
        int randomCardIndex;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            selectedCard = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((selectedCard.TrySelectThisCard_OnPlayTime(this)) == false); // ���ÿ� �����ϸ� �ݺ�
        if (TyrSetPresentedCard(selectedCard))
        {
            Debug.Log($"���� ī�� : {PresentedCardScript}");
        }
    }



    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // ��ǻ���� ���ݴ�� ����
        SelectTarget_OnPlayTime(PlayerList);

        // ���ݿ� ����� ī�� ����
        SelectCard_OnPlayTime(true);

        // ��� �Ϸ�Ǿ����� �ִϸ��̼� ������ �������� ����
        PlaySequnce_PresentCard(true);
    }

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // ���� ����� ī�带 ����
        SelectCard_OnPlayTime(false);

        // ��� �Ϸ�Ǿ����� �ִϸ��̼� ������ �������� ����
        PlaySequnce_PresentCard(false);
    }

    



    //public void PlaySequnce_Deffence()
    //{
    //    Sequence sequence = DOTween.Sequence();
    //    float returnDelay;

    //    // ī�带 �����ϴ� �ִϸ��̼�
    //    returnDelay = GetSequnce_PresentCard(sequence, false);

    //    // �� ���� ������
    //    Debug.Log($"{gameObject.name}�� ���� ������");

    //    // ���� ī�带 ����
    //    sequence.AppendInterval(progressDelay);
    //    sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress); // progress 302 ����
    //    //sequence.AppendCallback(()=> CardGamePlayManager.Instance.CardOpenAtTheSameTime(AttackerScript, this));

    //    sequence.SetLoops(1);
    //    sequence.Play();
    //    //Debug.Log($"�ִϸ��̼� �ð� : {returnDelay}");
    //}
}

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
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ������ ����� �����մϴ�.");
        int randomPlayerIndex;

        // �ڽ� �̿��� �ٸ� �÷��̾� ã��
        //do{
        //    randomPlayerIndex = Random.Range(0, playerList.Count);
        //} while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // ���ÿ� ���������� �ݺ�

        Debug.Log("�׽�Ʈ�� ��뼱��");
        TrySetAttackTarget(playerList[0]);
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
        TyrSetPresentedCard(cardScript);

        Debug.Log($"���� ī�� : {PresentedCardScript}");
    }


    
    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // ��ǻ���� ���ݴ�� ����
        SelectTarget_OnPlayTime(PlayerList);

        // ���ݿ� ����� ī�� ����
        SelectCard_OnPlayTime();

        // ��� �Ϸ�Ǿ����� �ִϸ��̼� ������ �������� ����
        PlaySequnce_PresentCard(true);
    }

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // ���� ����� ī�带 ����
        SelectCard_OnPlayTime();

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

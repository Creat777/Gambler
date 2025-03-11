using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // ������ ����
    public CardGamePlayManager cardGamePlayManager;
    public PlayerMe playerMe;
    public CardButtonSet cardButtonSet;
    public Text textComp;

    // ��ũ��Ʈ ����
    public readonly string onFirstButtonText = "������ ī��\n�����ϱ�";
    public readonly string onPlayButtonText = "������ ī��\n�����ϱ�";
    private void Start()
    {
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("ī����ӸŴ��� ���� �ȵ���");
            Destroy(gameObject);
        }
        if(cardButtonSet == null)
        {
            Debug.LogAssertion("ī���ư���� ���� �ȵ���");
            Destroy(gameObject);
        }
        if(playerMe == null)
        {
            Debug.LogAssertion("�÷��̾� ���� �ȵ���");
            Destroy(gameObject);
        }
        if(textComp == null)
        {
            Debug.LogAssertion("�ڽİ�ü�� text�� ���� �ȵ���");
            Destroy(gameObject);
        }
    }

    // �����ϴ� �������� ��ǻ�ʹ� �̹� ������ �Ϸ��߾����
    public void CheckCompleteSelect_OnStartTime(Dictionary<eCardType, int> cardCountPerType)
    {
        int enumLength = Enum.GetValues(typeof(eCardType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            // ���� ī���� ������ 1���� ������ ������ �Ϸ�� ���� �ƴ�
            if (cardCountPerType[(eCardType)i] > 1)
            {
                Debug.Log("ī�� ������ �Ϸ���� ����");
                TryDeactivate_Button();
                return;
            }
        }
        // ������ �Ϸ������ ��ư�� Ȱ��ȭ��
        TryActivate_Button();
    }

    public void ChangeText(string text)
    {
        textComp.text = text;
    }

    public void CompleteCardSelect_OnStartTime()
    {
        if (cardGamePlayManager == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
        }

        // ��ư�� Ŭ�������� �Ȱ��� �׼��� �� �� ����
        TryDeactivate_Button();

        // ���� ī�弱�ù�ư�� ���ÿϷ��ư�� �ݹ��� ����
        SetButtonCallback(CompleteCardSelect_OnPlayTime);
        foreach (CardSelectButton cardButton in cardButtonSet.cardSelectButtonList)
        {
            cardButton.SetButtonCallback(cardButton.SelectThisCard_OnPlayTime);
        }

        float returnDelay = 0;

        // �������� �Ű������� �־� ���� �������� �ݹ��� �߰���
        Sequence sequence = DOTween.Sequence();
        cardGamePlayManager.cardGameView.GetSequnce_CardScrrenClose(sequence);
        sequence.AppendCallback(()=>ChangeText(onPlayButtonText));

        // ��� �÷��̾��� ����ī�带 Ȯ���ϰ� ī�带 ������
        foreach (var player in cardGamePlayManager.playersList)
        {
            if (player.gameObject.tag == "Player")
            {
                sequence.AppendCallback(() => cardGamePlayManager.cardButtonSet.InitCardButton(player.closeBox.transform));
            }
            returnDelay += player.GetSequnce_CompleteSelectCard(sequence); // ���⼭ ī���� �θ�ü�� ����� (close To open)
            returnDelay += player.GetSequnece_CardSpread(sequence);
        }

        // ��ǻ���� ��� ī�� ������ ������ ��� ���ݴ��� ������ ī�带 ������
        foreach (var player in cardGamePlayManager.playersList)
        {
            if (player.gameObject.tag == "Player")
            {
                continue;
            }
            sequence.AppendCallback(()=>
            (player as PlayerEtc).SelectCardAndTarget_OnPlayTime(cardGamePlayManager.playersList)
            );
            
        }
            
        Debug.Log($"�ִϸ��̼� �� �ð� : {returnDelay}");

        // ��� �÷��̾ ī�� ������ �������� �����ӿ� �����Ͽ� ������ ����
        sequence.AppendCallback(
            ()=>
            {
                // �ֻ������� ���� ū �÷��̾���� �ð������ ���� ����
                List<CardGamePlayerBase> OrderedPlayerList = cardGamePlayManager.GetOrderedPlayerList();

                // ������ ����� ������ ����
                if (OrderedPlayerList.Count > 1)
                {
                    if (OrderedPlayerList[0] != null)
                    { 
                        OrderedPlayerList[0].AttackOtherPlayers(0, OrderedPlayerList); 
                    }
                    else
                    {
                        Debug.LogAssertion("List�� ���������� null����");
                    }
                    
                }
            });

        sequence.SetLoops(1);
        sequence.Play();

        
    }

    public override bool TryActivate_Button()
    {
        if (button != null && cardGamePlayManager.isCotributionCompleted)
        {
            button.interactable = true;
            return true;
        }
        else
        {
            Debug.Log("button == null");
            return false;
        }
    }

    public void CompleteCardSelect_OnPlayTime()
    {
        playerMe.SetBoolButtonClickTrue();
        Debug.Log("�Լ� ���� ����");
    }
}

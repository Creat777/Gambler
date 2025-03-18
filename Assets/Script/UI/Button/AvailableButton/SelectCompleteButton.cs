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
    public CardButtonMemoryPool cardButtonMemoryPool;
    public Text textComp;

    // ��ũ��Ʈ ����
    public readonly string onFirstButtonText = "������ ī��\n�����ϱ�";
    public readonly string onPlayButtonText = "������ ī��\n�����ϱ�";

    public void InitAttribute()
    {
        SetButtonCallback(CompleteCardSelect_OnGameSetting);
        TryDeactivate_Button();
        ChangeText(onFirstButtonText);
    }

    private void Start()
    {
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("ī����ӸŴ��� ���� �ȵ���");
        }
        if(cardButtonMemoryPool == null)
        {
            Debug.LogAssertion("ī���ư���� ���� �ȵ���");
        }
        if(playerMe == null)
        {
            Debug.LogAssertion("�÷��̾� ���� �ȵ���");
        }
        if(textComp == null)
        {
            Debug.LogAssertion("�ڽİ�ü�� text�� ���� �ȵ���");
        }
    }

    // �����ϴ� �������� ��ǻ�ʹ� �̹� ������ �Ϸ��߾����
    public void CheckCompleteSelect_OnGameSetting(Dictionary<eCardType, int> cardCountPerType)
    {
        int enumLength = Enum.GetValues(typeof(eCardType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            // ���� ī���� ������ 1���� ������ ������ �Ϸ�� ���� �ƴ�
            if (cardCountPerType[(eCardType)i] > 1)
            {
                playerMe.Set_isCompleteSelect_OnGameSetting(false);
                Debug.Log("ī�� ������ �Ϸ���� ����");
                TryDeactivate_Button();
                return;
            }
        }
        playerMe.Set_isCompleteSelect_OnGameSetting(true);
        // ������ �Ϸ������ ��ư�� Ȱ��ȭ �õ�
        TryActivate_Button();
    }

    public void ChangeText(string text)
    {
        textComp.text = text;
    }

    public void CompleteCardSelect_OnGameSetting()
    {
        Debug.Log("CompleteCardSelect_OnStartTime ����");

        if (cardGamePlayManager == null)
        {
            Debug.LogAssertion("cardGamePlayManager == null");
            return;
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
                // ���� ī���ư�� ������ ������ �ʱ�ȭ
                sequence.AppendCallback(() => cardButtonMemoryPool.InitCardButton(player.closeBox.transform));
            }
            returnDelay += player.GetSequnce_CompleteSelectCard(sequence); // ���⼭ ī���� �θ�ü�� ����� (close To open)
            returnDelay += player.GetSequnece_CardSpread(sequence);
        }

        // ��ǻ���� ��� ī�� ������ ������ ��� ���ݴ��� ������ ī�带 ������
        //foreach (var player in cardGamePlayManager.playersList)
        //{
        //    if (player.gameObject.tag == "Player")
        //    {
        //        continue;
        //    }
        //    sequence.AppendCallback(()=>
        //    (player as PlayerEtc).SelectCardAndTarget_OnPlayTime(cardGamePlayManager.playersList)
        //    );
            
        //}
            
        Debug.Log($"�ִϸ��̼� �� �ð� : {returnDelay}");

        // ��� �÷��̾ ī�� ������ �������� �����ӿ� �����Ͽ� ������ ����
        sequence.AppendInterval(2.0f);
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

        // ��ư�� Ŭ�������� �Ȱ��� �׼��� �� �� ����
        TryDeactivate_Button();

        // ���� ���ÿϷ��ư�� �ݹ��� ����
        SetButtonCallback(CompleteCardSelect_OnPlayTime);

        // ���� ���൵�� ����
        CardGamePlayManager.Instance.ChangeGameProgress(true);
    }

    public override bool TryActivate_Button()
    {
        switch (CardGamePlayManager.Instance.currentProgress)
        {
            case eCardGameProgress.GameSetting:
                {
                    if (cardGamePlayManager.isCotributionCompleted && playerMe.isCompleteSelect_OnGameSetting)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case eCardGameProgress.PlayTime:
                {
                    if(playerMe.isCompleteSelect_OnPlayTime)
                    {
                        button.interactable = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                {
                    Debug.LogAssertion("�߸��� ����");
                    return false;
                }

        }
    }

    public void CompleteCardSelect_OnPlayTime()
    {

        Debug.Log("�Լ� ���� ����");
    }
}

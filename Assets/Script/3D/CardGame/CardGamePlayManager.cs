using UnityEngine;
using System.Collections.Generic;

public class CardGamePlayManager : MonoBehaviour
{
    // ������ ����
    public Transform playerParent;
    public CardGameView cardGameView;

    public CardManager cardManager;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;
    public CardButtonSet cardButtonSet;

    // ��ũ��Ʈ ����
    public bool isCotributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playersList {  get; private set; }

    public List<CardGamePlayerBase> playerList_FisrtToEnd { get; private set; }
    public int layerOfMe { get; private set; }

    private void Awake()
    {
        layerOfMe = LayerMask.NameToLayer("Me");
        InitPlayer();
    }

    public void EnterCardGame()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            if(playersList[i].CompareTag("Player"))
            {
                Debug.Log("�÷��̾��� ������ ����");
                playersList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                Debug.Log("��ǻ������ ������ ������ ����");
                playersList[i].SetCoin();
            }
        }
    }

    

    public void InitPlayer()
    {
        if (playersList == null) playersList = new List<CardGamePlayerBase>();
        else playersList.Clear();

        if(playerList_FisrtToEnd == null) playerList_FisrtToEnd = new List<CardGamePlayerBase>();
        else playerList_FisrtToEnd.Clear();

        for (int i = 0; i < playerParent.childCount; i++)
        {
            CardGamePlayerBase player = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();
            playersList.Add(player);
        }
    }


    public List<CardGamePlayerBase> GetOrderedPlayerList()
    {
        // ù��° ���� ã��
        int firstPlayerIndex = 0;
        for (int i = 1; i<playersList.Count; i++ )
        {
            if (playersList[firstPlayerIndex].myDiceValue < playersList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playersList[firstPlayerIndex].gameObject.name}�� �ֻ������� " +
            $"{playersList[firstPlayerIndex].myDiceValue}���� ���� Ů�ϴ�. ù��°�� ������ �����մϴ�.");

        // ù��° �÷��̾���� �ݽð�������� ����Ʈ�� �ֱ�
        playerList_FisrtToEnd.Clear();
        for (int i = 0; i < playersList.Count; i++)
        {
            int finalIndex = (firstPlayerIndex + i) % playersList.Count;
            playerList_FisrtToEnd.Add(playersList[finalIndex]);
        }
        return playerList_FisrtToEnd;
    }

    public void GameSetting()
    {
        if(playersList == null)
        {
            Debug.LogAssertion("players == null");
            return;
        }

        // �ݺ��������� �ֻ����� �ѹ� ���� �� �����, ��� ó���� ���� �� �ٽ� ȣ��Ǿ����
        for (int i = 0; i < playersList.Count; i++)
        {
            // �̹� �ֻ����� �������� �н�(���� �ֻ����� ���� Player(Me)�� �ڵ����� �н���)
            if (playersList[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}�� �̹� �ֻ����� ������");
                continue;
            }
            Debug.Log($"�ֻ����� ������ ���� �÷��̾� ���� == {4 - i}");
            Debug.Log($"{playersList[i].gameObject.name}�� �ֻ����� �������ϴ�");
            diceManager.RotateDice(playersList[i].gameObject);
            return; // �ֻ����� �ѹ� �������� �Լ��� ����
        }

        // �ֻ����� �� ���ȴµ� �� �Լ��� �����ߴٸ� ���� ó���� ��������

        // ��ǻ�Ͱ� �ڵ����� ī�带 ������ ����
        foreach (CardGamePlayerBase player in playersList)
        {
            // �÷��̾�� ī�带 ���� ���� �������� �Ѿ
            if(player.CompareTag("Player"))
            {
                continue;
            }

            PlayerEtc computer =  player.gameObject.GetComponent<PlayerEtc>();

            if (computer != null)
            {
                computer.SelectCard_OnStartTime();
            }

        }

        // �÷��̾�(me)�� ī�� ������ �Ϸ��ϴ� ��ư�� Ȱ��ȭ
        isCotributionCompleted = true;
        cardGameView.selectCompleteButton.TryActivate_Button();
    }

}

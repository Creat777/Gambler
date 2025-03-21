using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;

public enum eCardGameProgress
{
    None,
    BeforeStartGame,
    GameSetting,
    PlayTime,
    final
}

public enum ePlayerTurnState
{
    Attack,
    Defense
}

// �̱��������� �θ�ü�� �����Ͽ� �� �̵��� �ı���
public class CardGamePlayManager : Singleton<CardGamePlayManager>
{
    // ������ ����
    public Transform playerParent;
    public CardGameView cardGameView;

    public PlayerMe playerMe;
    public CardManager cardManager;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;
    [SerializeField] private CardButtonMemoryPool cardButtonMemoryPool;
    public CardButtonMemoryPool CardButtonMemoryPool
    {
        get 
        {
            if (cardButtonMemoryPool != null)
            {
                return cardButtonMemoryPool;
            }
            else
            {
                Debug.LogAssertion("error");
                return null;
            }
        }

        private set
        {
            cardButtonMemoryPool = value;
        }
    }

    public PopUpView popUpView;

    // ��ũ��Ʈ ����
    public eCardGameProgress currentProgress { get; private set; }
    public ePlayerTurnState currentPlayerTurn {  get; private set; }
    public bool isCotributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playersList {  get; private set; }
    public List<CardGamePlayerBase> playerList_FisrtToEnd { get; private set; }
    public int layerOfMe { get; private set; }
    private int coinMultiple; // ���� ���

    protected override void Awake()
    {
        base.Awake();
        layerOfMe = LayerMask.NameToLayer("Me");
        InitPlayer();
    }

    public void InitPlayer()
    {
        if (playersList == null) playersList = new List<CardGamePlayerBase>();
        else playersList.Clear();

        if (playerList_FisrtToEnd == null) playerList_FisrtToEnd = new List<CardGamePlayerBase>();
        else playerList_FisrtToEnd.Clear();

        for (int i = 0; i < playerParent.childCount; i++)
        {
            CardGamePlayerBase player = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();
            playersList.Add(player);
        }
    }

    public void InitCurrentGame()
    {
        ChangeGameProgress(false, eCardGameProgress.GameSetting);
        playerMe.InitAttribute();
        cardGameView.InitAttribute();

        isCotributionCompleted = false;
    }

    public void EnterCardGame()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            if(playersList[i].CompareTag("Player"))
            {
                //Debug.Log("�÷��̾��� ������ ����");
                playersList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                //Debug.Log($"��ǻ��{gameObject.name}���� ������ ������ ����");
                playersList[i].SetCoin();
            }
        }

        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();
        cardGameView.playerInterface.InitInterface();
    }

    public void ChangeGameProgress(bool isIncrease, eCardGameProgress progress = eCardGameProgress.None)
    {
        // 1������ ��� �μ��� bool���� ����
        if (isIncrease)
        {
            currentProgress++;
        }

        // ���� ����Ǵ� ��� false�� �Բ� �μ��� �� 2�� �ԷµǾ����
        else
        {
            if(progress == eCardGameProgress.None)
            {
                Debug.LogAssertion("�߸��� ����");
                return;
            }
            currentProgress = progress;
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

        // Interface ����
        cardGameView.playerInterface.ChangeInterfaceNext();

        // �ʿ������ ī�嵦�� ����
        deckOfCards.StartDisappearEffect();

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

    public void  CardOpenAtTheSameTime(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // �� ī�带 ���ÿ� ����
        Sequence sequence = DOTween.Sequence();

        Sequence appendSequence = DOTween.Sequence();
        AttackerScript.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, AttackerScript);
        sequence.Append(appendSequence);

        Sequence joinSequnce = DOTween.Sequence();
        DefenderScript.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, DefenderScript);
        sequence.Join(joinSequnce);

        // ī�� ���� �� ��� Ȯ��
        sequence.AppendCallback(()=>DetermineTheResult(AttackerScript, DefenderScript));

        sequence.SetLoops(1);
        sequence.Play();
    }

    private void DetermineTheResult(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // ���� �÷��̾� ������ ������ ����Ͽ� �̵��ϴ� ������ ������(1��, 4��, 9��� ����)
        coinMultiple = 1 + playerParent.childCount - playersList.Count;
        coinMultiple *= coinMultiple;

        // ��Ŀ�� �ִ��� ���� Ȯ��
        if (AttackerScript.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            OnJokerAppear(AttackerScript, DefenderScript);
        }
        else if (DefenderScript.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            OnJokerAppear(DefenderScript, AttackerScript);
        }

        // ���ݼ��� �Ǵ� ���񼺰� ���� �Ǻ�
        else if (AttackerScript.PresentedCardScript.trumpCardInfo.cardType ==
            DefenderScript.PresentedCardScript.trumpCardInfo.cardType) // ���� ����
        {
            OnAttackSuccess(AttackerScript, DefenderScript);
        }
        else // ���� ����
        {
            OnDefenceSuccess(AttackerScript, DefenderScript);
        }
    }

    private void OnJokerAppear(CardGamePlayerBase JokerPresenter, CardGamePlayerBase Victim)
    {
        // ��Ŀ�� ������ ��� �������� ī�尪 * ���
        int resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue * coinMultiple;

        JokerPresenter.AddCoin(resultValue);
        Victim.AddCoin(-resultValue);
    }

    private void OnAttackSuccess(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // �����ڿ� �������� �������� * ���
        int resultValue =
            AttackerScript.PresentedCardScript.trumpCardInfo.cardValue -
            DefenderScript.PresentedCardScript.trumpCardInfo.cardValue;
        resultValue = Mathf.Abs(resultValue) * coinMultiple;

        AttackerScript.AddCoin(resultValue);
        DefenderScript.AddCoin(-resultValue);
    }

    private void OnDefenceSuccess(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {

    }
}

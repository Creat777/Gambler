using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;
using System.Linq;



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
    public eOOLProgress currentProgress { get; private set; }
    public bool isCotributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playerList {  get; private set; }
    public Queue<CardGamePlayerBase> OrderedPlayerQueue { get; private set; }
    public eCriteria currentCriteria { get; private set; }





    public CardGamePlayerBase Attacker { get; private set; }
    public CardGamePlayerBase Deffender { get; private set; }
    public CardGamePlayerBase Joker { get; private set; }
    public CardGamePlayerBase Victim { get; private set; }

    public void SetAttacker(CardGamePlayerBase value)
    {
        Attacker = value;
    }

    public void SetDeffender(CardGamePlayerBase value)
    {
        Deffender = value;
    }

    public void SetJoker(CardGamePlayerBase value)
    {
        Joker = value;
    }

    public void SetVictim(CardGamePlayerBase value)
    {
        Victim = value;
    }

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
        if (playerList == null) playerList = new List<CardGamePlayerBase>();
        else playerList.Clear();

        if (OrderedPlayerQueue == null) OrderedPlayerQueue = new Queue<CardGamePlayerBase>();
        else OrderedPlayerQueue.Clear();

        for (int i = 0; i < playerParent.childCount; i++)
        {
            CardGamePlayerBase player = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();
            playerList.Add(player);
        }
    }

    public void InitCurrentGame()
    {
        playerMe.InitAttribute();
        cardGameView.InitAttribute();

        isCotributionCompleted = false;
    }

    public void EnterCardGame()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].CompareTag("Player"))
            {
                //Debug.Log("�÷��̾��� ������ ����");
                playerList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                //Debug.Log($"��ǻ��{gameObject.name}���� ������ ������ ����");
                playerList[i].SetCoin();
            }
        }

        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();
        cardGameView.playerInterface.InitInterface();

        GameManager.connector.iconView_Script.TryIconUnLock(eIcon.GameAssistant);
        SetProgress_EnterGame();
    }

    /// <summary>
    /// cardGameView�� ó�� �����Ҷ��� ����
    /// </summary>
    public void SetProgress_EnterGame()
    {
        currentProgress = eOOLProgress.num101_BeforeStartGame;
        GameManager.connector.textWindowView_Script.StartTextWindow(currentProgress);
    }

    public void InitProgress_NewGame()
    {
        currentProgress = eOOLProgress.num102_BeforeRotateDiceAndDistribution;
        GameManager.connector.textWindowView_Script.StartTextWindow(currentProgress);
    }

    /// <summary>
    /// �ѹ��� ����
    /// </summary>
    public void NextProgress()
    {

        if(currentProgress == eOOLProgress.num104_OnChooseFirstPlayer || currentProgress == eOOLProgress.num405_AfterSettlementOfAccounts)
        {
            if (Attacker != null)
            {
                if (Attacker.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num201_AttackTurnPlayer;
                }
                else
                {
                    // ��ǻ���� ��� num201_AttackTurnPlayer�� �н���
                    currentProgress = eOOLProgress.num202_Attack;
                }
            }
            else
            {
                // num405_AfterSettlementOfAccounts ���� ������ �÷��̾ ������ �����
                currentProgress = eOOLProgress.num501_final;
            }
            
        }
        else if (currentProgress == eOOLProgress.num202_Attack)
        {
            if(Deffender != null)
            {
                if (Deffender.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num301_DefenseTrun_Player;
                }
                else
                {
                    // ��ǻ���� ��� num301_DefenseTrun_Player�� �н���
                    currentProgress = eOOLProgress.num302_Defense;
                }
            }
        }
        else if(currentProgress == eOOLProgress.num401_CardOpenAtTheSameTime)
        {
            switch(currentCriteria)
            {
                case eCriteria.JokerWin: currentProgress = eOOLProgress.num402_OnJokerAppear; break;
                case eCriteria.AttakkerWin: currentProgress = eOOLProgress.num403_OnAttackSuccess; break;
                case eCriteria.DeffenderWin: currentProgress = eOOLProgress.num404_OnDefenceSuccess; break;
            }
        }
        else
        {
            currentProgress++;
        }
        GameManager.connector.textWindowView_Script.StartTextWindow(currentProgress);

    }
    public Queue<CardGamePlayerBase> InitOrderedPlayerQueue()
    {
        // ù��° ���� ã��
        int firstPlayerIndex = 0;
        for (int i = 1; i<playerList.Count; i++ )
        {
            if (playerList[firstPlayerIndex].myDiceValue < playerList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playerList[firstPlayerIndex].gameObject.name}�� �ֻ������� " +
            $"{playerList[firstPlayerIndex].myDiceValue}���� ���� Ů�ϴ�. ù��°�� ������ �����մϴ�.");

        // ù��° �÷��̾���� �ݽð�������� ť�� �ֱ�
        OrderedPlayerQueue.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            int finalIndex = (firstPlayerIndex + i) % playerList.Count;
            OrderedPlayerQueue.Enqueue(playerList[finalIndex]);
        }

        SetAttacker(OrderedPlayerQueue.Dequeue());
        return OrderedPlayerQueue;
    }


    public void StartPlayerAttack()
    {
        Attacker.AttackOtherPlayers(playerList);
    }


    public void StartPlayerDeffence()
    {
        Deffender.DeffenceFromOtherPlayers(Attacker);
    }


    public void GameSetting()
    {
        if(playerList == null)
        {
            Debug.LogAssertion("players == null");
            return;
        }

        // �ݺ��������� �ֻ����� �ѹ� ���� �� �����, ��� ó���� ���� �� �ٽ� ȣ��Ǿ����
        for (int i = 0; i < playerList.Count; i++)
        {
            // �̹� �ֻ����� �������� �н�(���� �ֻ����� ���� Player(Me)�� �ڵ����� �н���)
            if (playerList[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}�� �̹� �ֻ����� ������");
                continue;
            }
            Debug.Log($"�ֻ����� ������ ���� �÷��̾� ���� == {4 - i}");
            Debug.Log($"{playerList[i].gameObject.name}�� �ֻ����� �������ϴ�");
            diceManager.RotateDice(playerList[i].gameObject);
            return; // �ֻ����� �ѹ� �������� �Լ��� ����
        }
        // �ֻ����� �� ���ȴµ� �� �Լ��� �����ߴٸ� ���� ó���� ��������
        Sequence sequence = DOTween.Sequence();

        // Interface ����
        Sequence appendSequence = DOTween.Sequence();
        cardGameView.playerInterface.GetSequnce_ChangeInterfaceNext(appendSequence);
        sequence.Append(appendSequence);

        // �ʿ������ ī�嵦�� ����
        sequence.JoinCallback(deckOfCards.StartDisappearEffect);

        sequence.AppendCallback(() => InitProgress_NewGame());

        // ��ǻ�Ͱ� �ڵ����� ī�带 ������ ����
        foreach (CardGamePlayerBase player in playerList)
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
        sequence.AppendCallback(NextProgress);

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void DetermineTheResult()
    {
        // ���� �÷��̾� ������ ������ ����Ͽ� �̵��ϴ� ������ ������(1��, 4��, 9��� ����)
        coinMultiple = 1 + playerParent.childCount - playerList.Count;
        coinMultiple *= coinMultiple;

        // ��Ŀ�� �ִ��� ���� Ȯ��
        if (Attacker.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Attacker);
            SetVictim(Deffender);

            NextProgress();
            //OnJokerAppear(Attacker, Deffender);
        }
        else if (Deffender.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Deffender);
            SetVictim(Attacker);

            NextProgress();
            //OnJokerAppear(Deffender, Attacker);
        }

        // ���ݼ��� �Ǵ� ���񼺰� ���� �Ǻ�
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType ==
            Deffender.PresentedCardScript.trumpCardInfo.cardType) // ���� ����
        {
            currentCriteria = eCriteria.AttakkerWin;

            NextProgress();
            //OnAttackSuccess(Attacker, Deffender);
        }
        else // ���� ����
        {
            currentCriteria = eCriteria.DeffenderWin;

            NextProgress();
            //OnDefenceSuccess(Attacker, Deffender);
        }
    }

    public void OnJokerAppear()
    {
        // ��Ŀ�� ������ ��� �������� ī�尪 * ���
        int resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue * coinMultiple;

        Joker.AddCoin(resultValue);
        Victim.AddCoin(-resultValue);
    }

    public void OnAttackSuccess()
    {
        // �����ڿ� �������� �������� * ���
        int resultValue =
            Attacker.PresentedCardScript.trumpCardInfo.cardValue -
            Deffender.PresentedCardScript.trumpCardInfo.cardValue;
        resultValue = Mathf.Abs(resultValue) * coinMultiple;

        Attacker.AddCoin(resultValue);
        Deffender.AddCoin(-resultValue);
    }

    public void OnDefenceSuccess()
    {

    }
}

using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;



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
    public MainCameraAnimation mainCamAnime;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp;

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
    public int ExpressionValue { get; private set; }

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

    public void ResultExpression(int value)
    {
        // ���� �÷��̾� ������ ������ ����Ͽ� �̵��ϴ� ������ ������(1��, 4��, 9��� ����)
        coinMultiple = 1 + playerParent.childCount - playerList.Count;
        coinMultiple *= coinMultiple;

        ExpressionValue = value * coinMultiple;
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

        // ��ŸƮ��ư ȭ�鿡�� �����
        cardGameView.InitStartButton();

        // �������̽� �ʱ�ȭ
        cardGameView.playerInterface.InitInterface();

        // ���� ��ý���Ʈ �ʱ�ȭ
        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();
        
        // ���Ӿ�ý���Ʈ�� ����� �� �ֵ��� �õ�
        GameManager.connector.iconView_Script.TryIconUnLock(eIcon.GameAssistant);

        // ���Ӿ�ý���Ʈ�� ���� ����� ���������� ����
        gameAssistantPopUp.PlaceRestrictionToAllSelections();

        // ���� ���൵ �ʱ�ȭ
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
    /// ��� ��������� ���� �������� �ʰ� �ش� �Լ��� ������
    /// </summary>
    public void NextProgress()
    {
        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");

        if (currentProgress == eOOLProgress.num104_OnChooseFirstPlayer || 
            currentProgress == eOOLProgress.num405_OnChooseNextPlayer)
        {
            if (Attacker != null)
            {
                // ��ǻ���� ��� num201_AttackTurnPlayer�� �н���
                if (Attacker.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num201_AttackTurnPlayer;
                }
                else
                {
                    
                    currentProgress = eOOLProgress.num202_Attack;
                }
            }
            else
            {
                Debug.LogAssertion("�̹� ������ �����ڰ� �������� �ʾ���");
            }
            
        }
        else if (currentProgress == eOOLProgress.num202_Attack)
        {
            if(Deffender != null)
            {
                // ��ǻ���� ��� num301_DefenseTrun_Player�� �н���
                if (Deffender.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num301_DefenseTrun_Player;
                }
                else
                {
                    currentProgress = eOOLProgress.num302_Defense;
                }
            }
            else
            {
                Debug.LogAssertion("�̹� ������ ����ڰ� �������� �ʾ���");
            }
        }
        else if(currentProgress == eOOLProgress.num302_Defense)
        {
            currentProgress = eOOLProgress.num401_CardOpenAtTheSameTime;
        }

        else if(currentProgress == eOOLProgress.num401_CardOpenAtTheSameTime)
        {
            switch(currentCriteria)
            {
                case eCriteria.JokerWin: currentProgress = eOOLProgress.num402_OnJokerAppear; break;
                case eCriteria.AttakkerWin: currentProgress = eOOLProgress.num403_OnAttackSuccess; break;
                case eCriteria.DeffenderWin: currentProgress = eOOLProgress.num404_OnDefenceSuccess; break;
                default: Debug.LogAssertion("�������� ���� ���� ����"); break;
            }
        }
        else if(currentProgress == eOOLProgress.num402_OnJokerAppear ||
            currentProgress == eOOLProgress.num403_OnAttackSuccess ||
            currentProgress == eOOLProgress.num404_OnDefenceSuccess)
        {
            // �������ʰ� ������ ť���� �� ������ ����� ���� �� �������ʸ� ����
            if(OrderedPlayerQueue.Count > 1)
            {
                Attacker = OrderedPlayerQueue.Dequeue();
                currentProgress = eOOLProgress.num405_OnChooseNextPlayer;
            }
            // �׷��� ������ ������ ����
            else
            {
                currentProgress = eOOLProgress.num501_final;
            }
        }
        else if(currentProgress == eOOLProgress.num501_final)
        {
            InitProgress_NewGame();
        }
        else // 1�� �����ϴ� ��� ++�� ���
        {
            currentProgress++;
        }


        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");
        GameManager.connector.textWindowView_Script.StartTextWindow(currentProgress);

        
    }
    public void InitOrderedPlayerQueue()
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
        //return OrderedPlayerQueue;
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

        // �ֻ������� ���� ū �÷��̾���� �ð������ ť�� �ʱ�ȭ
        InitOrderedPlayerQueue();

        Sequence sequence = DOTween.Sequence();

        // Interface ����
        Sequence appendSequence = DOTween.Sequence();
        cardGameView.playerInterface.GetSequnce_ChangeInterfaceNext(appendSequence);
        sequence.Append(appendSequence);

        // �ʿ������ ī�嵦�� ����
        sequence.JoinCallback(deckOfCards.StartDisappearEffect);

        sequence.AppendCallback(NextProgress);

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

    public void  CardOpenAtTheSameTime()
    {
        // �� ī�带 ���ÿ� ����
        Sequence sequence = DOTween.Sequence();

        // ī�޶� ������ �� ȭ���� Ȯ��
        mainCamAnime.GetSequnce_CameraZoomIn(sequence);

        Sequence appendSequence = DOTween.Sequence();
        Attacker.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, Attacker);
        sequence.Append(appendSequence);

        Sequence joinSequnce = DOTween.Sequence();
        Deffender.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, Deffender);
        sequence.Join(joinSequnce);

        // ī�带 �ڼ��� Ȯ���ϱ� ���� �ð�
        float delay = 1.5f;
        sequence.AppendInterval(delay);

        // ī�� ���� �� ��� Ȯ��
        sequence.AppendCallback(DetermineTheResult);

        sequence.SetLoops(1);
        sequence.Play();
    }

    //public void CardOpenAtTheSameTime(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    //{
    //    // �� ī�带 ���ÿ� ����
    //    Sequence sequence = DOTween.Sequence();

    //    Sequence appendSequence = DOTween.Sequence();
    //    AttackerScript.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, AttackerScript);
    //    sequence.Append(appendSequence);

    //    Sequence joinSequnce = DOTween.Sequence();
    //    DefenderScript.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, DefenderScript);
    //    sequence.Join(joinSequnce);

    //    // ī�� ���� �� ��� Ȯ��
    //    sequence.AppendCallback(NextProgress);

    //    sequence.SetLoops(1);
    //    sequence.Play();
    //}

    public void DetermineTheResult()
    {
        

        // ��Ŀ�� �ִ��� ���� Ȯ��
        if (Attacker.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Attacker);
            SetVictim(Deffender);
        }
        else if (Deffender.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Deffender);
            SetVictim(Attacker);
        }
        // ���ݼ��� �Ǵ� ���񼺰� ���� �Ǻ�
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType ==
            Deffender.PresentedCardScript.trumpCardInfo.cardType) // ���� ����
        {
            currentCriteria = eCriteria.AttakkerWin;
        }
        else // ���� ����
        {
            currentCriteria = eCriteria.DeffenderWin;
        }

        // 402, 403, 404 �� �ϳ� ����
        NextProgress();
    }

    public void OnJokerAppear()
    {
        // ��Ŀ�� ������ ��� �������� ī�尪 
        int resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue;

        // ������� ����Ͽ� ����
        ResultExpression(resultValue);

        Joker.AddCoin(ExpressionValue);
        Victim.AddCoin(-ExpressionValue);

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Victim.PresentedCardScript.UnselectThisCard_OnPlayTime(Victim);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Joker);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Victim);

        sequence.AppendCallback(NextProgress); // 405 �Ǵ� 501�� �̵�

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnAttackSuccess()
    {
        // �����ڿ� �������� �������� * ���
        int resultValue =
            Attacker.PresentedCardScript.trumpCardInfo.cardValue -
            Deffender.PresentedCardScript.trumpCardInfo.cardValue;
        resultValue = Mathf.Abs(resultValue);

        // ������� ����Ͽ� ����
        ResultExpression(resultValue);

        Attacker.AddCoin(ExpressionValue);
        Deffender.AddCoin(-ExpressionValue);

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Attacker.PresentedCardScript.UnselectThisCard_OnPlayTime(Attacker);
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender);

        sequence.AppendCallback(NextProgress); // 405 �Ǵ� 501�� �̵�

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnDefenceSuccess()
    {

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender);

        sequence.AppendCallback(NextProgress); // 405 �Ǵ� 501�� �̵�

        sequence.SetLoops(0);
        sequence.Play();
    }
}

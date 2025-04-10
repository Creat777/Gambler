using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using NUnit.Framework.Constraints;



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
    public PopUpView_InGame popUpView;
    public MainCameraAnimation mainCamAnime;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp;

    // ��ũ��Ʈ ����
    public eOOLProgress currentProgress { get; private set; }
    public bool isDistributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playerList {  get; private set; }
    public Queue<CardGamePlayerBase> OrderedPlayerQueue { get; private set; }
    public eCriteria currentCriteria { get; private set; }
    





    public CardGamePlayerBase Attacker { get; private set; }
    public CardGamePlayerBase Deffender { get; private set; }
    public CardGamePlayerBase Joker { get; private set; }
    public CardGamePlayerBase Victim { get; private set; }
    public CardGamePlayerBase Prey {  get; private set; }
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

    /// <summary>
    /// �� ������ ���۵ɶ� ���հ��� �ʱ�ȭ��
    /// </summary>
    public void ClearPrey()
    {
        Prey = null;
    }

    /// <summary>
    /// �������� ������ ���������� ���հ��� �ֳ� Ž����
    /// </summary>
    public void TrySetPrey()
    {
        foreach(CardGamePlayerBase player in playerList)
        {
            if(player.closedCardList.Count == 0)
            {
                if(Prey == null) Prey = player; // ���հ��� ������ �ش� �÷��̾ ����
                else if (Prey != player && player.CompareTag("Player")) Prey = player; // ���ΰ��� �켱�ؼ� ���հ��� ��
            }
        }
    }

    public int layerOfMe { get; private set; }
    private int coinMultiple; // ���� ���

    protected override void Awake()
    {
        base.Awake();
        layerOfMe = LayerMask.NameToLayer("Me");
        
    }

    public void InitPlayerList()
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
        foreach (CardGamePlayerBase player in playerList)
        {
            player.InitAttribute_All();
        }
        cardGameView.InitAttribute();

        isDistributionCompleted = false;

        ClearPrey();
    }

    public void EnterCardGame()
    {
        InitPlayerList();

        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].CompareTag("Player"))
            {
                //Debug.Log("�÷��̾��� ������ ����");
                playerList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.coin);
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
        GameManager.connector_InGame.iconView_Script.TryIconUnLock(eIcon.GameAssistant);

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
        (GameManager.connector as Connector_InGame).textWindowView_Script.StartTextWindow(currentProgress);
    }

    public void SetProgress(eOOLProgress progress)
    {
        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");
        currentProgress = progress;

        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");
        (GameManager.connector as Connector_InGame).textWindowView_Script.StartTextWindow(currentProgress);
    }
    /// <summary>
    /// ��� ��������� ���� �������� �ʰ� �ش� �Լ��� ������
    /// </summary>
    public void NextProgress()
    {
        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");

        if (currentProgress == eOOLProgress.num104_OnChooseFirstPlayer || 
            currentProgress == eOOLProgress.num407_OnChooseNextPlayer)
        {
            if (Attacker != null)
            {
                // ī�尡 1�常 �����Ͽ��� ����� �� ��Ŀ�� ��������� ���ݿ� �� ī�尡 ����
                // ������ ī�尡 ���� ���
                if (Attacker.closedCardList.Count <= 0)
                {
                    currentProgress = eOOLProgress.num203_PlayerCantAttack;
                }
                // �÷��̾��� ���
                else if (Attacker.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num201_AttackTurnPlayer;
                }
                // ��ǻ���� ���
                else
                {
                    currentProgress = eOOLProgress.num202_AttackDone;
                }
            }
            else
            {
                Debug.LogAssertion("�̹� ������ �����ڰ� �������� �ʾ���");
            }
            
        }
        else if (currentProgress == eOOLProgress.num202_AttackDone)
        {
            if(Deffender != null)
            {
                // ī�尡 1�常 �����Ͽ��� ��Ŀ�� ����߰ų� ���ݿ� �����Ѱ�� ����� ī�尡 ����
                // ������ ī�尡 ���� ���
                if (Deffender.closedCardList.Count <= 0)
                {
                    currentProgress = eOOLProgress.num303_PlayerCantDefense;
                }
                else if (Deffender.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num301_DefenseTrun_Player;
                }
                else
                {
                    currentProgress = eOOLProgress.num302_DefenseDone;
                }
            }
            else
            {
                Debug.LogAssertion("�̹� ������ ����ڰ� �������� �ʾ���");
            }
        }
        else if(currentProgress == eOOLProgress.num302_DefenseDone ||
                currentProgress == eOOLProgress.num303_PlayerCantDefense)
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
                case eCriteria.HuntingTime: currentProgress = eOOLProgress.num405_OnHuntPrey; break;
                default: Debug.LogAssertion("�������� ���� ���� ����"); break;
            }
        }
        else if(currentProgress == eOOLProgress.num203_PlayerCantAttack ||
            currentProgress == eOOLProgress.num402_OnJokerAppear ||
            currentProgress == eOOLProgress.num403_OnAttackSuccess ||
            currentProgress == eOOLProgress.num404_OnDefenceSuccess ||
            currentProgress == eOOLProgress.num405_OnHuntPrey||
            currentProgress == eOOLProgress.num406_OnPlayerBankrupt) // ���ΰ��� ��� �������� �Ѿ�� �ʰ� ������ �����
        {
            // �������ʰ� ������ ť���� �� ������ ����� ���� �� �������ʸ� ����
            if(OrderedPlayerQueue.Count > 0)
            {
                Debug.Log($"���� �÷��̾� ���� : {OrderedPlayerQueue.Count}");

                TrySetPrey(); // ���հ��� �ֳ� Ȯ��

                foreach (var player in playerList)
                {
                    player.InitAttribute_ForNextOrder();
                }

                Attacker = OrderedPlayerQueue.Dequeue();
                currentProgress = eOOLProgress.num407_OnChooseNextPlayer;
            }
            // �׷��� ������ ������ ����
            else
            {
                currentProgress = eOOLProgress.num501_final;
            }
        }
        else if(currentProgress == eOOLProgress.num501_final) // ������ ������ ���� ������ ������ ���
        {
            currentProgress = eOOLProgress.num102_BeforeRotateDiceAndDistribution;
        }
        else // 1�� �����ϴ� ��� ++�� ���
        {
            currentProgress++;
        }


        Debug.Log($"���� �����Ȳ : {currentProgress.ToString()}");
        (GameManager.connector as Connector_InGame).textWindowView_Script.StartTextWindow(currentProgress);

        
    }
    public void InitOrderedPlayerQueue()
    {
        // ù��° ���� ã��
        int firstPlayerIndex = 0;
        for (int i = 1; i<playerList.Count; i++ )
        {
            if (playerList[firstPlayerIndex].myDiceValue > playerList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playerList[firstPlayerIndex].gameObject.name}�� �ֻ������� " +
            $"{playerList[firstPlayerIndex].myDiceValue}���� ���� �۽��ϴ�. ù��°�� ������ �����մϴ�.");

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
        isDistributionCompleted = true;
        cardGameView.selectCompleteButton.TryActivate_Button();
    }

    public void  CardOpenAtTheSameTime()
    {
        // �� ī�带 ���ÿ� ����
        Sequence sequence = DOTween.Sequence();

        // ī�޶� ������ �� ȭ���� Ȯ��
        mainCamAnime.GetSequnce_CameraZoomIn(sequence);

        if (Attacker != null && Attacker.PresentedCardScript != null)
        {
            Sequence appendSequence = DOTween.Sequence();
            Attacker.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, Attacker);
            sequence.Append(appendSequence);
        }
        else Debug.LogWarning("�����ڰ� ī�带 �������� ����");

        // ���հ��� �÷��̾�� ī�带 �������� ����
        if (Deffender != null && Deffender.PresentedCardScript != null)
        {
            Sequence joinSequnce = DOTween.Sequence();
            Deffender.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, Deffender);
            sequence.Join(joinSequnce);
        }
        else Debug.LogWarning("����ڰ� ī�带 �������� ����");

        // ī�带 �ڼ��� Ȯ���ϱ� ���� �ð�
        float delay = 1.5f;
        sequence.AppendInterval(delay);

        // ī�� ���� �� ��� Ȯ��
        sequence.AppendCallback(DetermineTheResult);

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void DetermineTheResult()
    {
        // ���� ����� ī�尡 ���� ���
        if(Prey != null)
        {
            currentCriteria = eCriteria.HuntingTime;
        }

        // ��Ŀ�� �ִ� ���
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
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
        // ���� ����
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType ==
            Deffender.PresentedCardScript.trumpCardInfo.cardType ||// ī���� ������ ���� ���
            Attacker.PresentedCardScript.trumpCardInfo.cardValue ==
            Deffender.PresentedCardScript.trumpCardInfo.cardValue) // ī���� ���� ���� ���
        {
            currentCriteria = eCriteria.AttakkerWin;
        }
        else // ���� ����
        {
            currentCriteria = eCriteria.DeffenderWin;
        }

        // ����� ���� ���� ���, ���� ������ Text�� �˷����ϱ⿡ �̸� ����ؾ���
        ResultExpression();

        // 402, 403, 404 �� �ϳ� ����
        NextProgress();
    }

    public void ResultExpression()
    {
        int resultValue = 0;

        switch (currentCriteria)
        {
            case eCriteria.JokerWin:
                {
                    resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue;
                }
                break;

            case eCriteria.HuntingTime:
                {
                    resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue;
                }break;

            case eCriteria.AttakkerWin:
                {
                    resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue -
                                    Deffender.PresentedCardScript.trumpCardInfo.cardValue;
                    if(resultValue == 0)
                    {
                        resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue;
                    }
                    resultValue = Mathf.Abs(resultValue);
                }break;
            //case eCriteria.DeffenderWin: break;
            default: break; // �ʿ� ������ ������ ���
        }

        // ���� �÷��̾� ������ ������ ����Ͽ� �̵��ϴ� ������ ������(1��, 4��, 9��� ����)
        coinMultiple = 1 + playerParent.childCount - playerList.Count;
        coinMultiple *= coinMultiple;

        int defaultMultiple = 10; //������ ���̵��� ���� ���� �� �� �ֵ��� �����?
        ExpressionValue = resultValue * coinMultiple * defaultMultiple;

        if(Prey != null) // ���հ��� ������ 2�� �̺�Ʈ
        {
            ExpressionValue *= 2;
        }
    }

    public void OnJokerAppear()
    {
        int result = Victim.TryMinusCoin(ExpressionValue, out bool isBankrupt);
        Joker.AddCoin(result);
        

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Victim.PresentedCardScript.UnselectThisCard_OnPlayTime(Victim);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Joker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Victim, true);

        if(isBankrupt)
        {
            sequence.AppendCallback(() => SetProgress(eOOLProgress.num406_OnPlayerBankrupt));
        }
        else
        {
            sequence.AppendCallback(NextProgress); // 407 �Ǵ� 501�� �̵�
        }
        

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnAttackSuccess()
    {
        int result = Deffender.TryMinusCoin(ExpressionValue, out bool isBankrupt);
        Attacker.AddCoin(result);

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Attacker.PresentedCardScript.UnselectThisCard_OnPlayTime(Attacker);
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender, true);

        if (isBankrupt)
        {
            SetVictim(Deffender); // ������ �������� ��ũ��Ʈ���� ������ Ȱ���ϱ� ����  Victim���� ����
            sequence.AppendCallback(() => SetProgress(eOOLProgress.num406_OnPlayerBankrupt));
        }
        else
        {
            sequence.AppendCallback(NextProgress); // 407 �Ǵ� 501�� �̵�
        }


        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnDefenceSuccess()
    {
        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�� �𼿷�
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender, true);

        sequence.AppendCallback(NextProgress); // 406 �Ǵ� 501�� �̵�

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnHuntPrey()
    {
        int result = Prey.TryMinusCoin(ExpressionValue, out bool isBankrupt);
        Attacker.AddCoin(result);

        Sequence sequence = DOTween.Sequence();

        // ȭ�� �� �ƿ�
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // ī�� ���� , ���ο��� ���ư� ī�常 �𼿷���
        Attacker.PresentedCardScript.UnselectThisCard_OnPlayTime(Attacker);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);

        if (isBankrupt)
        {
            sequence.AppendCallback(() => SetProgress(eOOLProgress.num406_OnPlayerBankrupt));
        }
        else
        {
            sequence.AppendCallback(NextProgress); // 407 �Ǵ� 501�� �̵�
        }

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnPlayerBankrupt()
    {
        Debug.Log($"�÷��̾�{Victim.characterInfo.CharacterName} �Ļ�");
        if (Victim.CompareTag("Player")) 
        {
            GameManager.Instance.GameOver();
            return;
        }// �Ļ��Ѱ� ���ΰ��̸� ���� ������ �ʿ����

        // ����Ʈ���� �����Ͽ� ���갪�� ����
        playerList.Remove(Victim);

        // Queue���� �ش� �÷��̾��� ������ ����
        Queue<CardGamePlayerBase> tempQueue = new Queue<CardGamePlayerBase>(OrderedPlayerQueue);
        OrderedPlayerQueue.Clear();
        while (tempQueue.Count > 0)
        {
            CardGamePlayerBase player = tempQueue.Dequeue();
            if(player == Victim)
            {
                continue;
            }
            else
            {
                OrderedPlayerQueue.Enqueue(player);
            }
        }

        // ���Ӿ�ý���Ʈ���� �ش� �÷��̾� ����, PlayerMe�� "if (Victim.CompareTag("Player"))" ���� �ɷ�����
        GameAssistantPopUp_OnlyOneLives.Instance.ReturnObject((Victim as PlayerEtc).AsisstantPanel.gameObject);

        NextProgress();
    }
}

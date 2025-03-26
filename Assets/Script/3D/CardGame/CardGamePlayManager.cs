using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;
using System.Linq;



// 싱글톤이지만 부모객체가 존재하여 씬 이동시 파괴됨
public class CardGamePlayManager : Singleton<CardGamePlayManager>
{
    // 에디터 연결
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

    // 스크립트 편집
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
    private int coinMultiple; // 코인 배수

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
                //Debug.Log("플레이어의 코인을 연동");
                playerList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                //Debug.Log($"컴퓨터{gameObject.name}한테 랜덤한 코인을 증정");
                playerList[i].SetCoin();
            }
        }

        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();
        cardGameView.playerInterface.InitInterface();

        GameManager.connector.iconView_Script.TryIconUnLock(eIcon.GameAssistant);
        SetProgress_EnterGame();
    }

    /// <summary>
    /// cardGameView에 처음 진입할때만 실행
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
    /// 한방향 진행
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
                    // 컴퓨터의 경우 num201_AttackTurnPlayer는 패스됨
                    currentProgress = eOOLProgress.num202_Attack;
                }
            }
            else
            {
                // num405_AfterSettlementOfAccounts 다음 공격할 플레이어가 없으면 종료됨
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
                    // 컴퓨터의 경우 num301_DefenseTrun_Player는 패스됨
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
        // 첫번째 순서 찾기
        int firstPlayerIndex = 0;
        for (int i = 1; i<playerList.Count; i++ )
        {
            if (playerList[firstPlayerIndex].myDiceValue < playerList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playerList[firstPlayerIndex].gameObject.name}의 주사위값은 " +
            $"{playerList[firstPlayerIndex].myDiceValue}으로 제일 큽니다. 첫번째로 공격을 시작합니다.");

        // 첫번째 플레이어부터 반시계방향으로 큐에 넣기
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

        // 반복문이지만 주사위를 한번 굴린 후 종료됨, 모든 처리가 끝난 후 다시 호출되어야함
        for (int i = 0; i < playerList.Count; i++)
        {
            // 이미 주사위를 돌렸으면 패스(직접 주사위를 돌린 Player(Me)는 자동으로 패스됨)
            if (playerList[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}은 이미 주사위를 돌렸음");
                continue;
            }
            Debug.Log($"주사위를 굴리지 않은 플레이어 숫자 == {4 - i}");
            Debug.Log($"{playerList[i].gameObject.name}의 주사위가 굴러갑니다");
            diceManager.RotateDice(playerList[i].gameObject);
            return; // 주사위를 한번 돌렸으면 함수를 종료
        }
        // 주사위를 다 돌렸는데 이 함수에 진입했다면 다음 처리를 진행해줌
        Sequence sequence = DOTween.Sequence();

        // Interface 변경
        Sequence appendSequence = DOTween.Sequence();
        cardGameView.playerInterface.GetSequnce_ChangeInterfaceNext(appendSequence);
        sequence.Append(appendSequence);

        // 필요없어진 카드덱을 제거
        sequence.JoinCallback(deckOfCards.StartDisappearEffect);

        sequence.AppendCallback(() => InitProgress_NewGame());

        // 컴퓨터가 자동으로 카드를 고르도록 만듬
        foreach (CardGamePlayerBase player in playerList)
        {
            // 플레이어는 카드를 직접 고르니 다음으로 넘어감
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

        // 플레이어(me)가 카드 선택을 완료하는 버튼을 활성화
        isCotributionCompleted = true;
        cardGameView.selectCompleteButton.TryActivate_Button();
    }

    public void  CardOpenAtTheSameTime(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // 각 카드를 동시에 오픈
        Sequence sequence = DOTween.Sequence();

        Sequence appendSequence = DOTween.Sequence();
        AttackerScript.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, AttackerScript);
        sequence.Append(appendSequence);

        Sequence joinSequnce = DOTween.Sequence();
        DefenderScript.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, DefenderScript);
        sequence.Join(joinSequnce);

        // 카드 오픈 후 결과 확인
        sequence.AppendCallback(NextProgress);

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void DetermineTheResult()
    {
        // 남은 플레이어 숫자의 제곱에 비례하여 이동하는 코인이 결정됨(1배, 4배, 9배로 증가)
        coinMultiple = 1 + playerParent.childCount - playerList.Count;
        coinMultiple *= coinMultiple;

        // 조커가 있는지 먼저 확인
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

        // 공격성공 또는 수비성공 여부 판별
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType ==
            Deffender.PresentedCardScript.trumpCardInfo.cardType) // 공격 성공
        {
            currentCriteria = eCriteria.AttakkerWin;

            NextProgress();
            //OnAttackSuccess(Attacker, Deffender);
        }
        else // 수비 성공
        {
            currentCriteria = eCriteria.DeffenderWin;

            NextProgress();
            //OnDefenceSuccess(Attacker, Deffender);
        }
    }

    public void OnJokerAppear()
    {
        // 조커를 제시한 경우 피해자의 카드값 * 배수
        int resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue * coinMultiple;

        Joker.AddCoin(resultValue);
        Victim.AddCoin(-resultValue);
    }

    public void OnAttackSuccess()
    {
        // 공격자와 수비자의 숫자차이 * 배수
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

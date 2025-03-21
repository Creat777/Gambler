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
    public eCardGameProgress currentProgress { get; private set; }
    public ePlayerTurnState currentPlayerTurn {  get; private set; }
    public bool isCotributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playersList {  get; private set; }
    public List<CardGamePlayerBase> playerList_FisrtToEnd { get; private set; }
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
                //Debug.Log("플레이어의 코인을 연동");
                playersList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                //Debug.Log($"컴퓨터{gameObject.name}한테 랜덤한 코인을 증정");
                playersList[i].SetCoin();
            }
        }

        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();
        cardGameView.playerInterface.InitInterface();
    }

    public void ChangeGameProgress(bool isIncrease, eCardGameProgress progress = eCardGameProgress.None)
    {
        // 1증가의 경우 인수를 bool값만 받음
        if (isIncrease)
        {
            currentProgress++;
        }

        // 값이 변경되는 경우 false와 함께 인수가 총 2개 입력되어야함
        else
        {
            if(progress == eCardGameProgress.None)
            {
                Debug.LogAssertion("잘못된 접근");
                return;
            }
            currentProgress = progress;
        }
        
    }
    public List<CardGamePlayerBase> GetOrderedPlayerList()
    {
        // 첫번째 순서 찾기
        int firstPlayerIndex = 0;
        for (int i = 1; i<playersList.Count; i++ )
        {
            if (playersList[firstPlayerIndex].myDiceValue < playersList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playersList[firstPlayerIndex].gameObject.name}의 주사위값은 " +
            $"{playersList[firstPlayerIndex].myDiceValue}으로 제일 큽니다. 첫번째로 공격을 시작합니다.");

        // 첫번째 플레이어부터 반시계방향으로 리스트에 넣기
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

        // 반복문이지만 주사위를 한번 굴린 후 종료됨, 모든 처리가 끝난 후 다시 호출되어야함
        for (int i = 0; i < playersList.Count; i++)
        {
            // 이미 주사위를 돌렸으면 패스(직접 주사위를 돌린 Player(Me)는 자동으로 패스됨)
            if (playersList[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}은 이미 주사위를 돌렸음");
                continue;
            }
            Debug.Log($"주사위를 굴리지 않은 플레이어 숫자 == {4 - i}");
            Debug.Log($"{playersList[i].gameObject.name}의 주사위가 굴러갑니다");
            diceManager.RotateDice(playersList[i].gameObject);
            return; // 주사위를 한번 돌렸으면 함수를 종료
        }
        // 주사위를 다 돌렸는데 이 함수에 진입했다면 다음 처리를 진행해줌

        // Interface 변경
        cardGameView.playerInterface.ChangeInterfaceNext();

        // 필요없어진 카드덱을 제거
        deckOfCards.StartDisappearEffect();

        // 컴퓨터가 자동으로 카드를 고르도록 만듬
        foreach (CardGamePlayerBase player in playersList)
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
        sequence.AppendCallback(()=>DetermineTheResult(AttackerScript, DefenderScript));

        sequence.SetLoops(1);
        sequence.Play();
    }

    private void DetermineTheResult(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // 남은 플레이어 숫자의 제곱에 비례하여 이동하는 코인이 결정됨(1배, 4배, 9배로 증가)
        coinMultiple = 1 + playerParent.childCount - playersList.Count;
        coinMultiple *= coinMultiple;

        // 조커가 있는지 먼저 확인
        if (AttackerScript.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            OnJokerAppear(AttackerScript, DefenderScript);
        }
        else if (DefenderScript.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            OnJokerAppear(DefenderScript, AttackerScript);
        }

        // 공격성공 또는 수비성공 여부 판별
        else if (AttackerScript.PresentedCardScript.trumpCardInfo.cardType ==
            DefenderScript.PresentedCardScript.trumpCardInfo.cardType) // 공격 성공
        {
            OnAttackSuccess(AttackerScript, DefenderScript);
        }
        else // 수비 성공
        {
            OnDefenceSuccess(AttackerScript, DefenderScript);
        }
    }

    private void OnJokerAppear(CardGamePlayerBase JokerPresenter, CardGamePlayerBase Victim)
    {
        // 조커를 제시한 경우 피해자의 카드값 * 배수
        int resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue * coinMultiple;

        JokerPresenter.AddCoin(resultValue);
        Victim.AddCoin(-resultValue);
    }

    private void OnAttackSuccess(CardGamePlayerBase AttackerScript, CardGamePlayerBase DefenderScript)
    {
        // 공격자와 수비자의 숫자차이 * 배수
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

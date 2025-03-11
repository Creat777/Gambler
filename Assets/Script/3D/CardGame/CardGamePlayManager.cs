using UnityEngine;
using System.Collections.Generic;

public class CardGamePlayManager : MonoBehaviour
{
    // 에디터 연결
    public Transform playerParent;
    public CardGameView cardGameView;

    public CardManager cardManager;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;
    public CardButtonSet cardButtonSet;

    // 스크립트 편집
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
                Debug.Log("플레이어의 코인을 연동");
                playersList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.money);
            }
            else
            {
                Debug.Log("컴퓨터한테 랜덤한 코인을 증정");
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

}

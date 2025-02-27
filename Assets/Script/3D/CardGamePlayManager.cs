using Unity.VisualScripting;
using UnityEngine;

public class CardGamePlayManager : MonoBehaviour
{
    // 에디터 연결
    public Transform playerParent;
    public CardGameView cardGameView;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;

    // 스크립트 편집
    private CardGamePlayerBase[] player;
    public int layerOfMe { get; private set; }

    private void Awake()
    {
        InitPlayer();
        layerOfMe = LayerMask.NameToLayer("Me");
    }

    public void InitPlayer()
    {
        player = new CardGamePlayerBase[playerParent.childCount];
        for(int i = 0; i < player.Length; i++)
        {
            player[i] = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();

            if (player[i] != null)
            {
                player[i].InitAttribute();
            }
            else
            {
                Debug.LogAssertion($"{player[i]} == null");
            }

            
        }
    }

    public void NPC_Dice()
    {
        Debug.Log($"NPC_Dice실행 \n" +
            $"player.Length == {player.Length}");

        // 반복문이지만 주사위를 한번 굴린 후 종료됨, 모든 처리가 끝난 후 다시 호출되어야함
        for (int i = 0; i < player.Length; i++)
        {
            // 이미 주사위를 돌렸으면 패스

            if (player[i].diceDone == true)
            {
                Debug.Log($"{player[i].gameObject.name}은 이미 주사위를 돌렸음");
                continue;
            }

            Debug.Log($"현재 i값은 = {i} \n" +
                $"{player[i].gameObject.name}의 주사위가 굴러갑니다");
            diceManager.RotateDice(player[i].gameObject);
            return; // 주사위를 한번 돌렸으면 함수를 종료
        }

        // 주사위를 다 돌렸는데 이 함수에 진입했다면 다음 처리를 진행해줌
        // doto -> 게임 시작

    }
}

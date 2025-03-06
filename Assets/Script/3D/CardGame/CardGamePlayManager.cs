using Unity.VisualScripting;
using UnityEngine;

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
    public CardGamePlayerBase[] players {  get; private set; }
    public int layerOfMe { get; private set; }

    private void Awake()
    {
        layerOfMe = LayerMask.NameToLayer("Me");
        InitPlayer();
    }

    public void InitGame()
    {
        cardGameView.diceButton.Activate_Button();
        cardGameView.cardScreenButton.Deactivate_Button();
        cardGameView.selectCompleteButton.SetButtonCallback(cardGameView.selectCompleteButton.CompleteCardSelect);
        cardGameView.selectCompleteButton.Deactivate_Button();

    }

    public void InitPlayer()
    {
        players = new CardGamePlayerBase[playerParent.childCount];

        for(int i = 0; i < players.Length; i++)
        {
            players[i] = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();
        }
    }

    public void GameSetting()
    {
        if(players == null)
        {
            Debug.LogAssertion("players == null");
            return;
        }

        // �ݺ��������� �ֻ����� �ѹ� ���� �� �����, ��� ó���� ���� �� �ٽ� ȣ��Ǿ����
        for (int i = 0; i < players.Length; i++)
        {
            // �̹� �ֻ����� �������� �н�(���� �ֻ����� ���� Player(Me)�� �ڵ����� �н���)
            if (players[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}�� �̹� �ֻ����� ������");
                continue;
            }
            Debug.Log($"�ֻ����� ������ ���� �÷��̾� ���� == {4 - i}");
            Debug.Log($"{players[i].gameObject.name}�� �ֻ����� �������ϴ�");
            diceManager.RotateDice(players[i].gameObject);
            return; // �ֻ����� �ѹ� �������� �Լ��� ����
        }

        // �ֻ����� �� ���ȴµ� �� �Լ��� �����ߴٸ� ���� ó���� ��������
        cardGameView.selectCompleteButton.Activate_Button();


    }
}

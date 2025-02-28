using Unity.VisualScripting;
using UnityEngine;

public class CardGamePlayManager : MonoBehaviour
{
    // ������ ����
    public Transform playerParent;
    public CardGameView cardGameView;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;

    // ��ũ��Ʈ ����
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
        //Debug.Log($"NPC_Dice���� \n" +
        //    $"player.Length == {player.Length}");

        // �ݺ��������� �ֻ����� �ѹ� ���� �� �����, ��� ó���� ���� �� �ٽ� ȣ��Ǿ����
        for (int i = 0; i < player.Length; i++)
        {
            // �̹� �ֻ����� �������� �н�(Player Me�� �ݵ�� pass��)
            if (player[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}�� �̹� �ֻ����� ������");
                continue;
            }
            Debug.Log($"�ֻ����� ������ ���� �÷��̾� ���� == {4 - i}");
            Debug.Log($"{player[i].gameObject.name}�� �ֻ����� �������ϴ�");
            diceManager.RotateDice(player[i].gameObject);
            return; // �ֻ����� �ѹ� �������� �Լ��� ����
        }

        // �ֻ����� �� ���ȴµ� �� �Լ��� �����ߴٸ� ���� ó���� ��������
        


    }
}

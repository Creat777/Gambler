using PublicSet;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text playerMoneyViewText; // �÷��̾��� ���� ȭ�鿡 ǥ���� �ؽ�Ʈ
    private PlayerMoneyAnimation moneyAnimation;


    public struct PlayerStatus
    {
        public int hp; // ü��
        public int agility; // ��ø��
        public int hunger; // ���

        public int money; // ������

        public static PlayerStatus GetDefault()
        {
            return default(PlayerStatus);
        }
    }


    public PlayerStatus currentPlayerStatus { get; private set; }


    //public int current
    protected override void Awake()
    {
        base.Awake();
        currentPlayerStatus = PlayerStatus.GetDefault();
    }

    void Start()
    {
        playerMoneyViewText = GameManager.connector.playerMoneyView_Script.coinResult;
        moneyAnimation = GameManager.connector.playerMoneyView_Script.GetComponent<PlayerMoneyAnimation>();

        if(playerMoneyViewText == null)
        {
            Debug.LogAssertion($"{gameObject.name}�� playerMoneyViewText == null");
        }
        if(moneyAnimation == null)
        {
            Debug.LogAssertion($"{gameObject.name}�� moneyAnimation == null");
        }

        AddExp();
        AddItem();
        DoQuest();
    }


    /// <summary>
    /// �÷��̾ �����ϴ� ���ΰ����� �ʱ�ȭ
    /// </summary>
    /// <param name="setValue"></param>
    public void PlayerMoneySet(int setValue)
    {
        var update = currentPlayerStatus;
        update.money = setValue;
        currentPlayerStatus = update;

        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();
    }

    /// <summary>
    /// �÷��̾ �����ִ� ���� �߰����� ����
    /// </summary>
    /// <param name="Value"></param>
    public void AddPlayerMoney(int Value)
    {
        var update = currentPlayerStatus;
        update.money += Value;
        currentPlayerStatus = update;

        // ������ �ʱ�ȭ
        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();

        // ��ȭ�� �ִϸ��̼�
        if( Value > 0 )
        {
            moneyAnimation.PlaySequnce_PlayerMoneyPlus(Value);
        }
        else if( Value < 0 )
        {
            moneyAnimation.PlaySequnce_PlayerMoneyMinus(Value);
        }
    }



    public void AddExp()
    {

    }

    public void LevelUp()
    {

    }

    public void AddItem()
    {
        // PlayerPrefs �� ������ ȹ�� ���
    }

    public void DoQuest()
    {

    }
}

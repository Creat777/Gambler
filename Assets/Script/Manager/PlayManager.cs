using PublicSet;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text _playerMoneyViewText; // �÷��̾��� ���� ȭ�鿡 ǥ���� �ؽ�Ʈ
    public Text playerMoneyViewText
    {
        get 
        { 
            if(_playerMoneyViewText == null) _playerMoneyViewText = GameManager.connector.playerMoneyView_Script.coinResult;
            return _playerMoneyViewText; 
        }
    }


    private PlayerMoneyAnimation _moneyAnimation;
    public PlayerMoneyAnimation moneyAnimation
    {
        get
        {
            if (_moneyAnimation == null) _moneyAnimation = GameManager.connector.playerMoneyView_Script.GetComponent<PlayerMoneyAnimation>();
            return _moneyAnimation;
        }
    }


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
        AddExp();
        AddItem();
        DoQuest();
    }


    /// <summary>
    /// �÷��̾ �����ϴ� ���ΰ����� �ʱ�ȭ
    /// </summary>
    /// <param name="setValue"></param>
    public void SetPlayerMoney(int setValue)
    {
        var update = currentPlayerStatus;
        update.money = setValue;
        currentPlayerStatus = update;

        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();
    }

    /// <summary>
    /// �÷��̾ �����ִ� ���� �߰����� ����, �Ļ꿩�δ� ���� ��ũ��Ʈ�� TryMinusCoin���� �Ǻ�
    /// </summary>
    /// <param name="Value"></param>
    /// <returns>�Ļ꿩�θ� Ȯ��</returns>
    public void AddPlayerMoney(int Value)
    {
        PlayerStatus update = currentPlayerStatus;
        update.money += Value;
        currentPlayerStatus = update;

        // ������ �ʱ�ȭ
        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();

        // ��ȭ�� �ִϸ��̼�
        if (Value > 0) // AddCoin�� ���� ȣ��� ���
        {
            moneyAnimation.PlaySequnce_PlayerMoneyPlus(Value);
        }
        else if (Value < 0) // TryMinusCoin�� ���� ȣ��� ���
        {
            Value -= Value; // �����ǿ� ���� ���� ����� �ٲ�
            moneyAnimation.PlaySequnce_PlayerMoneyMinus(Value);
        }

        //if (currentPlayerStatus.money > 0)
        //{
            
        //}
        
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

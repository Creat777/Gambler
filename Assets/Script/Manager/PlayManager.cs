using PublicSet;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text _playerMoneyViewText; // �÷��̾��� ���� ȭ�鿡 ǥ���� �ؽ�Ʈ
    private PlayerMoneyAnimation _moneyAnimation;


    public sPlayerStatus currentPlayerStatus { get; private set; }
    public Text playerMoneyViewText
    {
        get
        {
            if (_playerMoneyViewText == null) _playerMoneyViewText =
                    (GameManager.connector as Connector_InGame).playerMoneyView_Script.coinResult;
            return _playerMoneyViewText;
        }
    }
    public PlayerMoneyAnimation moneyAnimation
    {
        get
        {
            if (_moneyAnimation == null) _moneyAnimation = (GameManager.connector as Connector_InGame).playerMoneyView_Script.GetComponent<PlayerMoneyAnimation>();
            return _moneyAnimation;
        }
    }
    //public int current
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        AddExp();
        AddItem();
        DoQuest();
    }

    public void SetPlayerStatus(sPlayerStatus value = default)
    {
        currentPlayerStatus = value;
        UpdateInterface();
    }

    public void UpdateInterface()
    {
        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();
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
        UpdateInterface();
    }

    /// <summary>
    /// �÷��̾ �����ִ� ���� �߰����� ����, �Ļ꿩�δ� ���� ��ũ��Ʈ�� TryMinusCoin���� �Ǻ�
    /// </summary>
    /// <param name="Value"></param>
    /// <returns>�Ļ꿩�θ� Ȯ��</returns>
    public void AddPlayerMoney(int Value)
    {
        sPlayerStatus update = currentPlayerStatus;
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
            Value = (-Value); // �����ǿ� ���� ���� ����� �ٲ�
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

using PublicSet;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text _playerMoneyViewText; // 플레이어의 돈을 화면에 표시할 텍스트
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
    /// 플레이어가 소지하는 코인개수를 초기화
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
    /// 플레이어가 갖고있는 돈에 추가값을 설정, 파산여부는 같은 스크립트의 TryMinusCoin에서 판별
    /// </summary>
    /// <param name="Value"></param>
    /// <returns>파산여부를 확인</returns>
    public void AddPlayerMoney(int Value)
    {
        sPlayerStatus update = currentPlayerStatus;
        update.money += Value;
        currentPlayerStatus = update;

        // 전광판 초기화
        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();

        // 변화량 애니메이션
        if (Value > 0) // AddCoin에 의해 호출된 경우
        {
            moneyAnimation.PlaySequnce_PlayerMoneyPlus(Value);
        }
        else if (Value < 0) // TryMinusCoin에 의해 호출된 경우
        {
            Value = (-Value); // 전광판에 띄우기 위해 양수로 바꿈
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
        // PlayerPrefs 에 아이템 획득 기록
    }

    public void DoQuest()
    {

    }
}

using PublicSet;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text playerMoneyViewText; // 플레이어의 돈을 화면에 표시할 텍스트
    private PlayerMoneyAnimation moneyAnimation;


    public struct PlayerStatus
    {
        public int hp; // 체력
        public int agility; // 민첩성
        public int hunger; // 허기

        public int money; // 소지금

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
            Debug.LogAssertion($"{gameObject.name}의 playerMoneyViewText == null");
        }
        if(moneyAnimation == null)
        {
            Debug.LogAssertion($"{gameObject.name}의 moneyAnimation == null");
        }

        AddExp();
        AddItem();
        DoQuest();
    }


    /// <summary>
    /// 플레이어가 소지하는 코인개수를 초기화
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
    /// 플레이어가 갖고있는 돈에 추가값을 설정
    /// </summary>
    /// <param name="Value"></param>
    public void AddPlayerMoney(int Value)
    {
        var update = currentPlayerStatus;
        update.money += Value;
        currentPlayerStatus = update;

        // 전광판 초기화
        playerMoneyViewText.text = "x" + currentPlayerStatus.money.ToString();

        // 변화량 애니메이션
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
        // PlayerPrefs 에 아이템 획득 기록
    }

    public void DoQuest()
    {

    }
}

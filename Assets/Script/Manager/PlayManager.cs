using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text playerMoneyViewText; // 플레이어의 돈을 화면에 표시할 텍스트


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


    public PlayerStatus currentPlayerStatus;


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

    private void FixedUpdate()
    {
        if(GameManager.Instance.currentScene == eScene.InGame)
        {
            PlayerMoneyUpdate();
        }
    }

    /// <summary>
    /// 현재 플레이어가 갖고있는 코인개수로 ui를 업데이트
    /// </summary>
    private void PlayerMoneyUpdate()
    {
        playerMoneyViewText = GameManager.Connector.playerMoneyView.transform.GetChild(1).gameObject.GetComponent<Text>();
        if(playerMoneyViewText != null)
        {
            playerMoneyViewText.text = "x"+currentPlayerStatus.money.ToString();
        }
        
    }

    /// <summary>
    /// 플레이어가 소지하는 코인개수를 초기화
    /// </summary>
    /// <param name="setValue"></param>
    public void PlayerMoneySet(int setValue)
    {
        currentPlayerStatus.money = setValue;
    }

    /// <summary>
    /// 플레이어가 갖고있는 돈에 추가값을 설정
    /// </summary>
    /// <param name="plusValue"></param>
    public void PlayerMoneyPlus(int plusValue)
    {
        currentPlayerStatus.money += plusValue;
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

    void Update()
    {
        
    }
}

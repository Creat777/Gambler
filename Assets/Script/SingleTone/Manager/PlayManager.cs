using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{
    private Text playerMoneyViewText;

    protected override void MakeSingleTone()
    {
        // 싱글톤 인스턴스 설정 및 중복 방지
        if (Instance == null)
        {
            Instance = this; // DontDestroyOnLoad를 제거했음
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public int currentCoin {  get; private set; }

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

    private void FixedUpdate()
    {
        PlayerMoneyUpdate();
    }

    /// <summary>
    /// 현재 플레이어가 갖고있는 코인개수로 ui를 업데이트
    /// </summary>
    private void PlayerMoneyUpdate()
    {
        if(playerMoneyViewText == null)
        {
            playerMoneyViewText = GameManager.Connector.playerMoneyView.transform.GetChild(1).gameObject.GetComponent<Text>();
        }

        playerMoneyViewText.text = currentCoin.ToString();
    }

    /// <summary>
    /// 플레이어가 소지하는 코인개수를 초기화
    /// </summary>
    /// <param name="setValue"></param>
    public void PlayerMoneySet(int setValue)
    {
        currentCoin = setValue;
    }

    /// <summary>
    /// 플레이어가 갖고있는 돈에 추가값을 설정
    /// </summary>
    /// <param name="plusValue"></param>
    public void PlayerMoneyPlus(int plusValue)
    {
        currentCoin += plusValue;
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

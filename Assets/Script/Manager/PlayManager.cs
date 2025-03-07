using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{

    private Text playerMoneyViewText; // �÷��̾��� ���� ȭ�鿡 ǥ���� �ؽ�Ʈ


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
    /// ���� �÷��̾ �����ִ� ���ΰ����� ui�� ������Ʈ
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
    /// �÷��̾ �����ϴ� ���ΰ����� �ʱ�ȭ
    /// </summary>
    /// <param name="setValue"></param>
    public void PlayerMoneySet(int setValue)
    {
        currentPlayerStatus.money = setValue;
    }

    /// <summary>
    /// �÷��̾ �����ִ� ���� �߰����� ����
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
        // PlayerPrefs �� ������ ȹ�� ���
    }

    public void DoQuest()
    {

    }

    void Update()
    {
        
    }
}

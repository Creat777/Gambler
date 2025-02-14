using UnityEngine;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{
    private Text playerMoneyViewText;

    protected override void MakeSingleTone()
    {
        // �̱��� �ν��Ͻ� ���� �� �ߺ� ����
        if (Instance == null)
        {
            Instance = this; // DontDestroyOnLoad�� ��������
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
    /// ���� �÷��̾ �����ִ� ���ΰ����� ui�� ������Ʈ
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
    /// �÷��̾ �����ϴ� ���ΰ����� �ʱ�ȭ
    /// </summary>
    /// <param name="setValue"></param>
    public void PlayerMoneySet(int setValue)
    {
        currentCoin = setValue;
    }

    /// <summary>
    /// �÷��̾ �����ִ� ���� �߰����� ����
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
        // PlayerPrefs �� ������ ȹ�� ���
    }

    public void DoQuest()
    {

    }

    void Update()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PublicSet;
using DG.Tweening;
using UnityEngine.UI;
using System;




public class GameManager : Singleton<GameManager>
{
    // �̱���� ��̱����� ������
    static private Connector _connector;
    static public Connector connector
    {
        get
        {
            if (_connector == null) Debug.LogAssertion("Ŀ���� ���� �ȵ���");
            return _connector;
        }
    }

    static public Connector_Lobby connector_Lobby
    {
        get
        {
            if (connector is Connector_Lobby) return (connector as Connector_Lobby);
            else { Debug.LogAssertion("Ŀ���� ĳ���� ���� : connector_Lobby"); return null; }
        }
    }
    static public Connector_InGame connector_InGame
    {
        get
        {
            if (connector is Connector_InGame) return (connector as Connector_InGame);
            else { Debug.LogAssertion("Ŀ���� ĳ���� ���� : connector_InGame"); return null; }
        }
    }


    // �����Ϳ��� ����
    public float defaultGamespeed;
    public int maxStage;

    // ��ũ��Ʈ���� ����
    public bool isGamePause {  get; private set; }
    public bool isCasinoGameView {  get; private set; }
    public float gameSpeed { get; private set; }

    public ePlayerSaveKey _currentPlayerSaveKey;
    public eScene _currentScene;
    public eMap _currentMap;
    public eStage _currentStage;
    [SerializeField] private int _currentRemainingPeriod;

    public ePlayerSaveKey currentPlayerSaveKey
    { get { return _currentPlayerSaveKey; } private set {  _currentPlayerSaveKey = value; } }
    public eScene currentScene
    { get { return _currentScene; } private set { _currentScene = value; } }
    public eMap currentMap
    { get { return _currentMap; } private set { _currentMap = value; } }
    public eStage currentStage
    { get { return _currentStage; } private set { _currentStage = value; } }
    public int currentRemainingPeriod 
    { get { return _currentRemainingPeriod; } private set { _currentRemainingPeriod = value; } }

    public void SetPlayerSaveKey(ePlayerSaveKey value)
    {
        currentPlayerSaveKey = value;
    }
    public void SetCurrentScene(eScene value)
    {
        currentScene = value;
        InitConnector();
        SceneLoadView();
    }
    public void SetCurrentMap(eMap value)
    {
        currentMap = value;
    }
    public void SetCurrentStage(eStage value)
    {
        currentStage = value;
    }
    public void SetRemainingPeriod(int value)
    {
        currentRemainingPeriod = value;
    }
    public void CountDownRemainingPeriod()
    {
        currentRemainingPeriod--;
    }

    public Dictionary<eStage, string> stageMessageDict;

    public void Init_StageMessageDict()
    {
        stageMessageDict = new Dictionary<eStage, string>();
        stageMessageDict.Add(eStage.Stage1, "STAGE 1\n���Ⱑ ��ü ����?");
        stageMessageDict.Add(eStage.Stage2, "STAGE 2\nī���뿡 �Լ�����");
    }

    protected override void Awake()
    {
        base.Awake();
        Init_StageMessageDict();
        Continue_theGame();
    }

    public void Pause_theGame()
    {
        gameSpeed = 0;
        isGamePause = true;
    }
    public void Continue_theGame()
    {
        gameSpeed = defaultGamespeed;
        isGamePause = false;
    }

    public void ChangeCardGameView(bool boolValue)
    {
        isCasinoGameView = boolValue;
    }

    public void InitConnector()
    {
        switch(currentScene)
        {
            case eScene.Title:{
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector>();
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector ���� ����");
                } break;
            case eScene.Lobby: { 
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_Lobby>();
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector_Lobby ���� ����");
                } break;
            case eScene.InGame: {
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_InGame>();
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector_InGame ���� ����");
                } break;
        }

        
    }

    

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("�׽�Ʈ ����");
            CallbackManager.Instance.EnterCasino();
        }
    }
#endif


    private void OnEnable()
    {
        // ���� �ε�� �� ȣ��� �Լ��� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ���� ����� �ݹ�����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SceneLoadView(Action LoadSceneCallback = null)
    {
        ProcessSceneView(true, Color.black, Color.clear, LoadSceneCallback);
    }

    public void SceneUnloadView(Action LoadSceneCallback)
    {
        ProcessSceneView(false, Color.clear, Color.black, LoadSceneCallback);
    }

    public void ProcessSceneView(bool isSceneLoad, Color startColor, Color targetColor, Action callback = null)
    {
        connector.blackView.SetActive(true);
        Image blackViewImage = connector.blackView.GetComponent<Image>();
        blackViewImage.color = startColor;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.3f)
                .Append(blackViewImage.DOColor(targetColor, 0.7f));

        if(isSceneLoad)
        {
            sequence.AppendCallback(() => _connector.blackView.SetActive(false));
        }
        if (callback != null)
        {
            sequence.AppendCallback(() => callback());
        }

        sequence.SetLoops(1);

        sequence.Play();
    }

    public void SetStage(eStage stageEnum)
    {
        currentStage = stageEnum;
    }

    public void NextStage()
    {
        currentStage++;
    }

    //public void PlaySequnce_StageAnimation()
    //{
    //    //Debug.Log("stage �ִϸ��̼� ����");

    //    // �̹��� Ȱ��ȭ
    //    connector_InGame.EventView.SetActive(true);

    //    // �̹��� ���� �ʱ�ȭ
    //    Image stateViewImage = connector_InGame.EventView.GetComponent<Image>();
    //    Color colorBack = new Color(1,1,1,0);
    //    stateViewImage.color = colorBack;

    //    // �̹��� ���� �ؽ�Ʈ �ʱ�ȭ
    //    Text StageViewText = connector_InGame.EventView.transform.GetChild(0).gameObject.GetComponent<Text>();
    //    StageViewText.text = StageMessageDict[currentStage];
    //    StageViewText.color = Color.clear;

    //    Sequence sequence = DOTween.Sequence();

    //    float intervalDelay = 0.5f; // �̺�Ʈ ������ ��������ȭ�� ���� ���ð�
    //    float startDelay = 1f; // ȭ�� ����ð�
    //    float middleDelay = 1; // �����ð�
    //    float endDelay = 1f; // ����ð�

    //    // ����
    //    sequence.AppendInterval(intervalDelay)

    //            .Append(stateViewImage.DOColor(Color.white, startDelay))
    //            .Join(StageViewText.DOColor(Color.black, startDelay))

    //            .AppendInterval(middleDelay)

    //            .Append(stateViewImage.DOColor(colorBack, endDelay))
    //            .Join(StageViewText.DOColor(Color.clear, endDelay))

    //            .AppendCallback(() =>
    //            {
    //                connector_InGame.EventView.SetActive(false);
    //            })

    //            .SetLoops(1);

    //    sequence.Play();
    //}

    private void StartNewGame()
    {
        // ���̴� 30�Ϻ��� �����ؼ� 0�� �Ǹ� ������ �����
        SetRemainingPeriod(30);

        // ������ 0���� �ʱ�ȭ
        PlayManager.Instance.SetPlayerStatus();

        connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse);
        SetStage(eStage.Stage1);
        SceneLoadView(
            () =>
            {
                PlayManager.Instance.StartPlayerMonologue();
            }
            );
    }

    private void ContinueGame()
    {
        // �����Ⱓ �ҷ�����
        SetRemainingPeriod(PlayerSaveManager.Instance.LoadRemainingPeriod(currentPlayerSaveKey));

        // �÷��̾� ���� �ҷ�����
        PlayManager.Instance.SetPlayerStatus(
            PlayerSaveManager.Instance.LoadPlayerStatus(currentPlayerSaveKey)
            );

        connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse);
        PlayerSaveManager.Instance.LoadStage(currentPlayerSaveKey); // ���⼭ ������ Set��
        PlayerSaveManager.Instance.LoadOpenedIconCount(currentPlayerSaveKey); // ���⼭ Set��
        PlayerSaveManager.Instance.LoadItems(currentPlayerSaveKey);
        PlayerSaveManager.Instance.LoadQuests(currentPlayerSaveKey);

        SceneLoadView(
            () =>
            {
                PlayManager.Instance.StartPlayerMonologue();
            }
            );
    }

    

    public void GameOver()
    {
        float delay = 2f;
        CallbackManager.Instance.PlaySequnce_BlackViewProcess(
            delay,
            () => connector_InGame.youLoseView_Script.gameObject.SetActive(true));
    }


    /// <summary>
    /// Awake -> OnEnable(SceneManager.sceneLoaded�� �߰�) -> ���ε� -> OnSceneLoaded -> Start
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        { 
            case 0: SetCurrentScene(eScene.Title);  break;
            case 1: SetCurrentScene(eScene.Lobby);  break;
            case 2:
                {
                    SetCurrentScene(eScene.InGame); 
                    switch (currentPlayerSaveKey)
                    {
                        case ePlayerSaveKey.None: StartNewGame(); break; // ���ν����ϱ� ���� ���
                        default: ContinueGame(); break; // �̾��ϱ� ���� ���
                    }
                }
                break;
        }
        
    }
}

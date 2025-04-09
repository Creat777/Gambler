using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PublicSet;
using DG.Tweening;
using UnityEngine.UI;
using System;




public class GameManager : Singleton<GameManager>
{
    // 싱글톤과 논싱글톤의 연결자
    static private Connector _connector;
    static public Connector connector
    {
        get
        {
            if (_connector == null) Debug.LogAssertion("커넥터 연결 안됐음");
            return _connector;
        }
    }

    static public Connector_Lobby connector_Lobby
    {
        get
        {
            if (connector is Connector_Lobby) return (connector as Connector_Lobby);
            else { Debug.LogAssertion("커넥터 캐스팅 실패 : connector_Lobby"); return null; }
        }
    }
    static public Connector_InGame connector_InGame
    {
        get
        {
            if (connector is Connector_InGame) return (connector as Connector_InGame);
            else { Debug.LogAssertion("커넥터 캐스팅 실패 : connector_InGame"); return null; }
        }
    }


    // 에디터에서 수정
    public float defaultGamespeed;
    public int maxStage;

    // 스크립트에서 수정
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

    //Dictionary<eStage, string> StageMessageDict;

    //public void Init_StageMessageDict()
    //{
    //    StageMessageDict = new Dictionary<eStage, string>();
    //    StageMessageDict.Add(eStage.Stage1, "STAGE 1\n여기가 대체 어디야?");
    //    StageMessageDict.Add(eStage.Stage2, "STAGE 2\n카지노에 입성하자");
    //}

    protected override void Awake()
    {
        base.Awake();
        //Init_StageMessageDict();
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
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector 연결 실패");
                } break;
            case eScene.Lobby: { 
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_Lobby>();
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector_Lobby 연결 실패");
                } break;
            case eScene.InGame: {
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_InGame>();
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector_InGame 연결 실패");
                } break;
        }

        
    }

    

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("테스트 시작");
            CallbackManager.Instance.EnterCasino();
        }
    }
#endif


    private void OnEnable()
    {
        // 씬이 로드될 때 호출될 함수를 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 게임 종료시 콜백제거
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
    //    //Debug.Log("stage 애니메이션 시작");

    //    // 이미지 활성화
    //    connector_InGame.EventView.SetActive(true);

    //    // 이미지 색깔 초기화
    //    Image stateViewImage = connector_InGame.EventView.GetComponent<Image>();
    //    Color colorBack = new Color(1,1,1,0);
    //    stateViewImage.color = colorBack;

    //    // 이미지 내부 텍스트 초기화
    //    Text StageViewText = connector_InGame.EventView.transform.GetChild(0).gameObject.GetComponent<Text>();
    //    StageViewText.text = StageMessageDict[currentStage];
    //    StageViewText.color = Color.clear;

    //    Sequence sequence = DOTween.Sequence();

    //    float intervalDelay = 0.5f; // 이벤트 종료후 스테이지화면 등장 대기시간
    //    float startDelay = 1f; // 화면 등장시간
    //    float middleDelay = 1; // 유지시간
    //    float endDelay = 1f; // 퇴장시간

    //    // 등장
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
        // 디데이는 30일부터 시작해서 0이 되면 게임이 종료됨
        SetRemainingPeriod(30);

        // 코인을 0으로 초기화
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
        // 남은기간 불러오기
        SetRemainingPeriod(PlayerSaveManager.Instance.LoadRemainingPeriod(currentPlayerSaveKey));

        // 플레이어 정보 불러오기
        PlayManager.Instance.SetPlayerStatus(
            PlayerSaveManager.Instance.LoadPlayerStatus(currentPlayerSaveKey)
            );

        connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse);
        PlayerSaveManager.Instance.LoadStage(currentPlayerSaveKey); // 여기서 데이터 Set함
        PlayerSaveManager.Instance.LoadOpenedIconCount(currentPlayerSaveKey); // 여기서 Set함
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
    /// Awake -> OnEnable(SceneManager.sceneLoaded에 추가) -> 씬로드 -> OnSceneLoaded -> Start
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
                        case ePlayerSaveKey.None: StartNewGame(); break; // 새로시작하기 누른 경우
                        default: ContinueGame(); break; // 이어하기 누른 경우
                    }
                }
                break;
        }
        
    }
}

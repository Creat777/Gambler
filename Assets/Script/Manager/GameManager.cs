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
        get { 
            if(_connector == null)
            {
                _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector>();
            }
            return _connector;
        }
    }

    // 에디터에서 수정
    public float defaultGamespeed;
    public int maxStage;

    // 스크립트에서 수정
    public bool isJoinGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public bool isCasinoGameView {  get; private set; }
    public float gameSpeed { get; private set; }

    public ePlayerSaveKey currentSaveKey;
    public eScene currentScene;
    public eMap currentMap;
    public eStage currentStage { get; set; }


    [SerializeField] private int month = 12;
    [SerializeField] private int day;
    public int Month { get { return month; } set { month = value; } }
    public int Day { get { return day; } set { day = value; } }

    Dictionary<eStage, string> StageMessageDict;

    public void Init_StageMessageDict()
    {
        StageMessageDict = new Dictionary<eStage, string>();
        StageMessageDict.Add(eStage.Stage1, "STAGE 1\n여기가 대체 어디야?");
        StageMessageDict.Add(eStage.Stage2, "STAGE 2\n카지노에 입성하자");
    }

    protected override void Awake()
    {
        base.Awake();
        Init_StageMessageDict();
        Continue_theGame();
    }

    public void Join_In_Game()
    {
        isJoinGame = true;
    }
    public void Out_Of_Game()
    {
        isJoinGame = false;
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
        Image blackViewImage = _connector.blackView.GetComponent<Image>();
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

    public void ChangeStage(eStage stageEnum)
    {
        currentStage = stageEnum;
    }

    public void NextStage()
    {
        currentStage++;
    }

    public void PlaySequnce_StageAnimation()
    {
        //Debug.Log("stage 애니메이션 시작");

        // 이미지 활성화
        connector.StageView.SetActive(true);

        // 이미지 색깔 초기화
        Image stateViewImage = connector.StageView.GetComponent<Image>();
        Color colorBack = new Color(1,1,1,0);
        stateViewImage.color = colorBack;

        // 이미지 내부 텍스트 초기화
        Text StageViewText = connector.StageView.transform.GetChild(0).gameObject.GetComponent<Text>();
        StageViewText.text = StageMessageDict[currentStage];
        StageViewText.color = Color.clear;

        Sequence sequence = DOTween.Sequence();

        float intervalDelay = 0.5f; // 이벤트 종료후 스테이지화면 등장 대기시간
        float startDelay = 1f; // 화면 등장시간
        float middleDelay = 1; // 유지시간
        float endDelay = 1f; // 퇴장시간

        // 등장
        sequence.AppendInterval(intervalDelay)

                .Append(stateViewImage.DOColor(Color.white, startDelay))
                .Join(StageViewText.DOColor(Color.black, startDelay))

                .AppendInterval(middleDelay)

                .Append(stateViewImage.DOColor(colorBack, endDelay))
                .Join(StageViewText.DOColor(Color.clear, endDelay))

                .AppendCallback(() =>
                {
                    connector.StageView.SetActive(false);
                })

                .SetLoops(1);

        sequence.Play();
    }

    private void StartNewGame()
    {
        // 게임을 저장하기 전엔 쓰레기 데이터 취급
        currentSaveKey = ePlayerSaveKey.None;
        PlayerPrefsManager.Instance.SetPlayerKeySet();

        // 게임시작은 12월 1일부터 시작
        Month = 12;
        Day = 1;

        // 코인을 0으로 초기화
        PlayManager.Instance.SetPlayerMoney(50);

        connector.map_Script.ChangeMapTo(eMap.InsideOfHouse);
        ChangeStage(eStage.Stage1);
        SceneLoadView(
            () =>
            {
                StartPlayerMonologue();
            }
            );
    }

    public void StartPlayerMonologue()
    {
        CallbackManager.Instance.TextWindowPopUp_Open();

        GameManager.connector.textWindowView_Script.StartTextWindow(eTextScriptFile.PlayerMonologue);

    }



    // 씬 로드 시 호출될 콜백 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬넘어갈 시 기존 커넥터의 연결을 초기화
        _connector = null;

        switch (scene.buildIndex)
        { 
            case 0:
                {
                    currentScene = eScene.Title;
                    SceneLoadView();
                }
                break;
            case 1:
                {
                    currentScene = eScene.Lobby;
                    SceneLoadView();
                }
                 break;
            case 2:
                {
                    currentScene = eScene.InGame;
                    // 일반적인 경우
                    {
                        SceneLoadView();
                    }
                    // 새로하기 시작하는 경우
                    {
                        SceneLoadView();
                        StartNewGame();
                    }
                }
                

                
                
                break;
        }

    }
}

using System.Collections;
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
    static private Connector connector;
    static public Connector Connector
    {
        get { 
            if(connector == null)
            {
                connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector>();
            }
            return connector;
        }
    }

    // 에디터에서 수정
    public float defaultGamespeed;
    public int maxStage;

    // 스크립트에서 수정
    public bool isJoinGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public float gameSpeed { get; private set; }
    public eScene currentScene;
    public eMap currentMap;
    public eStage currentStage;

    [SerializeField] private int month = 12;
    [SerializeField] private int day;
    public int __month { get { return month; } set { month = value; } }
    public int __day { get { return day; } set { day = value; } }

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

    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            StageAnimation(); 
        }
    }

    
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
        Connector.blackView.SetActive(true);
        Image blackViewImage = connector.blackView.GetComponent<Image>();
        blackViewImage.color = startColor;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.3f)
                .Append(blackViewImage.DOColor(targetColor, 0.7f));

        if(isSceneLoad)
        {
            sequence.AppendCallback(() => connector.blackView.SetActive(false));
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

    public void StageAnimation()
    {
        Debug.Log("stage 애니메이션 시작");

        // 이미지 활성화
        Connector.StageView.SetActive(true);

        // 이미지 색깔 초기화
        Image stateViewImage = Connector.StageView.GetComponent<Image>();
        Color colorBack = new Color(1,1,1,0);
        stateViewImage.color = colorBack;

        // 이미지 내부 텍스트 초기화
        Text StageViewText = Connector.StageView.transform.GetChild(0).gameObject.GetComponent<Text>();
        StageViewText.text = StageMessageDict[currentStage];
        StageViewText.color = Color.clear;

        Sequence sequence = DOTween.Sequence();

        float startDelay = 2f;
        float EndDelay = 2f;

        // 등장
        sequence.AppendInterval(1f)
                .Append(stateViewImage.DOColor(Color.white, startDelay))
                .Join(StageViewText.DOColor(Color.black, startDelay))
                .Append(stateViewImage.DOColor(colorBack, EndDelay))
                .Join(StageViewText.DOColor(Color.clear, EndDelay))
                .AppendCallback(() =>
                {
                    Connector.StageView.SetActive(false);
                });
                    sequence.SetLoops(1);

        sequence.Play();
    }

    private void StartNewGame()
    {
        Connector.map_Script.ChangeMapTo(eMap.InsideOfHouse);
        ChangeStage(eStage.Stage1);
        SceneLoadView(
            () =>
            {
                //Debug.Log("콜백 실행됐음");
                CallbackManager.Instance.TextWindowPopUp_Open();
                TextWindowView textViewScript = Connector.textWindowView.GetComponent<TextWindowView>();
                if (textViewScript != null)
                {
                    textViewScript.StartTestWindow(eTextType.PlayerMonologue, eCsvFile_PlayerMono.PlayerTutorial);
                }
            }
            );
    }

    // 씬 로드 시 호출될 콜백 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

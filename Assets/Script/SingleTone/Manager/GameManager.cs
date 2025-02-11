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
    public string[] gameStages { get; private set; }
    public eScene currentScene;
    public eMap currentMap;
    public eStage currentStage;

    [SerializeField] private int month = 12;
    [SerializeField] private int day;
    public int __month { get { return month; } set { month = value; } }
    public int __day { get { return day; } set { day = value; } }



    public string curruentGameStage {  get; private set; } // 현재 게임 진행정도를 확인하기 위한 변수

    protected override void Awake()
    {
        base.Awake();
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
        gameStages = new string[maxStage];
        for(int i = 0; i < maxStage; i++)
        {
            gameStages[i] = $"Stage{i + 1}";
        }
        curruentGameStage = gameStages[0];
    }

    // Update is called once per frame
    void Update()
    {
        
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
                        Connector.map_Script.ChangeMapTo(eMap.InsideOfHouse);
                        currentStage = eStage.Stage1;
                        SceneLoadView(
                            () =>
                            {
                                Debug.Log("콜백 실행됐음");
                                CallbackManager.Instance.TextWindowPopUp_Open();
                                TextWindowView textViewScript = Connector.textWindowView.GetComponent<TextWindowView>();
                                if (textViewScript != null)
                                {
                                    textViewScript.StartTestWindow(eTextType.PlayerMonologue, eCsvFile_PlayerMono.PlayerTutorial);
                                }
                            }
                            );
                        
                    }
                }
                

                
                
                break;
        }

    }
}

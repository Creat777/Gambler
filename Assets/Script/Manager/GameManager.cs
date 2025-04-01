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
        get { 
            if(_connector == null)
            {
                _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector>();
            }
            return _connector;
        }
    }

    // �����Ϳ��� ����
    public float defaultGamespeed;
    public int maxStage;

    // ��ũ��Ʈ���� ����
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
        StageMessageDict.Add(eStage.Stage1, "STAGE 1\n���Ⱑ ��ü ����?");
        StageMessageDict.Add(eStage.Stage2, "STAGE 2\nī���뿡 �Լ�����");
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
        //Debug.Log("stage �ִϸ��̼� ����");

        // �̹��� Ȱ��ȭ
        connector.StageView.SetActive(true);

        // �̹��� ���� �ʱ�ȭ
        Image stateViewImage = connector.StageView.GetComponent<Image>();
        Color colorBack = new Color(1,1,1,0);
        stateViewImage.color = colorBack;

        // �̹��� ���� �ؽ�Ʈ �ʱ�ȭ
        Text StageViewText = connector.StageView.transform.GetChild(0).gameObject.GetComponent<Text>();
        StageViewText.text = StageMessageDict[currentStage];
        StageViewText.color = Color.clear;

        Sequence sequence = DOTween.Sequence();

        float intervalDelay = 0.5f; // �̺�Ʈ ������ ��������ȭ�� ���� ���ð�
        float startDelay = 1f; // ȭ�� ����ð�
        float middleDelay = 1; // �����ð�
        float endDelay = 1f; // ����ð�

        // ����
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
        // ������ �����ϱ� ���� ������ ������ ���
        currentSaveKey = ePlayerSaveKey.None;
        PlayerPrefsManager.Instance.SetPlayerKeySet();

        // ���ӽ����� 12�� 1�Ϻ��� ����
        Month = 12;
        Day = 1;

        // ������ 0���� �ʱ�ȭ
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



    // �� �ε� �� ȣ��� �ݹ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���Ѿ �� ���� Ŀ������ ������ �ʱ�ȭ
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
                    // �Ϲ����� ���
                    {
                        SceneLoadView();
                    }
                    // �����ϱ� �����ϴ� ���
                    {
                        SceneLoadView();
                        StartNewGame();
                    }
                }
                

                
                
                break;
        }

    }
}

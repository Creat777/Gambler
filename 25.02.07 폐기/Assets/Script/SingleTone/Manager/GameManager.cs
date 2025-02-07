using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eScene
{
    Title,
    Lobby,
    InGame
}



public class GameManager : Singleton<GameManager>
{

    // �����Ϳ��� ����
    public float defaultGamespeed;
    public int maxStage;

    // ��ũ��Ʈ���� ����
    public bool isJoinGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public float gameSpeed { get; private set; }
    public string[] gameStages { get; private set; }
    public string currentPlayer;
    public eScene currentScene;
    public Map_InGame.eMap currentMap;
    public Connector_InGame connector;

    [SerializeField] private int month = 12;
    [SerializeField] private int day;
    public int __month { get { return month; } set { month = value; } }
    public int __day { get { return day; } set { day = value; } }



    public string curruentGameStage {  get; private set; } // ���� ���� ���������� Ȯ���ϱ� ���� ����

    protected override void Awake()
    {
        base.Awake();
        Continue_theGame();
        currentPlayer = string.Empty;
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
        // ���� �ε�� �� ȣ��� �Լ��� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ���� ����� �ݹ�����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �� �ε� �� ȣ��� �ݹ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        { 
            case 0:
                {
                    currentScene = eScene.Title; 
                }
                break;

            case 1:
                {
                    currentScene = eScene.Lobby; 
                }
                break;

            case 2:
                {
                    currentScene = eScene.InGame;
                    connector = GameObject.Find("Connector").GetComponent<Connector_InGame>();

                    // ���ν����� ���
                    if (currentPlayer == string.Empty)
                    {
                        // ������ġ�� �̵�
                        connector.map_InGame.GetComponent<Map_InGame>().ChangeMapToInSide();

                        // textwindow�� Ȱ��ȭ�ϰ� ���� ����
                        connector.textWindowView.SetActive(true);
                        connector.interfaceView.SetActive(false);
                        connector.textWindowView.GetComponent<TextWindowView>().StartTextWindow(TextWindowView.TextType.Monologue);
                    }
                    
                }
                break;

        }

    }
}

using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    // 에디터에서 수정
    public float defaultGamespeed;
    public int maxStage;

    // 스크립트에서 수정
    public bool isInGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public float gameSpeed { get; private set; }

    public string[] gameStages { get; private set; }


    //Dictionary<>

    // DoTO -> 어떤 오브젝트의 어떤 스테이지에서는 선택지가 나와야함


    public string curruentGameStage {  get; private set; } // 현재 게임 진행정도를 확인하기 위한 변수

    protected override void Awake()
    {
        base.Awake();
        Continue_theGame();
    }

    public void Join_In_Game()
    {
        isInGame = true;
    }
    public void Out_Of_Game()
    {
        isInGame = false;
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
}

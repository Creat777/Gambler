using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 에디터에서 수정
    
    private float defaultGamespeed;

    // 스크립트에서 수정
    public bool isInGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public float gameSpeed { get; private set; }
    public string gameStage {  get; private set; } // 현재 게임 진행정도를 확인하기 위한 변수

    protected override void Awake()
    {
        base.Awake();
        Unpause_theGame();
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
    public void Unpause_theGame()
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
        
    }
}

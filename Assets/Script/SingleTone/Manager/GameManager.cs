using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // �����Ϳ��� ����
    
    private float defaultGamespeed;

    // ��ũ��Ʈ���� ����
    public bool isInGame {  get; private set; }
    public bool isGamePause {  get; private set; }
    public float gameSpeed { get; private set; }
    public string gameStage {  get; private set; } // ���� ���� ���������� Ȯ���ϱ� ���� ����

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

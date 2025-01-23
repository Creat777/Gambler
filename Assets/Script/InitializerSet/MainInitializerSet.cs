using UnityEngine;
using UnityEngine.UI;

// Main씬으로 전환 되면 객체가 만들어지면서 모든 초기화를 담당함
public class MainInitializerSet : MonoBehaviour
{
    public GameObject canvas;
    public GameObject menuButton;
    public GameObject player;
    public GameObject scorePanel;
    public GameObject enemyBoss;

    void Start()
    {
        // 메모리Pool 초기화
        EffactPoolManager.Instance.InitializePool();
        EnemyPoolCarrier.Instance.InitializePool();
        EnemyPoolShooter.Instance.InitializePool();
        EnemyPoolSuicideBomber.Instance.InitializePool();
        CoinMemoryPool.Instance.InitializePool();
        BulletFactory.Instance.InitializePool();
        EnemyBulletFactory.Instance.InitializePool();

        // 버튼 콜백함수 추가
        menuButton.GetComponent<Button>().onClick.AddListener(SceneManager.Instance.Button_GoToMenu);
        GameManager.Instance.EnemyBoss = enemyBoss;

        GameManagerInit();
        BulletFactoryInit();
    }
    
    private void GameManagerInit()
    {
        // GameManager 에 canvas 연결
        GameManager.Instance.canvas = canvas;
        if (GameManager.Instance.canvas != null)
        {
            //Debug.Log("GameManager의 canvas 연결 성공");
        }
        else
        {
            Debug.LogError("GameManager의 canvas 연결 실패");
        }
        // GameManager에 scorePanel 연결 
        GameManager.Instance.scorePanel = scorePanel;

        // GameManager 내에 있는 클래스 초기화
        GameManager.Instance.InitCsvManager();
    }

    private void BulletFactoryInit()
    {
        // BulletFactory에 player 연결
        BulletFactory.Instance.player = player;

        // BulletFactory에 playerHeight및 playerWidth 초기화
        Bounds bounds = player.gameObject.GetComponent<SpriteRenderer>().sprite.bounds;
        if (bounds != null)
        {
            BulletFactory.Instance.playerHeight = bounds.size.y;
            BulletFactory.Instance.playerWidth = bounds.size.x;
        }
        else
        {
            Debug.LogError("player 스프라이트 못찾았음");
        }
    }
    void Update()
    {
        
    }
}

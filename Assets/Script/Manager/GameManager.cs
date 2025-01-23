using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CsvManager
{
    public string csvFileName;
    public List<List<float>> positionCsv;
    public bool isDone;
    public float doneDelay;


    public CsvManager(string csvName = null, float delay = 1)
    {
        positionCsv = new List<List<float>>();
        isDone = false;

        csvFileName = csvName;
        doneDelay = delay;
    }
}

public class GameManager : MonoBehaviour
{
    // edit에서 편집
    public int gameSpeed;
    [SerializeField] private float MaxTime;
    public float MaxTime_ { get => MaxTime; private set => MaxTime = value; }

    //[SerializeField] private float MaxTime;
    //public float MaxTime_
    //{
    //    get { return MaxTime; }
    //    private set { MaxTime = value; }
    //}

    // 스크립트에서 편집
    public int defualtGameSpeed;
    public float gameTime { get; set; }
    public bool isGameStart = false;
    public bool isGamePause = false;
    public bool isBossExist { get; set; }
    private bool isItemDeley { get; set; }

    public int gameDifficulty { get; set; }
    public string gameDifficulyKey { get; set; }
    public string BackgroundVolumeKey { get; set; }
    public string EffctVolumeKey { get; set; }
    public string[] playerRankKeys { get; set; }
    CsvManager[] EnemyCsvManagerArr; // csv파일(enemy) 관리를 위한 객체
    CsvManager[] ItemCsvManagerArr; // csv파일(item) 관리를 위한 객체
    public string playerID;

    public GameObject EnemyBoss;

    Dictionary<string, float> rankerMap;


    // MainInitializerSet에서 연결
    public GameObject canvas;
    public GameObject scorePanel;

    //싱글톤 객체
    public static GameManager Instance { get; private set; }
    void MakeSingleTone()
    {
        if (Instance == null)
        {
            Instance = this; // 스크립트 컴포넌트
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        // 기본 속성 초기화
        gameTime = 0;
        gameSpeed = defualtGameSpeed;
        isGameStart = true;
        isGamePause = false;

        if (canvas != null)
        {
            canvas.GetComponent<MainCanvas>().menuButton.SetActive(false);
            canvas.GetComponent<MainCanvas>().UI_Box.SetActive(true);
        }
        else
        {
            Debug.LogError("canvas 연결이 안됐음");
        }
    }

    public void LoadPlayerSetting()
    {
        // 게임 난이도 불러오기
        int loadDifficulty = PlayerInfoManager.Instance.LoadData(gameDifficulyKey, 3);
        gameDifficulty = loadDifficulty;
    }

    public void GameOver()
    {
        BackGroundMusic.Instance.LetsPlay();
        isGameStart = false;
        isBossExist = false;
        canvas.GetComponent<MainCanvas>().menuButton.SetActive(true);
        canvas.GetComponent<MainCanvas>().UI_Box.SetActive(false);

        RankScore();
    }

    public List<string> GetOrderedRanker(float newScore)
    {
        // 점수 목록에 기존 점수들 추가
        string[] RankersID = PlayerInfoManager.Instance.GetRankersID(playerRankKeys);
        float[] RankerScores = PlayerInfoManager.Instance.GetRankScores(RankersID);

        // dict 초기화 후 랭커를 맵핑
        rankerMap.Clear();
        for (int i = 0; i < RankersID.Length; i++)
        {
            if (i < RankerScores.Length)
            {
                rankerMap.TryAdd(RankersID[i], RankerScores[i]);
            }
            else
            {
                Debug.LogError("RankerScores out of range");
            }

        }

        if (rankerMap.ContainsKey(playerID))// 기존 랭커에 해당 아이디가 있는 경우
        {
            if (rankerMap[playerID] < newScore)  //새로운 점수가 더 크면 해당 점수를 저장
                rankerMap[playerID] = newScore;
        }
        else // 기존 랭커에 해당 아이디가 없는 경우
        {
            // 점수 목록에 새로운 점수 추가
            rankerMap.Add(playerID, newScore);
        }


        // 점수별로 정렬된 키값 찾기
        List<string> sortedKeys = rankerMap
            .OrderBy(pair => pair.Value) // 값(value)을 기준으로 정렬
            .Select(pair => pair.Key)   // 정렬된 키(key)만 가져옴
            .ToList();

        sortedKeys.RemoveAt(0); // 가장 작은 값 제거
        sortedKeys.Reverse(); // 내림차순 정렬

        return sortedKeys;
    }

    public void RankScore()
    {
        // 점수 목록에 새로운 점수 추가
        float totalScore = scorePanel.GetComponent<Score>().totalScore;


        // 개인 최고점수 저장
        if (PlayerInfoManager.Instance.LoadData(playerID, 0f) < totalScore)
        {
            PlayerInfoManager.Instance.SaveData(playerID, totalScore);
        }

        // 랭커 업데이트
        string[] newRankersID = GetOrderedRanker(totalScore).ToArray<string>();

        // 저장된 랭커들을 지우고 차례대로 삽입
        PlayerInfoManager.Instance.ResetAllRank(playerRankKeys);
        PlayerInfoManager.Instance.SetNewRank(playerRankKeys, newRankersID);

        //제대로 저장되었는지 확인
        float[] rankScore = PlayerInfoManager.Instance.GetRankScores(newRankersID);
        for (int i = 0; i < rankScore.Length; i++)
        {
            Debug.Log($"{playerRankKeys[i]}의 점수 : {rankScore[i]}");
        }
    }
    private void Awake()
    {
        MakeSingleTone();

        // 최대시간 설정
        MaxTime_ = 45;

        // 보스 유무
        isBossExist = false;

        // 모든 아이템이 겹치지 않도록 딜레이를 만듬
        isItemDeley = false;

        // 디폴트 속도값 저장
        defualtGameSpeed = gameSpeed = 8;

        // 각종 키 설정
        gameDifficulyKey = "option_GameDifficulty";
        BackgroundVolumeKey = "option_BackgroundVolumeKey";
        EffctVolumeKey = "option_EffctVolumeKey";
        playerRankKeys = new string[3];
        rankerMap = new Dictionary<string, float>();
        for (int i = 0; i < 3; i++)
        {
            playerRankKeys[i] = $"PlayerRank{i}";
        }
    }

    void Start()
    {
        LoadPlayerSetting();
    }

    public void InitCsvManager()
    {
        EnemyCsvManagerArr = new CsvManager[2];
        EnemyCsvManagerArr[0] = new CsvManager("Enemy_Carrier", 3.0f);
        EnemyCsvManagerArr[1] = new CsvManager("Enemy_Shooter", 5.0f);

        foreach (var CsvManager in EnemyCsvManagerArr)
        {
            TextAsset csvFile = Resources.Load<TextAsset>(CsvManager.csvFileName);
            PositionCsvLoading(csvFile, CsvManager.positionCsv);
            //CheckCsv(enemyCsvManager);
        }


        ItemCsvManagerArr = new CsvManager[3];
        ItemCsvManagerArr[0] = new CsvManager("Item_Coin_1", 5.0f + 3.0f);
        ItemCsvManagerArr[1] = new CsvManager("Item_Coin_2", 5.0f);
        ItemCsvManagerArr[2] = new CsvManager("Item_Coin_3", 5.0f + 3.0f);

        foreach (var CsvManager in ItemCsvManagerArr)
        {
            TextAsset csvFile = Resources.Load<TextAsset>(CsvManager.csvFileName);
            PositionCsvLoading(csvFile, CsvManager.positionCsv);
        }
    }


    public void PositionCsvLoading(TextAsset csvFile, List<List<float>> positionCsv)
    {
        if (csvFile != null)
        {
            // csv에서 x좌표 랜덤값을 부여
            float randomValue = UnityEngine.Random.Range(-2f, 3f);

            // csv파일을 \n 문자를 기준으로 분할
            string[] rows = csvFile.text.Split('\n');
            foreach (string row in rows) // 각행을 floatList로 바꿔서 csv 변수에 삽입
            {
                string[] fileds = row.Split(','); // ,을 기준으로 분할
                List<float> posCsv = new List<float>(fileds.Length); // 크기를 미리 설정

                foreach (string filed in fileds) // 각 원소는 x, y, z 성분
                {
                    try
                    {
                        if (filed == "random") // x좌표에만 적용
                        {
                            posCsv.Add(randomValue);
                        }
                        else
                        {
                            posCsv.Add(float.Parse(filed));
                        }
                    }
                    catch (FormatException)
                    {
                        Debug.LogError($"format이 맞지 않음: {filed}"); // 예외가 발생한 경우 스킵
                    }
                }
                positionCsv.Add(posCsv);
                //Debug.Log("csv파일 Load성공");
            }
        }
        else
        {
            Debug.Log("csv파일 Load실패");
        }
    }

    private void CheckCsv(CsvManager csvManager)
    {
        int row_num = 0;
        Debug.Log(csvManager.csvFileName + " : ");
        foreach (var row in csvManager.positionCsv)
        {
            Debug.Log($"[ {row_num + 1} ]행");
            int field_num = 0;
            foreach (var field in row)
            {
                Debug.Log($"{++field_num}열 " + field);
            }
            row_num++;
        }
    }

    public Vector3 FloatListToVector3(List<float> list)
    {
        Vector3 position = Vector3.zero;
        if (list.Count == 3)
        {
            position = new Vector3(list[0], list[1], list[2]);
            //Debug.Log(position);
        }
        else
        {
            Debug.Log("list to position 실패");
        }
        return position;
    }

    void Update()
    {
        if (isGameStart && isGamePause == false)
        {
            gameTime += Time.deltaTime; // 게임시간 체크

            // 일반 몬스터 객체 관리
            if (gameTime < MaxTime * (2.0f / 3.0f)) //
            {
                ManageEnemyCsvObject();
                //Debug.Log("에너미 관리!");
            }
                

            // 아이템은 겹치지 않게 등장
            if (isItemDeley == false && gameTime > 5f)
                StartCoroutine(ManageItemCsvObject());

            if (isBossExist == false && gameTime > MaxTime * (2.0f / 3.0f)) //
            {
                BackGroundMusic.Instance.BossEnter();
                EnemyBoss.SetActive(true);
                // 스타트 포지션 코루틴
                isBossExist = true;
            }
        }


        if (gameTime > MaxTime && isGameStart)
        {
            Debug.LogWarning($"gameTime : {gameTime} > MaxTime : {MaxTime}");
            GameOver();
        }

    }

    private void ManageEnemyCsvObject()
    {
        // 몬스터는 겹쳐도 상관없음
        foreach (var csvManager in EnemyCsvManagerArr)
        {
            if (!csvManager.isDone)
            {
                // 일정 시간마다 소환
                StartCoroutine(CsvCount(csvManager));
            }
        }


    }

    IEnumerator ManageItemCsvObject()
    {
        isItemDeley = true;

        // 아이템 랜덤 선택
        int size = ItemCsvManagerArr.Length;
        int index = UnityEngine.Random.Range(0, size);
        if (ItemCsvManagerArr[index].isDone == false) // 아이템이 나와있지 않으면
        {
            StartCoroutine(CsvCount(ItemCsvManagerArr[index]));
            yield return new WaitForSeconds(ItemCsvManagerArr[index].doneDelay);
        }
        isItemDeley = false;
    }

    IEnumerator CsvCount(CsvManager CsvManager)
    {
        string choice = CsvManager.csvFileName.Split('_')[0];
        if (choice == "Enemy")
        {
            CsvManager.isDone = true;
            PositioningCsv(CsvManager);
            float delayDifficulty = (1.0f / 2.0f) * (gameDifficulty - 1) + 1; // 값은 집합 {1.0, 1.5, 2.0, 2.5, 3.0}의 원소
            yield return new WaitForSeconds(CsvManager.doneDelay / delayDifficulty); // 난이도가 높을수록 더 빨리 소환됨
            Debug.Log($"난이도가 {gameDifficulty}일 때 몬스터 소환 딜레이 : " + CsvManager.doneDelay / delayDifficulty);
            CsvManager.isDone = false;
        }
        else if (choice == "Item")
        {
            PositioningCsv(CsvManager);
        }
    }




    private void PositioningCsv(CsvManager CsvManager)
    {
        if (CsvManager == null)
        {
            Debug.LogError("CsvManager 객체가 존재하지 않음");
            return;
        }

        if (CsvManager.positionCsv.Count != 0)
        {
            string fileFirstName = CsvManager.csvFileName.Split("_")[0];

            // Enemy 파일이면
            if (fileFirstName == "Enemy")
            {
                int spawnIndex = UnityEngine.Random.Range(0, CsvManager.positionCsv.Count);
                Vector3 Position = FloatListToVector3(CsvManager.positionCsv[spawnIndex]);
                GameObject enemy;
                if (CsvManager.csvFileName == "Enemy_Carrier")
                {
                    enemy = EnemyPoolCarrier.Instance.GetObject(Position);
                }
                else if (CsvManager.csvFileName == "Enemy_Shooter")
                {
                    enemy = EnemyPoolShooter.Instance.GetObject(Position);
                }
                else // 찾는 이름이 없을 때
                {
                    Debug.LogWarning("EnemydefaultName");
                }
                //Debug.Log($"Spawned {enemy.name} at {Position}");
            }

            // Item파일이면
            else if (fileFirstName == "Item")
            {
                GameObject item;
                if (CsvManager.csvFileName == "Item_Coin_1" ||
                    CsvManager.csvFileName == "Item_Coin_2" ||
                    CsvManager.csvFileName == "Item_Coin_3")
                {
                    for (int i = 0; i < CsvManager.positionCsv.Count; i++)
                    {
                        Vector3 position = FloatListToVector3(CsvManager.positionCsv[i]);
                        item = CoinMemoryPool.Instance.GetObject(position);
                    }
                }
                else // 찾는 이름이 없을 때
                {
                    Debug.LogWarning("defaultName");
                }
                //Debug.Log($"Spawned {enemy.name} at {spawnPosition}");
            }



        }

    }

}

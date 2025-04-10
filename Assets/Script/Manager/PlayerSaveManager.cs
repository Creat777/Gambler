using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 저장 자료구조
/// </summary>
public struct sItem
{
    public int id; // 내가 소유하고 있는 아이템 번호
    public eItemType type; // 아이템 시리얼 번호 - 아이콘, 아이템의 능력치

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        return $"{id}:{type}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
    public static sItem DataSplit(string data)
    {
        sItem item = new sItem();
        string[] parts = data.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out item.id) &&
            eItemType.TryParse(parts[1], out item.type))
        {
            return item;
        }

        // 값타입이 반환형인경우 각 속성이 기본값인 해당 값타입을 반환
            // int의 기본값은 0
            // float의 기본값은 0.0f
            // bool의 기본값은 false
        // 참조타입이 반환형인경우 기본값으로 null을 반환
        return default;
    }

    public override bool Equals(object obj)
    {
        return obj is sItem item &&
               id == item.id &&
               type == item.type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(id, type);
    }

    // 생성자
    public sItem(int id, eItemType serail)
    {
        this.id = id;
        type = serail;
        return;
    }

    public sItem(sItem item)
    {
        id = item.id;
        type = item.type;
        return;
    }
}

/// <summary>
/// 퀘스트 저장 자료구조
/// </summary>
public struct sQuest
{
    public int id; // 퀘스트 순서
    public eQuestType type; // 퀘스트 번호

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

        // 참조변수의 일부 데이터를 같이 저장
        return $"{id}:{type}:{questInfo.isComplete.ToString()}:{questInfo.hasReceivedReward.ToString()}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
    public static sQuest DataSplit(string data)
    {
        sQuest item = new sQuest();
        string[] parts = data.Split(':');

        if (parts.Length == 4 &&
            int.TryParse(parts[0], out item.id) &&
            eQuestType.TryParse(parts[1], out item.type)&&
            bool.TryParse(parts[2], out bool isComplete) &&
            bool.TryParse(parts[3], out bool hasReceivedReward)
            )
            
        {
            // 데이터를 불러오면서 참조변수의 데이터도 같이 복원
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(item.type);
            questInfo.isComplete = isComplete;
            questInfo.hasReceivedReward = hasReceivedReward;

            return item;
        }
        return default;
    }

    public override bool Equals(object obj)
    {
        return obj is sQuest quest &&
               id == quest.id &&
               type == quest.type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(id, type);
    }

    // 생성자
    public sQuest(int id, eQuestType type)
    {
        this.id = id;
        this.type = type;
        return;
    }

    public sQuest(sQuest quest)
    {
        id = quest.id;
        type = quest.type;
        return;
    }
}

/// <summary>
/// 플레이어 데이터 자료구조
/// </summary>
public struct sPlayerStatus
{
    public int hp; // 체력
    public int agility; // 민첩성
    public int hunger; // 허기
    public int coin; // 소지금



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{coin}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
    public static sPlayerStatus DataSplit(string data)
    {
        sPlayerStatus playerStatus = new sPlayerStatus();
        string[] parts = data.Split(':');

        if (parts.Length == 4 &&
            int.TryParse(parts[0], out playerStatus.hp) &&
            int.TryParse(parts[1], out playerStatus.agility) &&
            int.TryParse(parts[2], out playerStatus.hunger) &&
            int.TryParse(parts[3], out playerStatus.coin))
        {
            return playerStatus;
        }

        // 값타입이 반환형인경우 각 속성이 기본값인 해당 값타입을 반환
        // int의 기본값은 0
        // float의 기본값은 0.0f
        // bool의 기본값은 false
        // 참조타입이 반환형인경우 기본값으로 null을 반환
        return default;
    }

    public static sPlayerStatus defaultData
    {
        get
        {
            return default;
        }
        
    }

    public override bool Equals(object obj)
    {
        return obj is sPlayerStatus status &&
               hp == status.hp &&
               agility == status.agility &&
               hunger == status.hunger &&
               coin == status.coin;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(hp, agility, hunger, coin);
    }

    // == 연산자 재정의
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // 객체가 동일한지 체크
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.coin == right.coin;
    }

    // != 연산자 재정의
    public static bool operator !=(sPlayerStatus left, sPlayerStatus right)
    {
        return !(left == right);
    }

    // 생성자
    public sPlayerStatus(int hp, int agility, int hunger, int money)
    {
        this.hp = hp;
        this.agility = agility;
        this.hunger = hunger;
        this.coin = money;
    }

    public sPlayerStatus(sPlayerStatus status)
    {
        hp = status.hp;
        agility = status.agility;
        hunger = status.hunger;
        coin = status.coin;
    }
}

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    string currentPlayerSavaKey { get { return GameManager.Instance.currentPlayerSaveKey.ToString(); } }

    public const string defaultSaveKey_SavedDate = "savedDate"; // 데이터가 저장된 날짜와 시간을 저장
    public const string defaultSaveKey_Items = "SavedItems";
    public const string defaultSaveKey_Quests = "SavedQuests";
    public const string defaultSaveKey_RemainingPeriod = "SavedRemainingPeriod";
    public const string defaultSaveKey_Stage = "SavedStage";
    public const string defaultSaveKey_PlayerStatus = "SavedPlayerStatus";
    public const string defaultSaveKey_OpenedIconCount = "SavedOpenedIconCount";

    public string currentPlayerSaveKey_SavedDate
    { get { return (currentPlayerSavaKey + defaultSaveKey_SavedDate); } }
    public string currentPlayerSaveKey_Items
    { get { return (currentPlayerSavaKey + defaultSaveKey_Items); } }
    public string currentPlayerSaveKey_Quests
    { get { return (currentPlayerSavaKey + defaultSaveKey_Quests); } }
    public string currentPlayerSaveKey_RemainingPeriod
    { get { return (currentPlayerSavaKey + defaultSaveKey_RemainingPeriod); } }
    public string currentPlayerSaveKey_Stage
    { get { return (currentPlayerSavaKey + defaultSaveKey_Stage); } }
    public string currentPlayerSaveKey_PlayerStatus
    { get { return (currentPlayerSavaKey + defaultSaveKey_PlayerStatus); } }

    public string currentPlayerSaveKey_OpenedIconCount
    { get { return (currentPlayerSavaKey + defaultSaveKey_OpenedIconCount); } }

    public void SaveTotalData()
    {
        Debug.Log($"SaveData -> currentPlayer : {currentPlayerSavaKey}");

        // 저장되는 날짜
        string SavedDate = DateTime.Now.ToString("yyyy년 MM월 dd일:HH시 mm분 ss초");

        // 결과 출력
        Debug.Log($"저장 시간 : {SavedDate}");

        SaveData(currentPlayerSaveKey_SavedDate, SavedDate);

        //LoadItems(ePlayerSaveKey.None);
        string itemData = string.Join(",", ItemManager.ItemHashSet);
        SaveData(currentPlayerSaveKey_Items, itemData);

        //LoadQuests(ePlayerSaveKey.None);
        string questData = string.Join(",", QuestManager.questHashSet);
        SaveData(currentPlayerSaveKey_Quests, questData);

        int remainingPeriod = GameManager.Instance.currentRemainingPeriod;
        SaveData(currentPlayerSaveKey_RemainingPeriod, remainingPeriod);

        int numStage = (int)GameManager.Instance.currentStage;
        SaveData(currentPlayerSaveKey_Stage, numStage);

        string playerStatus = PlayManager.Instance.currentPlayerStatus.ToString();
        SaveData(currentPlayerSaveKey_PlayerStatus, playerStatus);

        int openedIconCount = GameManager.connector_InGame.iconView_Script.OpenedIconCount;
        SaveData(currentPlayerSaveKey_OpenedIconCount, openedIconCount);
    }
    

    private void OnDisable()
    {
        DeleteDefaultData();
    }

    /// <summary>
    /// 게임 종료시 더미데이터는 모두 삭제
    /// </summary>
    private void DeleteDefaultData()
    {
        string playerKey = ePlayerSaveKey.None.ToString();
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_SavedDate);
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Items);
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Quests);
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_RemainingPeriod);
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Stage);
        PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_PlayerStatus);
    }

    

    
    

    protected override void Awake()
    {
        base.Awake();
        
        

        //currentPlayerSaveKey_Items = ePlayerSaveKey.None.ToString()+defaultSaveKey_Items;
    }

    

    

    //public void ItemsDataSave(int itemId, eItemType serialNum, Action<sItem> middleCallback, Action endCallback)
    //{
    //    // 더미데이터에 저장된 데이터를 불러오고
    //    LoadItems(ePlayerSaveKey.None);

    //    // 입력된 아이템 코드에 맞는 임시데이터를 생성
    //    sItem newItem = new sItem(itemId, serialNum);

    //    // 아이템 획득, 삭제에 따른
    //    middleCallback(newItem);

    //    // 새로운 아이템 목록을 PlayerPrefs를 이용하여 레지스트리에 저장
    //    // Join : 구분자(첫번째 변수)로 데이터목록을 묶음
    //    // Join의 두번째 인수에서 ToString 메소드가 사용됨
    //    SaveData(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items, string.Join(",", ItemManager.ItemHashSet));

    //    endCallback();

    //    // 아이템에 변화가 생겼으니 인벤토리창을 새로고침
    //    GameManager.connector_InGame.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>().RefreshPopUp();
    //}

    public string LoadSavedDate(ePlayerSaveKey saveKey)
    {
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_SavedDate, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("저장기록이 없음");
            return string.Empty;
        }
        else
        {
            Debug.Log("저장기록이 있음");
            return savedData;
        }
    }

    public HashSet<sItem> LoadItems(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_Items, string.Empty);
        
        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("저장된 데이터가 없음 : LoadItems");
            return new HashSet<sItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            sItem item = sItem.DataSplit(itemString);

            // 데이터가 잘못된경우 패스
            if (item.id == 0 && item.type == 0)
            {
                continue;
            }

            // 해쉬셋에서 이미 존재하는 데이터는 무시됨
            else if (ItemManager.ItemHashSet.Contains(item))
            {
                continue;
            }

            // 올바른 데이터를 해쉬셋에 추가
            else
            {
                ItemManager.ItemHashSet.Add(item);
            }
        }

        Debug.Log("데이터 로딩 성공 : LoadItems");
        return ItemManager.ItemHashSet;
    }

    public HashSet<sQuest> LoadQuests(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_Quests, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            return new HashSet<sQuest>();
        }

        string[] questStrings = savedData.Split(',');

        foreach (string questString in questStrings)
        {
            // id : serail
            sQuest quest = sQuest.DataSplit(questString);

            // 데이터가 잘못된경우 패스
            if (quest.id == 0 && quest.type == 0)
            {
                continue;
            }

            // 해쉬셋에서 이미 존재하는 데이터는 무시됨
            else if (QuestManager.questHashSet.Contains(quest))
            {
                continue;
            }

            // 올바른 데이터를 해쉬셋에 추가
            else
            {
                QuestManager.questHashSet.Add(quest);
            }
        }
        return QuestManager.questHashSet;
    }
    public int LoadRemainingPeriod(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_RemainingPeriod, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0)
        {
            Debug.LogWarning("데이터가 비었음 : RemainingPeriod");
            return savedData;
        }
        else if(savedData < 0)
        {
            Debug.LogAssertion("의도하지 않은 데이터 저장 : RemainingPeriod");
            return savedData;
        }
        else
        {
            Debug.Log("데이터 로딩 성공 : LoadRemainingPeriod");
            return savedData;
        }
    }

    public eStage LoadStage(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_Stage, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0)
        {
            Debug.LogWarning("데이터가 비었음 : LoadStage");
            return eStage.Stage1;
        }
        else
        {
            if(Enum.IsDefined(typeof(eStage), savedData))
            {
                Debug.Log("데이터 로딩 성공 : LoadStage");
                GameManager.Instance.SetStage((eStage)savedData);
                return (eStage)savedData;
            }
            else
            {
                Debug.LogError("데이터 로딩 실패 : LoadStage");
                return eStage.Defualt;
            }
        }
    }
    public sPlayerStatus LoadPlayerStatus(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_PlayerStatus, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("데이터가 비었음 : sPlayerStatus");
            return sPlayerStatus.defaultData;
        }

        // id : serail
        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // 데이터가 잘못된경우 패스
        if (playerStatus == sPlayerStatus.defaultData)
        {
            Debug.LogAssertion("데이터 저장 오류 : sPlayerStatus");
        }

        return playerStatus;
    }

    public int LoadOpenedIconCount(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_OpenedIconCount, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0)
        {
            Debug.LogWarning("데이터가 비었음 : LoadOpenedIconCount");
            return savedData;
        }
        else
        {
            Debug.Log("데이터 로딩 성공 : LoadOpenedIconCount");
            GameManager.connector_InGame.iconView_Script.SetOpendIconCount(savedData);
            return savedData;
        }
    }

    

#if UNITY_EDITOR
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("모든 저장된 데이터 삭제");
        }
    }
#endif

    private void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
    private void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
    private void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    
    public int LoadData(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public float LoadData(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public string LoadData(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
}

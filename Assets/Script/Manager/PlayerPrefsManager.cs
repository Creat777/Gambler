using PublicSet;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 아이템을 저장하는 자료구조
public struct sItem
{
    public int Id_inInventory; // 내가 소유하고 있는 아이템 번호
    public eItemType type; // 아이템 시리얼 번호 - 아이콘, 아이템의 능력치

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        return $"{Id_inInventory}:{type}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
    public static sItem DataSplit(string data)
    {
        sItem item = new sItem();
        string[] parts = data.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out item.Id_inInventory) &&
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
               Id_inInventory == item.Id_inInventory &&
               type == item.type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id_inInventory, type);
    }

    // 생성자
    public sItem(int id, eItemType serail)
    {
        Id_inInventory = id;
        type = serail;
        return;
    }

    public sItem(sItem item)
    {
        Id_inInventory = item.Id_inInventory;
        type = item.type;
        return;
    }
}

public struct sQuest
{
    public int ID;

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        return $"{ID}";
    }


    // 생성자
    public sQuest(int id)
    {
        this.ID = id;
        return;
    }

    public sQuest(sQuest quest)
    {
        this.ID = quest.ID;
        return;
    }
}

public struct sPlayerStatus
{
    public int hp; // 체력
    public int agility; // 민첩성
    public int hunger; // 허기
    public int money; // 소지금



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{money}";
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
            int.TryParse(parts[3], out playerStatus.money))
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

    public static sPlayerStatus GetDefault()
    {
        return default;
    }

    public override bool Equals(object obj)
    {
        return obj is sPlayerStatus status &&
               hp == status.hp &&
               agility == status.agility &&
               hunger == status.hunger &&
               money == status.money;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(hp, agility, hunger, money);
    }

    // == 연산자 재정의
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // 객체가 동일한지 체크
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.money == right.money;
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
        this.money = money;
    }

    public sPlayerStatus(sPlayerStatus status)
    {
        hp = status.hp;
        agility = status.agility;
        hunger = status.hunger;
        money = status.money;
    }
}

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    string currentPlayerSavaKey;

    private const string defaultSaveKey_Items = "SavedItems";
    public string currentPlayerSaveKey_Items;

    private const string defaultSaveKey_Quests = "SavedQuests";
    public string currentPlayerSaveKey_Quests;

    private const string defaultSaveKey_RemainingPeriod = "SavedRemainingPeriod";
    public string currentPlayerSaveKey_RemainingPeriod;

    private const string defaultSaveKey_PlayerStatus = "SavedPlayerStatus";
    public string currentPlayerSaveKey_PlayerStatus;

    public void SetPlayerKeySet()
    {
        // 현재 플레이어 정보를 가져옴
        currentPlayerSavaKey = GameManager.Instance.currentPlayerSaveKey.ToString();
        Debug.Log($"currentPlayer : {currentPlayerSavaKey}");

        Set_CurrentPlayerSaveKey_Items();
        Set_CurrentPlayerSaveKey_Quests();
        Set_CurrentPlayerSaveKey_RemainingPeriod();
        Set_CurrentPlayerSaveKey_PlayerStatus();
    }
    private void Set_CurrentPlayerSaveKey_Items()
    {
        // 아이템키를 업데이트하고 그 아이템키에 현재 아이템 데이터를 저장
        currentPlayerSaveKey_Items = currentPlayerSavaKey + defaultSaveKey_Items;
        SaveData(currentPlayerSaveKey_Items, string.Join(",", Player_Items));

        // 업데이트 전에 사용되었던 더미데이터는 모두 삭제
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString()+defaultSaveKey_Items);
    }

    private void Set_CurrentPlayerSaveKey_Quests()
    {
        currentPlayerSaveKey_Quests = currentPlayerSavaKey + defaultSaveKey_Quests;
        SaveData(currentPlayerSaveKey_Quests, string.Join(",", Player_Quests));

        // 업데이트 전에 사용되었던 더미데이터는 모두 삭제
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Quests);
    }
    private void Set_CurrentPlayerSaveKey_RemainingPeriod()
    {
        currentPlayerSaveKey_RemainingPeriod = currentPlayerSavaKey + defaultSaveKey_RemainingPeriod;
        SaveData(currentPlayerSaveKey_RemainingPeriod, GameManager.Instance.D_day);

        // 업데이트 전에 사용되었던 더미데이터는 모두 삭제
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_RemainingPeriod);
    }
    private void Set_CurrentPlayerSaveKey_PlayerStatus()
    {
        currentPlayerSaveKey_PlayerStatus = currentPlayerSavaKey + defaultSaveKey_PlayerStatus;
        SaveData(currentPlayerSaveKey_PlayerStatus, PlayManager.Instance.currentPlayerStatus);

        // 업데이트 전에 사용되었던 더미데이터는 모두 삭제
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_PlayerStatus);
    }

    private void OnDisable()
    {
        DeleteDefaultData();
    }

    // 플레이어가 저장하지 않은 경우
    private void DeleteDefaultData()
    {
        // 업데이트 전에 사용되었던 더미데이터는 모두 삭제
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items);
    }

    // 현재 플레이어가 소유하고있는 아이템
    // HashSet(집합) : 중복되는 데이터는 무시함
    // 참조타입을 자료형 파라미터에 넣을경우 참조(주소)를 비교하여 실 데이터가 중복될 수 있음
    public HashSet<sItem> Player_Items { get; private set; }
    public HashSet<sQuest> Player_Quests { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        Player_Items = new HashSet<sItem>();

        currentPlayerSaveKey_Items = ePlayerSaveKey.None.ToString()+defaultSaveKey_Items;
    }

    public void SaveData(string key, object value)
    {
        // 해당 key 리셋
        //PlayerPrefs.DeleteKey(key); 

        // 해당 key에 Value를 저장
        // 같은 키에 서로다른 자료형을 저장하는 경우 마지막에 저장된 데이터만 유효함
        if (value is int)
        {
            PlayerPrefs.SetInt(key, (int)value);
        }
        else if (value is float)
        {
            PlayerPrefs.SetFloat(key, (float)value);
        }
        else if (value is string)
        {
            PlayerPrefs.SetString(key, (string)value);
        }
        else
        {
            Debug.LogError("지원되지 않는 데이터 타입입니다.");
            return;
        }

        PlayerPrefs.Save();
        //Debug.Log($"Data saved : {key} = {value}");
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

    public void PlayerLoseItem(sItem item)
    {
        Debug.Log($"item {item.ToString()} 삭제 시도");
        ItemsDataSaveProcess(item.Id_inInventory, item.type,
            (sItem newItem) =>
            {
                // 지금 입력된 아이템이 존재하는지 여부를 판단
                if (Player_Items.Contains(newItem) == false)
                {
                    // 존재하지 않는 버그 아이템이라면 실행을 취소하고 inventory 업데이트
                    // DoTo 인벤토리 업데이트
                    Debug.LogWarning($"Item {item.Id_inInventory}은 존재하지 않는 데이터");
                    return;
                }
                // 존재하는 아이템이면 목록에서 제거
                Player_Items.Remove(newItem);
            },
            () => Debug.Log($"Item {item.Id_inInventory} 제거 성공함")
            );
    }

    // 새로운 아이템 정보를 저장하기 위한 함수
    public void PlayerGetItem(eItemType itemType)
    {
        int itemId = GetNewLastId();

        ItemsDataSaveProcess(itemId, itemType,
            (sItem newItem) =>
            {
                // 지금 입력된 아이템이 존재하는지 여부를 판단
                if (Player_Items.Contains(newItem))
                {
                    Debug.LogWarning($"Item {itemId} is already saved.");
                    return;
                }
                // 존재하지 않는다면 아이템 목록에 추가
                Player_Items.Add(newItem);
            },
            ()=>Debug.Log($"Item {itemId} 저장 성공함.")
            );
        //
    }

    public void ItemsDataSaveProcess(int itemId, eItemType serialNum, Action<sItem> middleCallback, Action endCallback)
    {
        // 기존 저장된 데이터를 불러오고
        LoadItems();

        // 입력된 아이템 코드에 맞는 임시데이터를 생성
        sItem newItem = new sItem(itemId, serialNum);

        // 아이템 획득, 삭제에 따른
        middleCallback(newItem);

        // 새로운 아이템 목록을 PlayerPrefs를 이용하여 레지스트리에 저장
        // Join : 구분자(첫번째 변수)로 데이터목록을 묶음
        // Join의 두번째 인수에서 ToString 메소드가 사용됨

        SaveData(currentPlayerSaveKey_Items, string.Join(",", Player_Items));
        //PlayerPrefs.SetString(savedItemsKey, string.Join(",", Player_Items));
        //PlayerPrefs.Save();

        endCallback();

        // 아이템에 변화가 생겼으니 인벤토리창을 새로고침
        (GameManager.connector as Connector_InGame).
            popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>().RefreshPopUp();
    }

    public HashSet<sItem> LoadItems()
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(currentPlayerSaveKey_Items, string.Empty);
        
        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            return new HashSet<sItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            sItem item = sItem.DataSplit(itemString);

            // 데이터가 잘못된경우 패스
            if (item.Id_inInventory == 0 && item.type == 0)
            {
                continue;
            }

            // 해쉬셋에서 이미 존재하는 데이터는 무시됨
            else if (Player_Items.Contains(item))
            {
                continue;
            }

            // 올바른 데이터를 해쉬셋에 추가
            else
            {
                Player_Items.Add(item);
            }
        }
        return Player_Items;
    }

    public sPlayerStatus LoadPlayerStatus()
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(currentPlayerSaveKey_PlayerStatus, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("데이터가 비었습니다 : sPlayerStatus");
            return new sPlayerStatus();
        }

        // id : serail
        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // 데이터가 잘못된경우 패스
        if (playerStatus == sPlayerStatus.GetDefault())
        {
            Debug.LogAssertion("데이터 저장 오류 : sPlayerStatus");
        }

        return playerStatus;
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId에서 반환되는 id : {LastitemID + 1}");
        return LastitemID + 1;
    }

    public int GetLastItemId()
    {
        LoadItems();

        if (Player_Items.Count == 0)
        {
            Debug.Log("저장된 데이터가 없음");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach (sItem item in Player_Items)
        {
            if (item.Id_inInventory > maxItemNumber)
            {
                maxItemNumber = item.Id_inInventory;
            }
        }

        return maxItemNumber;
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
}

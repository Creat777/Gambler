using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� �ڷᱸ��
/// </summary>
public struct sItem
{
    public int id; // ���� �����ϰ� �ִ� ������ ��ȣ
    public eItemType type; // ������ �ø��� ��ȣ - ������, �������� �ɷ�ġ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        return $"{id}:{type}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
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

        // ��Ÿ���� ��ȯ���ΰ�� �� �Ӽ��� �⺻���� �ش� ��Ÿ���� ��ȯ
            // int�� �⺻���� 0
            // float�� �⺻���� 0.0f
            // bool�� �⺻���� false
        // ����Ÿ���� ��ȯ���ΰ�� �⺻������ null�� ��ȯ
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

    // ������
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
/// ����Ʈ ���� �ڷᱸ��
/// </summary>
public struct sQuest
{
    public int id; // ����Ʈ ����
    public eQuestType type; // ����Ʈ ��ȣ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

        // ���������� �Ϻ� �����͸� ���� ����
        return $"{id}:{type}:{questInfo.isComplete.ToString()}:{questInfo.hasReceivedReward.ToString()}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
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
            // �����͸� �ҷ����鼭 ���������� �����͵� ���� ����
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

    // ������
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
/// �÷��̾� ������ �ڷᱸ��
/// </summary>
public struct sPlayerStatus
{
    public int hp; // ü��
    public int agility; // ��ø��
    public int hunger; // ���
    public int coin; // ������



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{coin}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
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

        // ��Ÿ���� ��ȯ���ΰ�� �� �Ӽ��� �⺻���� �ش� ��Ÿ���� ��ȯ
        // int�� �⺻���� 0
        // float�� �⺻���� 0.0f
        // bool�� �⺻���� false
        // ����Ÿ���� ��ȯ���ΰ�� �⺻������ null�� ��ȯ
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

    // == ������ ������
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // ��ü�� �������� üũ
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.coin == right.coin;
    }

    // != ������ ������
    public static bool operator !=(sPlayerStatus left, sPlayerStatus right)
    {
        return !(left == right);
    }

    // ������
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

    public const string defaultSaveKey_SavedDate = "savedDate"; // �����Ͱ� ����� ��¥�� �ð��� ����
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

        // ����Ǵ� ��¥
        string SavedDate = DateTime.Now.ToString("yyyy�� MM�� dd��:HH�� mm�� ss��");

        // ��� ���
        Debug.Log($"���� �ð� : {SavedDate}");

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
    /// ���� ����� ���̵����ʹ� ��� ����
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
    //    // ���̵����Ϳ� ����� �����͸� �ҷ�����
    //    LoadItems(ePlayerSaveKey.None);

    //    // �Էµ� ������ �ڵ忡 �´� �ӽõ����͸� ����
    //    sItem newItem = new sItem(itemId, serialNum);

    //    // ������ ȹ��, ������ ����
    //    middleCallback(newItem);

    //    // ���ο� ������ ����� PlayerPrefs�� �̿��Ͽ� ������Ʈ���� ����
    //    // Join : ������(ù��° ����)�� �����͸���� ����
    //    // Join�� �ι�° �μ����� ToString �޼ҵ尡 ����
    //    SaveData(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items, string.Join(",", ItemManager.ItemHashSet));

    //    endCallback();

    //    // �����ۿ� ��ȭ�� �������� �κ��丮â�� ���ΰ�ħ
    //    GameManager.connector_InGame.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>().RefreshPopUp();
    //}

    public string LoadSavedDate(ePlayerSaveKey saveKey)
    {
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_SavedDate, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("�������� ����");
            return string.Empty;
        }
        else
        {
            Debug.Log("�������� ����");
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
            Debug.LogWarning("����� �����Ͱ� ���� : LoadItems");
            return new HashSet<sItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            sItem item = sItem.DataSplit(itemString);

            // �����Ͱ� �߸��Ȱ�� �н�
            if (item.id == 0 && item.type == 0)
            {
                continue;
            }

            // �ؽ��¿��� �̹� �����ϴ� �����ʹ� ���õ�
            else if (ItemManager.ItemHashSet.Contains(item))
            {
                continue;
            }

            // �ùٸ� �����͸� �ؽ��¿� �߰�
            else
            {
                ItemManager.ItemHashSet.Add(item);
            }
        }

        Debug.Log("������ �ε� ���� : LoadItems");
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

            // �����Ͱ� �߸��Ȱ�� �н�
            if (quest.id == 0 && quest.type == 0)
            {
                continue;
            }

            // �ؽ��¿��� �̹� �����ϴ� �����ʹ� ���õ�
            else if (QuestManager.questHashSet.Contains(quest))
            {
                continue;
            }

            // �ùٸ� �����͸� �ؽ��¿� �߰�
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
            Debug.LogWarning("�����Ͱ� ����� : RemainingPeriod");
            return savedData;
        }
        else if(savedData < 0)
        {
            Debug.LogAssertion("�ǵ����� ���� ������ ���� : RemainingPeriod");
            return savedData;
        }
        else
        {
            Debug.Log("������ �ε� ���� : LoadRemainingPeriod");
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
            Debug.LogWarning("�����Ͱ� ����� : LoadStage");
            return eStage.Stage1;
        }
        else
        {
            if(Enum.IsDefined(typeof(eStage), savedData))
            {
                Debug.Log("������ �ε� ���� : LoadStage");
                GameManager.Instance.SetStage((eStage)savedData);
                return (eStage)savedData;
            }
            else
            {
                Debug.LogError("������ �ε� ���� : LoadStage");
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
            Debug.LogWarning("�����Ͱ� ����� : sPlayerStatus");
            return sPlayerStatus.defaultData;
        }

        // id : serail
        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // �����Ͱ� �߸��Ȱ�� �н�
        if (playerStatus == sPlayerStatus.defaultData)
        {
            Debug.LogAssertion("������ ���� ���� : sPlayerStatus");
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
            Debug.LogWarning("�����Ͱ� ����� : LoadOpenedIconCount");
            return savedData;
        }
        else
        {
            Debug.Log("������ �ε� ���� : LoadOpenedIconCount");
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
            Debug.Log("��� ����� ������ ����");
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

using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� �ڷᱸ��
/// </summary>
public struct sItem
{
    public int Id_inInventory; // ���� �����ϰ� �ִ� ������ ��ȣ
    public eItemType type; // ������ �ø��� ��ȣ - ������, �������� �ɷ�ġ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        return $"{Id_inInventory}:{type}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
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
               Id_inInventory == item.Id_inInventory &&
               type == item.type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id_inInventory, type);
    }

    // ������
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

/// <summary>
/// ����Ʈ ���� �ڷᱸ��
/// </summary>
public struct sQuest
{
    public int ID;

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        return $"{ID}";
    }


    // ������
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

/// <summary>
/// �÷��̾� ������ �ڷᱸ��
/// </summary>
public struct sPlayerStatus
{
    public int hp; // ü��
    public int agility; // ��ø��
    public int hunger; // ���
    public int money; // ������



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{money}";
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
            int.TryParse(parts[3], out playerStatus.money))
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
               money == status.money;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(hp, agility, hunger, money);
    }

    // == ������ ������
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // ��ü�� �������� üũ
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.money == right.money;
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
    string currentPlayerSavaKey { get { return GameManager.Instance.currentPlayerSaveKey.ToString(); } }

    public const string defaultSaveKey_SavedDate = "savedDate"; // �����Ͱ� ����� ��¥�� �ð��� ����
    public const string defaultSaveKey_Items = "SavedItems";
    public const string defaultSaveKey_Quests = "SavedQuests";
    public const string defaultSaveKey_RemainingPeriod = "SavedRemainingPeriod";
    public const string defaultSaveKey_PlayerStatus = "SavedPlayerStatus";

    public string currentPlayerSaveKey_SavedDate
    { get { return (currentPlayerSavaKey + defaultSaveKey_SavedDate); } }
    public string currentPlayerSaveKey_Items
    { get { return (currentPlayerSavaKey + defaultSaveKey_Items); } }
    public string currentPlayerSaveKey_Quests
    { get { return (currentPlayerSavaKey + defaultSaveKey_Quests); } }
    public string currentPlayerSaveKey_RemainingPeriod
    { get { return (currentPlayerSavaKey + defaultSaveKey_RemainingPeriod); } }
    public string currentPlayerSaveKey_PlayerStatus
    { get { return (currentPlayerSavaKey + defaultSaveKey_PlayerStatus); } }

    public void SaveTotalData()
    {
        Debug.Log($"SaveData -> currentPlayer : {currentPlayerSavaKey}");

        // ����Ǵ� ��¥
        string SavedDate = DateTime.Now.ToString("yyyy�� MM�� dd��:HH�� mm�� ss��");

        // ��� ���
        Debug.Log($"���� �ð� : {SavedDate}");

        // ���峯¥�� ���̵����͸� ���� �÷��̾� �����Ϳ� ����
        SaveData(currentPlayerSaveKey_SavedDate, SavedDate);
        SaveData(currentPlayerSaveKey_Items, LoadItems(ePlayerSaveKey.None));
        SaveData(currentPlayerSaveKey_Quests, LoadQuests(ePlayerSaveKey.None));
        SaveData(currentPlayerSaveKey_RemainingPeriod, LoadRemainingPeriod(ePlayerSaveKey.None));
        SaveData(currentPlayerSaveKey_PlayerStatus, LoadPlayerStatus(ePlayerSaveKey.None));
    }
    

    private void OnDisable()
    {
        DeleteDefaultData();
    }

    // �÷��̾ �������� ���� ���
    private void DeleteDefaultData()
    {
        // ���� ����� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_SavedDate);
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items);
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Quests);
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_RemainingPeriod);
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_PlayerStatus);
    }

    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
    public HashSet<sItem> Player_Items { get; private set; }

    /// <summary>
    /// �Ϸ�Ǿ��� ����Ʈ�� ������ ��� ����Ʈ
    /// </summary>
    public HashSet<sQuest> Player_Quests { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        Player_Items = new HashSet<sItem>();
        Player_Quests = new HashSet<sQuest>();

        //currentPlayerSaveKey_Items = ePlayerSaveKey.None.ToString()+defaultSaveKey_Items;
    }

    private void SaveData(string key, object value)
    {
        // �ش� key ����
        //PlayerPrefs.DeleteKey(key); 

        // �ش� key�� Value�� ����
        // ���� Ű�� ���δٸ� �ڷ����� �����ϴ� ��� �������� ����� �����͸� ��ȿ��
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
            Debug.LogError("�������� �ʴ� ������ Ÿ���Դϴ�.");
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
        Debug.Log($"item {item.ToString()} ���� �õ�");
        ItemsDataSaveProcess(item.Id_inInventory, item.type,
            (sItem newItem) =>
            {
                // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
                if (Player_Items.Contains(newItem) == false)
                {
                    // �������� �ʴ� ���� �������̶�� ������ ����ϰ� inventory ������Ʈ
                    // DoTo �κ��丮 ������Ʈ
                    Debug.LogWarning($"Item {item.Id_inInventory}�� �������� �ʴ� ������");
                    return;
                }
                // �����ϴ� �������̸� ��Ͽ��� ����
                Player_Items.Remove(newItem);
            },
            () => Debug.Log($"Item {item.Id_inInventory} ���� ������")
            );
    }

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(eItemType itemType)
    {
        int itemId = GetNewLastId();

        ItemsDataSaveProcess(itemId, itemType,
            (sItem newItem) =>
            {
                // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
                if (Player_Items.Contains(newItem))
                {
                    Debug.LogWarning($"Item {itemId} is already saved.");
                    return;
                }
                // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
                Player_Items.Add(newItem);
            },
            ()=>Debug.Log($"Item {itemId} ���� ������.")
            );
        //
    }

    public void ItemsDataSaveProcess(int itemId, eItemType serialNum, Action<sItem> middleCallback, Action endCallback)
    {
        // ���̵����Ϳ� ����� �����͸� �ҷ�����
        LoadItems(ePlayerSaveKey.None);

        // �Էµ� ������ �ڵ忡 �´� �ӽõ����͸� ����
        sItem newItem = new sItem(itemId, serialNum);

        // ������ ȹ��, ������ ����
        middleCallback(newItem);

        // ���ο� ������ ����� PlayerPrefs�� �̿��Ͽ� ������Ʈ���� ����
        // Join : ������(ù��° ����)�� �����͸���� ����
        // Join�� �ι�° �μ����� ToString �޼ҵ尡 ����


        SaveData(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items, string.Join(",", Player_Items));

        endCallback();

        // �����ۿ� ��ȭ�� �������� �κ��丮â�� ���ΰ�ħ
        (GameManager.connector as Connector_InGame).
            popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>().RefreshPopUp();
    }

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
            return new HashSet<sItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            sItem item = sItem.DataSplit(itemString);

            // �����Ͱ� �߸��Ȱ�� �н�
            if (item.Id_inInventory == 0 && item.type == 0)
            {
                continue;
            }

            // �ؽ��¿��� �̹� �����ϴ� �����ʹ� ���õ�
            else if (Player_Items.Contains(item))
            {
                continue;
            }

            // �ùٸ� �����͸� �ؽ��¿� �߰�
            else
            {
                Player_Items.Add(item);
            }
        }
        return Player_Items;
    }
    public HashSet<sQuest> LoadQuests(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_Quests, 0);
        return null;
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

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId���� ��ȯ�Ǵ� id : {LastitemID + 1}");
        return LastitemID + 1;
    }

    public int GetLastItemId()
    {
        LoadItems(ePlayerSaveKey.None);

        if (Player_Items.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
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
            Debug.Log("��� ����� ������ ����");
        }
    }
#endif
}

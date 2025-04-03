using PublicSet;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �������� �����ϴ� �ڷᱸ��
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
        // ���� �÷��̾� ������ ������
        currentPlayerSavaKey = GameManager.Instance.currentPlayerSaveKey.ToString();
        Debug.Log($"currentPlayer : {currentPlayerSavaKey}");

        Set_CurrentPlayerSaveKey_Items();
        Set_CurrentPlayerSaveKey_Quests();
        Set_CurrentPlayerSaveKey_RemainingPeriod();
        Set_CurrentPlayerSaveKey_PlayerStatus();
    }
    private void Set_CurrentPlayerSaveKey_Items()
    {
        // ������Ű�� ������Ʈ�ϰ� �� ������Ű�� ���� ������ �����͸� ����
        currentPlayerSaveKey_Items = currentPlayerSavaKey + defaultSaveKey_Items;
        SaveData(currentPlayerSaveKey_Items, string.Join(",", Player_Items));

        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString()+defaultSaveKey_Items);
    }

    private void Set_CurrentPlayerSaveKey_Quests()
    {
        currentPlayerSaveKey_Quests = currentPlayerSavaKey + defaultSaveKey_Quests;
        SaveData(currentPlayerSaveKey_Quests, string.Join(",", Player_Quests));

        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Quests);
    }
    private void Set_CurrentPlayerSaveKey_RemainingPeriod()
    {
        currentPlayerSaveKey_RemainingPeriod = currentPlayerSavaKey + defaultSaveKey_RemainingPeriod;
        SaveData(currentPlayerSaveKey_RemainingPeriod, GameManager.Instance.D_day);

        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_RemainingPeriod);
    }
    private void Set_CurrentPlayerSaveKey_PlayerStatus()
    {
        currentPlayerSaveKey_PlayerStatus = currentPlayerSavaKey + defaultSaveKey_PlayerStatus;
        SaveData(currentPlayerSaveKey_PlayerStatus, PlayManager.Instance.currentPlayerStatus);

        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_PlayerStatus);
    }

    private void OnDisable()
    {
        DeleteDefaultData();
    }

    // �÷��̾ �������� ���� ���
    private void DeleteDefaultData()
    {
        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSaveKey_Items);
    }

    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
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
        // ���� ����� �����͸� �ҷ�����
        LoadItems();

        // �Էµ� ������ �ڵ忡 �´� �ӽõ����͸� ����
        sItem newItem = new sItem(itemId, serialNum);

        // ������ ȹ��, ������ ����
        middleCallback(newItem);

        // ���ο� ������ ����� PlayerPrefs�� �̿��Ͽ� ������Ʈ���� ����
        // Join : ������(ù��° ����)�� �����͸���� ����
        // Join�� �ι�° �μ����� ToString �޼ҵ尡 ����

        SaveData(currentPlayerSaveKey_Items, string.Join(",", Player_Items));
        //PlayerPrefs.SetString(savedItemsKey, string.Join(",", Player_Items));
        //PlayerPrefs.Save();

        endCallback();

        // �����ۿ� ��ȭ�� �������� �κ��丮â�� ���ΰ�ħ
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

    public sPlayerStatus LoadPlayerStatus()
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(currentPlayerSaveKey_PlayerStatus, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("�����Ͱ� ������ϴ� : sPlayerStatus");
            return new sPlayerStatus();
        }

        // id : serail
        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // �����Ͱ� �߸��Ȱ�� �н�
        if (playerStatus == sPlayerStatus.GetDefault())
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
        LoadItems();

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

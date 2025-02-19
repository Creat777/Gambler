using PublicSet;
using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

// �������� �����ϴ� �ڷᱸ��
public struct sItem
{
    public int Id_inInventory; // ���� �����ϰ� �ִ� ������ ��ȣ
    public eItemSerialNumber serialNumber; // ������ �ø��� ��ȣ - ������, �������� �ɷ�ġ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        return $"{Id_inInventory}:{serialNumber}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
    public static sItem DataSplit(string data)
    {
        string[] parts = data.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int id) &&
            eItemSerialNumber.TryParse(parts[1], out eItemSerialNumber serial)
                )
        {
            return new sItem(id, serial);
        }

        // ��Ÿ���� ��ȯ���ΰ�� �� �Ӽ��� �⺻���� �ش� ��Ÿ���� ��ȯ
            // int�� �⺻���� 0
            // float�� �⺻���� 0.0f
            // bool�� �⺻���� false
        // ����Ÿ���� ��ȯ���ΰ�� �⺻������ null�� ��ȯ
        return default;
    }

    // ������
    public sItem(int id, eItemSerialNumber serail)
    {
        Id_inInventory = id;
        serialNumber = serail;
        return;
    }

    public sItem(sItem item)
    {
        Id_inInventory = item.Id_inInventory;
        serialNumber = item.serialNumber;
        return;
    }
}

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    private const string defaultSavedItemsKey = "SavedItems";
    public string currentPlayerSaveItemKey;

    public void SetPlayerKeySet()
    {
        SetPlayerItemKey();
    }
    private void SetPlayerItemKey()
    {
        // ���� �÷��̾� ������ ������
        string currentPlayer = GameManager.Instance.currentSaveKey.ToString();
        Debug.Log($"currentPlayer : {currentPlayer}");

        // ������Ű�� ������Ʈ�ϰ� �� ������Ű�� ���� ������ �����͸� ����
        currentPlayerSaveItemKey = currentPlayer + defaultSavedItemsKey;
        SaveData(defaultSavedItemsKey, string.Join(",", Player_Items));

        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString()+defaultSavedItemsKey);
    }

    private void OnDisable()
    {
        DeleteDefaultData();
    }

    // �÷��̾ �������� ���� ���
    private void DeleteDefaultData()
    {
        // ������Ʈ ���� ���Ǿ��� ���̵����ʹ� ��� ����
        PlayerPrefs.DeleteKey(ePlayerSaveKey.None.ToString() + defaultSavedItemsKey);
    }

    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
    public HashSet<sItem> Player_Items { get; private set; }

    

    protected override void Awake()
    {
        base.Awake();
        Player_Items = new HashSet<sItem>();

        currentPlayerSaveItemKey = ePlayerSaveKey.None.ToString()+defaultSavedItemsKey;
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
        ItemSaveProcess(item.Id_inInventory, item.serialNumber,
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
    public void PlayerGetItem(eItemSerialNumber serialNum)
    {
        int itemId = GetNewLastId();

        ItemSaveProcess(itemId, serialNum,
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

    public void ItemSaveProcess(int itemId, eItemSerialNumber serialNum, Action<sItem> middleCallback, Action endCallback)
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

        SaveData(currentPlayerSaveItemKey, string.Join(",", Player_Items));
        //PlayerPrefs.SetString(savedItemsKey, string.Join(",", Player_Items));
        //PlayerPrefs.Save();

        endCallback();

        // �����ۿ� ��ȭ�� �������� �κ��丮â�� ���ΰ�ħ
        GameManager.Connector.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>().RefreshInventory();
    }

    public HashSet<sItem> LoadItems()
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(currentPlayerSaveItemKey, string.Empty);
        
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
            if (item.Id_inInventory == 0 && item.serialNumber == 0)
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("��� ����� ������ ����");
        }
    }
}

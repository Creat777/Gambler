using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private const string savedItemsKey = "SavedItems";

    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
    public HashSet<sItem> Player_Items { get; private set; }

    

    protected override void Awake()
    {
        base.Awake();
        Player_Items = new HashSet<sItem>();
    }

    public void SaveDate()
    {
        
    }

    public void PlayerLoseItem(sItem item)
    {
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
            () => Debug.Log($"Item {item.Id_inInventory} used successfully.")
            );
    }

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(int itemId, eItemSerialNumber serialNum)
    {
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
            ()=>Debug.Log($"Item {itemId} saved successfully.")
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
        PlayerPrefs.SetString(savedItemsKey, string.Join(",", Player_Items));
        PlayerPrefs.Save();

        endCallback();
    }

    public HashSet<sItem> LoadItems()
    {
        string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
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

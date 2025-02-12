using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �������� �����ϴ� �ڷᱸ��
public struct Item
{
    public int Id_inInventory; // ���� �����ϰ� �ִ� ������ ��ȣ
    public int serialNumber; // ������ �ø��� ��ȣ - ������, �������� �ɷ�ġ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        return $"{Id_inInventory}:{serialNumber}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
    public static Item Decoding(string data)
    {
        string[] parts = data.Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int id) &&
            int.TryParse(parts[1], out int serial)
                )
        {
            return new Item(id, serial);
        }

        // ��Ÿ���� ��ȯ���ΰ�� �� �Ӽ��� �⺻���� �ش� ��Ÿ���� ��ȯ
            // int�� �⺻���� 0
            // float�� �⺻���� 0.0f
            // bool�� �⺻���� false
        // ����Ÿ���� ��ȯ���ΰ�� �⺻������ null�� ��ȯ
        return default;
    }

    // ������
    public Item(int id, int serail)
    {
        Id_inInventory = id;
        serialNumber = serail;
        return;
    }
}

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    private const string savedItemsKey = "SavedItems";

    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
    public HashSet<Item> Player_Items { get; private set; }

    

    protected override void Awake()
    {
        base.Awake();
        Player_Items = new HashSet<Item>();
    }

    public void SaveDate()
    {
        
    }

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(int itemId, int serialNum)
    {
        // ���� ����� �����͸� �ҷ�����
        LoadItems();

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        Item newItem = new Item(itemId, serialNum);
        if (Player_Items.Contains(newItem))
        {
            Debug.Log($"Item {itemId} is already saved.");
            return;
        }

        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        Player_Items.Add(newItem);


        // ���ο� ������ ����� PlayerPrefs�� �̿��Ͽ� ������Ʈ���� ����
        // Join : ������(ù��° ����)�� �����͸���� ����
        // Join�� �ι�° �μ����� ToString �޼ҵ尡 ����
        PlayerPrefs.SetString(savedItemsKey, string.Join(",", Player_Items)); 
        PlayerPrefs.Save();

        Debug.Log($"Item {itemId} saved successfully.");
    }

    public HashSet<Item> LoadItems()
    {
        string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            return new HashSet<Item>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            Item item = Item.Decoding(itemString);

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

        foreach (Item item in Player_Items)
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

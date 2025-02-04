using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    private const string ItemsKey = "SavedItems";

    // �������� �����ϴ� �ڷᱸ��
    public class Item
    {
        public int Id_inInventory; // ���� �����ϰ� �ִ� ������ ��ȣ
        public int Type; // ������ ���� ��ȣ - ������, �������� �ɷ�ġ

        // ������ ������ ���� string���� ��ȯ
        public string Encoding()
        {
            return $"{Id_inInventory}:{Type}";
        }

        // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
        public static Item Decoding(string data)
        {
            string[] parts = data.Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int id) &&
                int.TryParse(parts[1], out int type))
            {
                return new Item(id, type);
            }

            // ������� �⺻���� ��ü�� ��ȯ (Id_inInventory : 0, Type : 0)
            return default;
        }

        public Item(int id, int type)
        {
            Id_inInventory = id;
            Type = type;
            return;
        }

        ~Item()
        {
            return;
        }
    }

    HashSet<Item> items = new HashSet<Item>();

    public void SaveDate()
    {

    }

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void SaveItem(int itemId, int itemType)
    {
        // ���� ����� �����͸� �ҷ�����
        HashSet<Item> savedItems = LoadItems();

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        Item newItem = new Item(itemId, itemType);
        if (savedItems.Contains(newItem))
        {
            Debug.Log($"Item {itemId} is already saved.");
            return;
        }

        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        savedItems.Add(newItem);

        // ���ο� ������ ����� PlayerPrefs�� �̿��Ͽ� ������Ʈ���� ����
        PlayerPrefs.SetString(ItemsKey, string.Join(",", savedItems)); // Join : ������(ù��° ����)�� �����͸���� ����
        PlayerPrefs.Save();

        Debug.Log($"Item {itemId} saved successfully.");
    }

    HashSet<Item> LoadItems()
    {
        string savedData = PlayerPrefs.GetString(ItemsKey, string.Empty);

        if (string.IsNullOrEmpty(savedData))
        {
            return new HashSet<Item>();
        }

        // id1:type1,id2:type2 ....
        string[] itemStrings = savedData.Split(',');


        foreach (string itemString in itemStrings)
        {
            Item item = Item.Decoding(itemString);

            // �Ѵ� 0�̸� �߸��� ������
            if (item.Id_inInventory == 0 && item.Type == 0) continue;

            // �ùٸ� ������ ����
            else items.Add(item);
        }

        return items;
    }
}

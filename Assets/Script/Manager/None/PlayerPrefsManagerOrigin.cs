using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManagerOrigin : MonoBehaviour
{
    private const string ItemsKey = "SavedItems";

    // 아이템을 저장하는 구조체
    public struct Item
    {
        public int Id; // 내가 소유하고 있는 아이템 번호
        public int ItemKey; // 아이템 종류 번호 - 아이콘, 아이템의 능력치

        public string Encoding()
        {
            return $"{Id}:{ItemKey}";
        }

        public static Item FromString(string data)
        {
            string[] parts = data.Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int id) &&
                int.TryParse(parts[1], out int type))
            {
                return new Item { Id = id, ItemKey = type };
            }

            // 모든멤버가 기본값인 객체를 반환 (Id_inInventory : 0, ItemKey : 0)
            return default;
        }
    }

    HashSet<Item> items = new HashSet<Item>();

    void Start()
    {
        
    }

    public int LastSavedItemCheck()
    {
        int LastitemID = GetLastItemNumber();
        return LastitemID + 1;
    }

    public void SaveItem(int itemId, int itemType)
    {
        HashSet<Item> savedItems = LoadItems();

        Item newItem = new Item { Id = itemId, ItemKey = itemType };

        if ( savedItems.Contains(newItem))
        {
            Debug.Log($"Item {itemId} is already saved.");
            return;
        }

        savedItems.Add(newItem);
        PlayerPrefs.SetString(ItemsKey, string.Join(",", savedItems));
        PlayerPrefs.Save();

        Debug.Log($"Item {itemId} saved successfully.");
    }

    public HashSet<Item> GetItemListInfo()
    {
        if (items.Count == 0)
            LoadItems();

        foreach (Item itemInfo in items)
        {
            Debug.Log("saved itemID : " + itemInfo.Id);
        }

        return items;
    }

    HashSet<Item> LoadItems()
    {
        string savedData = PlayerPrefs.GetString(ItemsKey, string.Empty);

        if(string.IsNullOrEmpty(savedData))
        {
            return new HashSet<Item>();
        }

        string[] itemStrings = savedData.Split(',');
        

        foreach(string itemString in itemStrings)
        {
            Item item = Item.FromString(itemString);
            if(item.Id != 0 && item.ItemKey != 0)
            {
                items.Add(item);
            }
        }

        return items;
    }

    public int GetLastItemNumber()
    {
        HashSet<Item> savedItems = LoadItems();

        if(savedItems.Count == 0)
        {
            Debug.LogWarning("No Player_Items are saved.");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach(Item item in savedItems)
        {
            Debug.Log("saved itemID : " + item.Id);

            if(item.Id > maxItemNumber)
            {
                maxItemNumber = item.Id;
            }
        }

        Debug.Log($"Last saved item number: {maxItemNumber}");
        return maxItemNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    private const string ItemsKey = "SavedItems";

    // 아이템을 저장하는 자료구조
    public class Item
    {
        public int Id_inInventory; // 내가 소유하고 있는 아이템 번호
        public int Type; // 아이템 종류 번호 - 아이콘, 아이템의 능력치

        // 데이터 저장을 위해 string으로 변환
        public string Encoding()
        {
            return $"{Id_inInventory}:{Type}";
        }

        // string으로 저장했던 정보를 사용가능한 데이터로 변환
        public static Item Decoding(string data)
        {
            string[] parts = data.Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int id) &&
                int.TryParse(parts[1], out int type))
            {
                return new Item(id, type);
            }

            // 모든멤버가 기본값인 객체를 반환 (Id_inInventory : 0, Type : 0)
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

    // 새로운 아이템 정보를 저장하기 위한 함수
    public void SaveItem(int itemId, int itemType)
    {
        // 기존 저장된 데이터를 불러오고
        HashSet<Item> savedItems = LoadItems();

        // 지금 입력된 아이템이 존재하는지 여부를 판단
        Item newItem = new Item(itemId, itemType);
        if (savedItems.Contains(newItem))
        {
            Debug.Log($"Item {itemId} is already saved.");
            return;
        }

        // 존재하지 않는다면 아이템 목록에 추가
        savedItems.Add(newItem);

        // 새로운 아이템 목록을 PlayerPrefs를 이용하여 레지스트리에 저장
        PlayerPrefs.SetString(ItemsKey, string.Join(",", savedItems)); // Join : 구분자(첫번째 변수)로 데이터목록을 묶음
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

            // 둘다 0이면 잘못된 데이터
            if (item.Id_inInventory == 0 && item.Type == 0) continue;

            // 올바른 데이터 저장
            else items.Add(item);
        }

        return items;
    }
}

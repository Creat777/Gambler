using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUp : PopUp
{
    private void OnEnable()
    {
        // 기존 목록 삭제
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        HashSet<Item> Player_items = PlayerPrefsManager.Instance.LoadItems();

        foreach (Item item in Player_items)
        {
            // 아이템 종합정보를 호출
            CsvToPrefabs itemInfo = ItemManager.Instance.Itemdict_serialToInfo[item.serialNumber];

            // 아이템 인스턴시
            GameObject obj = Instantiate(itemInfo.__itemPrefab);
            obj.SetActive(true);

            // 부모객체 설정
            obj.transform.SetParent(content.transform);

            // 스케일 초기화
            obj.transform.localScale = Vector3.one;
        }
    }
}

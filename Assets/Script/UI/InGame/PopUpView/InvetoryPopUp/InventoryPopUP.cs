using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUp : PopUp
{
    private void OnEnable()
    {
        // ���� ��� ����
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        HashSet<Item> Player_items = PlayerPrefsManager.Instance.LoadItems();

        foreach (Item item in Player_items)
        {
            // ������ ���������� ȣ��
            CsvToPrefabs itemInfo = ItemManager.Instance.Itemdict_serialToInfo[item.serialNumber];

            // ������ �ν��Ͻ�
            GameObject obj = Instantiate(itemInfo.__itemPrefab);
            obj.SetActive(true);

            // �θ�ü ����
            obj.transform.SetParent(content.transform);

            // ������ �ʱ�ȭ
            obj.transform.localScale = Vector3.one;
        }
    }
}

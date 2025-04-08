using PublicSet;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
    static public HashSet<sItem> ItemHashSet { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        ItemHashSet = new HashSet<sItem>();
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId���� ��ȯ�Ǵ� id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (ItemHashSet.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach (sItem item in ItemHashSet)
        {
            if (item.id > maxItemNumber)
            {
                maxItemNumber = item.id;
            }
        }
        return maxItemNumber;
    }

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(eItemType itemType)
    {
        int itemId = GetNewLastId();

        // �Էµ� ������ �ڵ忡 �´� �ӽõ����͸� ����
        sItem newItem = new sItem(itemId, itemType);

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (ItemHashSet.Contains(newItem))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Item {itemId} �� �̹� �����ϴ� ������ �׸�.");
            return;
        }
        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        ItemHashSet.Add(newItem);
        Debug.Log($"Item {itemId} ȹ�� ����.");

        // ������ ��Ͽ� ��ȭ�� �������� �˾� ��������
        InventoryPopUp.Instance.RefreshPopUp();
        //GameManager.connector_InGame.popUpView_Script.inventoryPopUp.RefreshPopUp();
    }
    //public void PlayerGetItem(eItemType itemType)
    //{
    //    int itemId = GetNewLastId();

    //    PlayerPrefsManager.Instance.ItemsDataSave(itemId, itemType,
    //        (sItem newItem) =>
    //        {
    //            // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
    //            if (ItemHashSet.Contains(newItem))
    //            {
    //                Debug.LogWarning($"Item {itemId} is already saved.");
    //                return;
    //            }
    //            // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
    //            ItemHashSet.Add(newItem);
    //        },
    //        () => Debug.Log($"Item {itemId} ���� ������.")
    //        );
    //}

    public void PlayerLoseItem(sItem item)
    {
        Debug.Log($"item {item.ToString()} ���� �õ�");

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (ItemHashSet.Contains(item) == false)
        {
            // �������� �ʴ� ���� �������̶�� ������ ���
            Debug.LogWarning($"Item {item.id}�� �������� �ʴ� ������");
            return;
        }

        // �����ϴ� �������̸� ��Ͽ��� ����
        ItemHashSet.Remove(item);

        Debug.Log($"Item {item.ToString()}  ���� ������");
    }
    //public void PlayerLoseItem(sItem item)
    //{
    //    Debug.Log($"item {item.ToString()} ���� �õ�");
    //    PlayerPrefsManager.Instance.ItemsDataSave(item.id, item.type,
    //        (sItem newItem) =>
    //        {
    //            // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
    //            if (ItemHashSet.Contains(newItem) == false)
    //            {
    //                // �������� �ʴ� ���� �������̶�� ������ ���
    //                Debug.LogWarning($"Item {item.id}�� �������� �ʴ� ������");
    //                return;
    //            }
    //            // �����ϴ� �������̸� ��Ͽ��� ����
    //            ItemHashSet.Remove(newItem);
    //        },
    //        () => Debug.Log($"Item {item.id} ���� ������")
    //        );
    //}
}

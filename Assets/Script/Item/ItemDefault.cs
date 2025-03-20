using PublicSet;
using UnityEngine;

public class ItemDefault : MonoBehaviour
{

    sItem item;
    cItemInfo itemInfo;

    public sItem Item { get { return item; } }
    public cItemInfo ItemInfo { get { return itemInfo; } }

    public void SaveItemData(int id, eItemType serial)
    {
        item = new sItem(id, serial);
        itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);
    }
    public void SaveItemData(sItem inputItem)
    {
        item = new sItem(inputItem);
        itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);
    }

    public void UsedByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);
    }

    public void SoldByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);
        PlayManager.Instance.PlayerMoneyPlus(itemInfo.value_Sale);
    }
}

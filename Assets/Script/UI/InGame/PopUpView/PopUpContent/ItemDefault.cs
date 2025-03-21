using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class ItemDefault : ButtonBase
{
    public Image image;
    sItem item;
    cItemInfo itemInfo;

    public sItem Item { get { return item; } }
    public cItemInfo ItemInfo { get { return itemInfo; } }

    

    public void InitItemData(int id, eItemType serial)
    {
        item = new sItem(id, serial);
        InitItemInfo(item.type);
    }

    public void InitItemData(sItem inputItem)
    {
        item = new sItem(inputItem);
        InitItemInfo(item.type);
    }

    public void InitItemInfo(eItemType itemType)
    {
        // ������ ������ �ʱ�ȭ
        itemInfo = CsvManager.Instance.GetItemInfo(itemType);

        // ������ �̹��� ��ü
        bool result = false;
        Sprite sprite = ItemImageResource.Instance.TryGetImage(itemType, out result);
        if (result)
        {
            image.sprite = sprite;
        }
    }

    public void UsedByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);
    }

    public void SoldByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);
        PlayManager.Instance.PlayerMoneyPlus(itemInfo.value_Sale);
    }
}

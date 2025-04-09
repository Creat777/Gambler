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

    

    public void InitItemData(int id, eItemType type, cItemInfo itemInfo)
    {
        item = new sItem(id, type);
        InitItemInfo(itemInfo);
    }

    public void InitItemData(sItem item, cItemInfo itemInfo)
    {
        this.item = new sItem(item);
        InitItemInfo(itemInfo);
    }

    public void InitItemInfo(cItemInfo itemInfo)
    {
        // 아이템 정보를 초기화
        this.itemInfo = itemInfo;

        // 아이템 이미지 교체
        bool result = false;
        Sprite sprite = ItemImageResource.Instance.TryGetImage(itemInfo.type, out result);
        if (result)
        {
            image.sprite = sprite;
        }
    }

    public void UsedByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(item);
    }

    public void SoldByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(item);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);
        PlayManager.Instance.AddPlayerMoney(itemInfo.value_Sale);
    }
}

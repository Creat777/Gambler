using PublicSet;
using UnityEngine;

public class ItemDefault : MonoBehaviour
{
    sItem item;
    
    public sItem SaveItemData(int id, eItemSerialNumber serial)
    {
        return item = new sItem(id, serial);
    }
    public sItem SaveItemData(sItem item)
    {
        return item = new sItem(item);
    }

    public void UsedByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);

        InventoryPopUp inventoryPopUpScript =  GameManager.Connector.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>();
        if(inventoryPopUpScript!= null)
        {
            inventoryPopUpScript.RefreshInventory();
        }
        else
        {
            Debug.LogWarning("error");
        }
    }

    public void SoldByPlayer()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.serialNumber);
        PlayManager.Instance.PlayerMoneyPlus(itemInfo.value_Sale);

        InventoryPopUp inventoryPopUpScript = GameManager.Connector.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>();
        if (inventoryPopUpScript != null)
        {
            inventoryPopUpScript.RefreshInventory();
        }
        else
        {
            Debug.LogWarning("error");
        }
    }
}

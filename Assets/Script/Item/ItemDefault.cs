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

    private void OnDestroy()
    {
        PlayerPrefsManager.Instance.PlayerLoseItem(item);
    }
}

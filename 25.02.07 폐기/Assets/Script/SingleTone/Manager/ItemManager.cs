using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private ItemTable itemTable;

    public Dictionary<int, ItemInfo> Itemdict_serialToInfo { get; private set; }

    public int QuestItemSerialNumber {  get; private set; }


    protected override void Awake()
    {
        base.Awake();
        InitItemDict();
        InitSerialNumber();
    }

    private void InitItemDict()
    {
        Itemdict_serialToInfo = new Dictionary<int, ItemInfo>();

        foreach(var itemInfo in itemTable.itemInfoList)
        {
            Itemdict_serialToInfo.Add(itemInfo.SerialNumber, itemInfo);
        }
    }

    private void InitSerialNumber()
    {
        QuestItemSerialNumber = itemTable.itemInfoList[0].SerialNumber;
    }

}

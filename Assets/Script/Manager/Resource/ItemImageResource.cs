using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class ItemImageResource : ImageResourceBase<ItemImageResource,eItemType>
{
    [SerializeField] private Sprite[] sScroll;
    [SerializeField] private Sprite[] sMeat;
    [SerializeField] private Sprite[] sFish;
    [SerializeField] private Sprite[] sEgg;
    protected override void InitImageDict()
    {
        imageDict = new Dictionary<eItemType, Sprite[]>();

        imageDict.Add(eItemType.TutorialQuest, sScroll);
        imageDict.Add(eItemType.Meat, sMeat);
        imageDict.Add(eItemType.Fish, sFish);
        imageDict.Add(eItemType.Egg, sEgg);
        imageDict.Add(eItemType.Notice_Stage1, sScroll);
    }
}

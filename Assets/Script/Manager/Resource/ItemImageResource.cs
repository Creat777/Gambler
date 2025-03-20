using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemImageResource : ImageResourceBase<ItemImageResource,eItemImage>
{
    [SerializeField] private Sprite[] sScroll;
    [SerializeField] private Sprite[] sMeat;
    [SerializeField] private Sprite[] sFish;
    [SerializeField] private Sprite[] sEgg;
    protected override void InitImageDict()
    {
        imageDict = new Dictionary<eItemImage, Sprite[]>();

        imageDict.Add(eItemImage.Scroll, sScroll);
        imageDict.Add(eItemImage.Meat, sMeat);
        imageDict.Add(eItemImage.Fish, sFish);
        imageDict.Add(eItemImage.Egg, sEgg);
    }
}

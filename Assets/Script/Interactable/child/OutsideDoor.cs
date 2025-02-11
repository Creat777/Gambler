using UnityEngine;
using PublicSet;

public class OutsideDoor : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.OutsideDoor;
    }
}

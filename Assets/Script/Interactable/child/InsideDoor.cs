using UnityEngine;
using PublicSet;

public class InsideDoor : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.InsideDoor;
    }
}

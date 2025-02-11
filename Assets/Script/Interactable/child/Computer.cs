using UnityEngine;
using PublicSet;

public class Computer : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.Computer;
    }
}

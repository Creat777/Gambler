using UnityEngine;
using PublicSet;

public class Bed : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.Bed;
    }
}

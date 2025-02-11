using UnityEngine;
using PublicSet;
public class Cabinet : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.Cabinet;
    }
}

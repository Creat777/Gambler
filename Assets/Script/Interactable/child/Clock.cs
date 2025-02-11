using UnityEngine;
using PublicSet;
public class Clock : InteractableObject
{
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return eCsvFile_InterObj.Clock;
    }
}

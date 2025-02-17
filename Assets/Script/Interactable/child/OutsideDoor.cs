using UnityEngine;
using PublicSet;

public class OutsideDoor : InteractableObject
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.OutsideDoor;
    }
}

using UnityEngine;
using PublicSet;

public class InsideDoor : InteractableObject
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.InsideDoor;
    }
}

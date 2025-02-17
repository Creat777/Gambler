using UnityEngine;
using PublicSet;

public class Computer : InteractableObject
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Computer;
    }
}

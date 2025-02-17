using UnityEngine;
using PublicSet;

public class Bed : InteractableObject
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Bed;
    }
}

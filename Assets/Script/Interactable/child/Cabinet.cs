using UnityEngine;
using PublicSet;
public class Cabinet : InteractableObject
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Cabinet;
    }
}

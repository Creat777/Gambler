using UnityEngine;
using PublicSet;
public class Box : InteractableObject
{
    eTextScriptFile current;

    private void Start()
    {
        current = eTextScriptFile.None;
    }

    public override eTextScriptFile GetInteractableEnum()
    {
        return current;
    }

    public void FillUpBox()
    {
        current = eTextScriptFile.Box_Full;
    }

    public void EmptyOutBox()
    {
        current = eTextScriptFile.Box_Empty;
    }
}

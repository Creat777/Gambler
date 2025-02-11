using UnityEngine;
using PublicSet;
public class Box : InteractableObject
{
    eCsvFile_InterObj current;

    private void Start()
    {
        FillUpBox();
    }

    public override eCsvFile_InterObj GetInteractableEnum()
    {
        return current;
    }

    public void FillUpBox()
    {
        current = eCsvFile_InterObj.Box_Full;
    }

    public void EmptyOutBox()
    {
        current = eCsvFile_InterObj.Box_Empty;
    }
}

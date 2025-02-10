using UnityEngine;

public class Box : InteractableObject
{
    CsvManager.eCsvFile_InterObj current;

    private void Start()
    {
        FillUpBox();
    }

    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return current;
    }

    public void FillUpBox()
    {
        current = CsvManager.eCsvFile_InterObj.Box_Full;
    }

    public void EmptyOutBox()
    {
        current = CsvManager.eCsvFile_InterObj.Box_Empty;
    }
}

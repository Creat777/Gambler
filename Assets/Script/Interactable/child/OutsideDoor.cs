using UnityEngine;

public class OutsideDoor : InteractableObject
{
    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return CsvManager.eCsvFile_InterObj.OutsideDoor;
    }
}

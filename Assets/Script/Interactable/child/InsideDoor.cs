using UnityEngine;

public class InsideDoor : InteractableObject
{
    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return CsvManager.eCsvFile_InterObj.InsideDoor;
    }
}

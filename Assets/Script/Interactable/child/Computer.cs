using UnityEngine;

public class Computer : InteractableObject
{
    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return CsvManager.eCsvFile_InterObj.Computer;
    }
}

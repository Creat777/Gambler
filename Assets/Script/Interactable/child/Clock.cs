using UnityEngine;

public class Clock : InteractableObject
{
    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return CsvManager.eCsvFile_InterObj.Clock;
    }
}

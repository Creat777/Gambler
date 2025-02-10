using UnityEngine;

public class Cabinet : InteractableObject
{
    public override CsvManager.eCsvFile_InterObj GetInteractableEnum()
    {
        return CsvManager.eCsvFile_InterObj.Cabinet;
    }
}

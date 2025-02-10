using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract CsvManager.eCsvFile_InterObj GetInteractableEnum();
}

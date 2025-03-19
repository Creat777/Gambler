using UnityEngine;

public class MethodManager : Singleton<MethodManager>
{
    public bool IsIndexInRange<T>(T[] array, int index)
    {
        return index >= 0 && index < array.Length;
    }
}

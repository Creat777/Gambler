using UnityEngine;

public class DontSavePlugsTerminate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = new GameObject();
        obj.hideFlags = HideFlags.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

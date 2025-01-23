using UnityEngine;

public class TableTest : MonoBehaviour
{
    public TableText table;

    public  void OnEnable()
    {
        Debug.Log($"TableTest c : {table.c}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

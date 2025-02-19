using UnityEngine;

public class Bclass : Aclass
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Debug.Log("클래스 B 실행");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

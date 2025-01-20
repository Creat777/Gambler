using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    

    // 에디터에서 수정
    public float gameSpeed;

    // 스크립트에서 수정
    List<List<string>> BedCsvData;


    protected override void Awake()
    {
        base.Awake();
        BedCsvData = new List<List<string>>();
        // 함수여러개야
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        csvProcess();
    }

    private void csvProcess()
    {
        string csvFileName = "Interactive_Bed";
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);
        CsvProcessor.Instance.CsvLoading(csvFile, BedCsvData);
        CsvProcessor.Instance.CheckCsv(csvFileName, BedCsvData);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

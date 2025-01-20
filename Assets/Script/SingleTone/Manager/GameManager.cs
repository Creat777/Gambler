using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    

    // �����Ϳ��� ����
    public float gameSpeed;

    // ��ũ��Ʈ���� ����
    List<List<string>> BedCsvData;


    protected override void Awake()
    {
        base.Awake();
        BedCsvData = new List<List<string>>();
        // �Լ���������
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

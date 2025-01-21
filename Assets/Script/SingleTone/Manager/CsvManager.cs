using UnityEngine;
using System.Collections.Generic;
public class CsvInfo
{
    public readonly string CsvFileName;
    public List<List<string>> csvData;

    public CsvInfo(string CsvFileName)
    {
        this.CsvFileName = CsvFileName;
    }
}

public struct Interactive
{
    byte unUsed;
}


public class CsvManager : Singleton<CsvManager>
{
    // 실사용 안함
    Interactive interactive;

    // 스크립트에서 수정
    public CsvInfo[] InteractiveCsvInfos {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        InitCsvManager();
    }

    private void Start()
    {
        foreach (var csvInfo in InteractiveCsvInfos)
        {
            CsvProcess(csvInfo, interactive);
        }
    }
    public void InitCsvManager()
    {
        InteractiveCsvInfos = new CsvInfo[1];
        InteractiveCsvInfos[0] = new CsvInfo("Interactive_Bed");
    }

    private void CsvProcess(CsvInfo csvInfo, Interactive interactive)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvInfo.CsvFileName);
        CsvProcessor.Instance.CsvLoading(csvFile, csvInfo.csvData);
        CsvProcessor.Instance.CheckCsv(csvInfo.CsvFileName, csvInfo.csvData);
    }

    
}



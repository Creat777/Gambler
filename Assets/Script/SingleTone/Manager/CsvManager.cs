using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using System.Text;
public class CsvInfo
{
    public readonly string CsvFileName;
    public List<List<string>> csvData;

    public CsvInfo(string CsvFileName)
    {
        this.CsvFileName = CsvFileName;
        csvData = new List<List<string>>();
    }
}

public struct Interactive
{
    private byte unUsed;
}


public class CsvManager : Singleton<CsvManager>
{
    // 실사용 안함
    Interactive interactive;

    // 스크립트에서 수정
    public CsvInfo[] InteractiveCsvInfos {  get; private set; } // 상호작용 가능한 객체들의 스크립트csv

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
        // 게임중 추가삭제가 없을테니 참조가 빠른 방법을 선택
        InteractiveCsvInfos = new CsvInfo[5];
        InteractiveCsvInfos[0] = new CsvInfo("Interactive_Bed");
        InteractiveCsvInfos[1] = new CsvInfo("Interactive_Cabinet");
        InteractiveCsvInfos[2] = new CsvInfo("Interactive_Clock");
        InteractiveCsvInfos[3] = new CsvInfo("Interactive_Computer");
        InteractiveCsvInfos[4] = new CsvInfo("Interactive_Door");
    }

    private void CsvProcess(CsvInfo csvInfo, Interactive interactive)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvInfo.CsvFileName);
        CsvProcessor.Instance.CsvLoading(csvFile, csvInfo.csvData);
        CsvProcessor.Instance.CheckCsv(csvInfo.CsvFileName, csvInfo.csvData);
    }

    
}



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
    // �ǻ�� ����
    Interactive interactive;

    // ��ũ��Ʈ���� ����
    public CsvInfo[] InteractiveCsvInfos {  get; private set; } // ��ȣ�ۿ� ������ ��ü���� ��ũ��Ʈcsv

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
        // ������ �߰������� �����״� ������ ���� ����� ����
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



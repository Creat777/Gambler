using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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

    private void CsvProcess(CsvInfo csvInfo, Interactive unused)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvInfo.CsvFileName);
        CsvLoading(csvFile, csvInfo.csvData, interactive);
        CheckCsv(csvInfo.CsvFileName, csvInfo.csvData);
    }

    //

    public virtual void CsvLoading(TextAsset csvFile, List<List<string>> CsvData, Interactive unused)
    {
        if (csvFile != null)
        {
            // csv������ \n ���ڸ� �������� ����
            string[] rows = csvFile.text.Split('\n');

            // ������ ,�� �и��ؼ� ����
            foreach (string row in rows)
            {
                // ,�� �������� ����
                string[] fileds = row.Split(',');

                // list�� ��ȯ
                List<string> rowData = fileds.ToList<string>();

                // ���� ���� ����
                if (rowData.Count >= 2)
                {
                    CsvData.Add(rowData);
                }
                else
                {
                    Debug.Log($"������ ������ ���� : {rowData[0]}");
                }
            }
        }
        else
        {
            Debug.Log("csv���� Load����");
        }
    }

    public virtual void CheckCsv(string csvFileName, List<List<string>> CsvData)
    {
        int row_num = 0;
        Debug.Log(csvFileName + " : ");
        foreach (var row in CsvData)
        {

            Debug.Log($"[ {++row_num} ]��");
            int field_num = 0;
            foreach (var field in row)
            {
                Debug.Log($"{++field_num}�� " + field);
            }
        }
    }

    public virtual string[] GetText(CsvInfo csvInfo, Interactive interactive)
    {
        string[] csvFileNames = csvInfo.CsvFileName.Split("_");

        // ���� �˻�
        if (csvFileNames[0] == "Interactive")
        {
            Debug.Log($"���ҿϷ� : {csvFileNames[0]}_{csvFileNames[1]}");

            // ����� �ؽ�Ʈ�� ������ ����(��ȯ����)
            List<string> csvRows = new List<string>();

            // ���� ���������� Ȯ���ϱ� ���� ����
            string gameStage = GameManager.Instance.curruentGameStage; 

            // ���� ������������ ����� ���� ���ߴٸ� �Լ�����
            if (gameStage == null)
            {
                return null;
            }

            // csv������(���)���� �� ���� ����
            foreach (var csvRow in csvInfo.csvData)
            {
                // ���� ���������� �ƴѺκ��� �н�
                if (csvRow[0] != gameStage)
                {
                    Debug.Log($"���� ������� �ʴ� ������ -> csvRow[0] : {csvRow[0]} != gameStage : {gameStage}");
                    continue;
                }

                // ���� ���������� ���
                else if (csvRow[0] == gameStage)
                {
                    Debug.Log("���ӽ��������� �ش��ϴ� ��ũ��Ʈ ���� ����");
                    // �����˻�(������ : stage, ��ũ��Ʈ)
                    if (csvRow.Count >= 2)
                    {
                        // ��ũ��Ʈ ��Ͽ� �߰�
                        csvRows.Add(csvRow[1]);
                    }
                }
            }
            return csvRows.ToArray();

        }
        else
        {
            Debug.LogError("�߸��� csv���� ����");
        }

        return null;
    }

    public CsvInfo FindCsvInfo(string objectName, CsvInfo[] csvInfos, Interactive interactive)
    {
        foreach (var csvInfo in csvInfos)
        {
            // ex) Interactive_Bed

            // �̸��� ������ �ش��ϴ� csvinfo�� ã������
            if (csvInfo.CsvFileName == objectName)
            {
                return csvInfo;
            }
        }
        return null;
    }
}



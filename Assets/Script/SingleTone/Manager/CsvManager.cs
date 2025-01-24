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
    Dictionary<string, int> InteractiveCsvIdDict; // InteractiveCsvInfos�� ���ϸ� �ش��ϴ� �ε����� ������


    protected override void Awake()
    {
        base.Awake();
        InteractiveCsvIdDict = new Dictionary<string, int>();
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
        InteractiveCsvInfos = new CsvInfo[6];
        InteractiveCsvInfos[0] = new CsvInfo("Interactive_Bed");
        InteractiveCsvInfos[1] = new CsvInfo("Interactive_Cabinet");
        InteractiveCsvInfos[2] = new CsvInfo("Interactive_Clock");
        InteractiveCsvInfos[3] = new CsvInfo("Interactive_Computer");
        InteractiveCsvInfos[4] = new CsvInfo("Interactive_Door");
        InteractiveCsvInfos[5] = new CsvInfo("Interactive_OutsideDoor");
        InitCsvIdDict(InteractiveCsvIdDict, InteractiveCsvInfos);
    }

    public void InitCsvIdDict(Dictionary<string, int> csvIdDict, CsvInfo[] csvInfos)
    {
        csvIdDict.Clear();
        for (int i = 0; i < csvInfos.Length; i++)
        {
            csvIdDict.Add(csvInfos[i].CsvFileName, i);
        }
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
            for (int i = 0; i < rows.Length; i++)
            {
                // ù���� ����
                if(i == 0)
                {
                    continue;
                }

                // ,�� �������� ����
                string[] fileds = rows[i].Split(',');

                // list�� ��ȯ
                List<string> rowData = fileds.ToList<string>();

                // ���� ���� ����
                if (rowData.Count >= 3)
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

    public virtual List<string[]> GetText(string objName, Interactive interactive)
    {
        // ������Ʈ�� �ش��ϴ� csvInfo�� ����
        CsvInfo csvInfo = FindCsvInfo(objName, interactive);

        // ������� ���� csvInfo�� ����� ���ϸ��� ����
        string csvFileName = csvInfo.CsvFileName;

        // ���� �˻�(�ùٸ� �����͸� ã�Ҵٸ� if�� ����)
        if (objName == csvFileName)
        {
            // ����� �ؽ�Ʈ�� ������ ����(��ȯ����)
            List<string[]> csvRows = new List<string[]>();

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

                    // selection ������ ��ũ��Ʈ���� �и��Ͽ� ����
                    string[] temp = new string[csvRow.Count-1];
                    for (int i = 1; i < csvRow.Count; i++)
                    {
                        temp[i-1] = csvRow[i];
                    }
                    csvRows.Add(temp.ToArray());
                }
            }
            return csvRows;

        }
        else
        {
            Debug.LogError("�߸��� csv���� ����");
        }

        return null;
    }

    public CsvInfo FindCsvInfo(string objectName, Interactive interactive)
    {
        // ���ϸ�(== ������Ʈ �̸�)�� ���ε� csvInfos�� �ε����� ����
        int index = InteractiveCsvIdDict[objectName];

        // �迭 ���� ������ �ùٸ� �����͸� ��ȯ
        if(index < InteractiveCsvInfos.Length)
        {
            return InteractiveCsvInfos[index];
        }
        return null;
    }
}



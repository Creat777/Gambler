using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;
public class CsvInfo
{
    public readonly string CsvFileName;
    public readonly string Path;
    public List<List<string>> csvData;
    public CsvInfo(string Path)
    {
        this.Path = Path;
        string[] temp = Path.Split('/');
        if(temp.Length > 1 )
        {
            this.CsvFileName = temp[temp.Length-1];
        }
        csvData = new List<List<string>>();
    }
}




public class CsvManager : Singleton<CsvManager>
{

    // ��ũ��Ʈ���� ����
    //public CsvInfo[] Interactable_CsvList {  get; private set; } // ��ȣ�ۿ� ������ ��ü���� ��ũ��Ʈcsv
    Dictionary<string, CsvInfo> CsvDict; // csv���ϸ�� csv �����͸� ����

    protected override void Awake()
    {
        base.Awake();
        CsvDict = new Dictionary<string, CsvInfo>();

        // List�� ���� �ѹ��� �����ϰ� ������
        List<CsvInfo>  Interactable_CsvList = new List<CsvInfo>();
        InitCsvManager(Interactable_CsvList);
        foreach (var csvInfo in Interactable_CsvList)
        {
            CsvProcess(csvInfo);
        }

        CheckCsv(Interactable_CsvList[Interactable_CsvList.Count-1].CsvFileName, Interactable_CsvList[Interactable_CsvList.Count - 1].csvData);
    }

    private void Start()
    {
        
    }
    public void InitCsvManager(List<CsvInfo> CsvList)
    {
        // �� ���� ��ȣ�ۿ�
        CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Bed"));
        CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Cabinet"));
        CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Clock"));
        CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Computer"));
        CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Door"));

        // �� �ܺ� ��ȣ�ۿ�
        CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_OutsideDoor"));
        CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_Box_Full"));
        CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_Box_Empty"));

        // Player ����
        CsvList.Add(new CsvInfo("PlayerMonologue/PlayerMonologue"));

        InitCsvDict(CsvDict, CsvList);
    }

    public void InitCsvDict(Dictionary<string, CsvInfo> csvDict, List<CsvInfo> csvInfos)
    {
        csvDict.Clear();
        foreach (var csvInfo in csvInfos)
        {
            csvDict.Add(csvInfo.CsvFileName, csvInfo);
        }
    }

    private void CsvProcess(CsvInfo csvInfo)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvInfo.Path);
        CsvLoading(csvFile, csvInfo.csvData);
        //CheckCsv(csvInfo.CsvFileName, csvInfo.csvData);
    }

    //

    public virtual void CsvLoading(TextAsset csvFile, List<List<string>> CsvData)
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

    public virtual List<string[]> GetText(string fileName)
    {
        // csvInfo�� ����
        CsvInfo csvInfo = FindCsvInfo(fileName);
        if(csvInfo == null)
        {
            Debug.LogWarning("csv������ ã�� ����");
            return null;
        }

        // ������� ���� csvInfo�� ����� ���ϸ��� ����
        string csvFileName = csvInfo.CsvFileName;

        // ���� �˻�(�ùٸ� �����͸� ã�Ҵٸ� if�� ����)
        if (fileName == csvFileName)
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
                    Debug.Log("���ӽ��������� �ش��ϴ� ������ ���� ����");

                    // stage��ȣ�� ������ ��� ������ �и��Ͽ� ����
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

    public CsvInfo FindCsvInfo(string fileName)
    {
        CsvDict.TryGetValue(fileName, out var csvInfo);
        return csvInfo;
    }
}



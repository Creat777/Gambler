using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CsvProcessor : Singleton<CsvProcessor>
{
    public Text text;

    void Start()
    {
        
    }
    

    public virtual void CsvLoading(TextAsset csvFile, List<List<string>> CsvData)
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
                    //new List<string>(fileds.Length);

                //// �� ��Ҹ� ����Ʈ�� ���� 
                //foreach(string filed in fileds)
                //{
                //    rowData.Add(filed);
                //}

                if(rowData.Count >= 2)
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

    public virtual string[] GetText(CsvInfo csvInfo,Interactive interactive)
    {
        string[] csvFileNames = csvInfo.CsvFileName.Split("_");

        // ���� �˻�
        if (csvFileNames[0] == "Interactive")
        {
            List<string> csvRows = new List<string>();
            string gameStage = GameManager.Instance.gameStage; // ���� ���������� Ȯ���ϱ� ���� ����

            foreach (var csvRow in csvInfo.csvData)// csv������(���)���� �� ���� ����
            {
                // ���� ���������� �ƴѺκ��� �н�
                if (csvRow[0] != gameStage)
                {
                    continue; 
                }

                // ���� ���������� ���
                else if(csvRow[0] == gameStage)
                {
                    // �����˻�
                    if(csvRow.Count>=2)
                    {
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
            string[] fileNameWord = csvInfo.CsvFileName.Split('_');

            // ����ó��
            if (fileNameWord.Length >=2)
            {
                // �̸��� ������ �ش��ϴ� csvinfo�� ã������
                if (fileNameWord[1] == objectName) 
                {
                    return csvInfo;
                }
            }
            else
            {
                //�����
            }
            
        }
        return null;
    }
    void Update()
    {
        
    }
}

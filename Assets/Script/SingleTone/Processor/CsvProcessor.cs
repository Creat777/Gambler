using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using System.Text;

public class CsvProcessor : Singleton<CsvProcessor>
{
    
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

    void Update()
    {
        
    }
}

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
            // csv파일을 \n 문자를 기준으로 분할
            string[] rows = csvFile.text.Split('\n');

            // 각행을 ,로 분리해서 저장
            foreach (string row in rows) 
            {
                // ,을 기준으로 분할
                string[] fileds = row.Split(',');

                // list로 변환
                List<string> rowData = fileds.ToList<string>();
                    //new List<string>(fileds.Length);

                //// 각 요소를 리스트에 삽입 
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
                    Debug.Log($"쓰레기 데이터 제거 : {rowData[0]}");
                }
            }
        }
        else
        {
            Debug.Log("csv파일 Load실패");
        }
    }

    public virtual void CheckCsv(string csvFileName, List<List<string>> CsvData)
    {
        int row_num = 0;
        Debug.Log(csvFileName + " : ");
        foreach (var row in CsvData)
        {
            
            Debug.Log($"[ {++row_num} ]행");
            int field_num = 0;
            foreach (var field in row)
            {
                Debug.Log($"{++field_num}열 " + field);
            }
        }
    }

    void Update()
    {
        
    }
}

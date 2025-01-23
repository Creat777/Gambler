using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CsvLoading : MonoBehaviour
{
    private List<List<string>> csvData;
    void Start()
    {
        csvData = new List<List<string>>();
        TextAsset csvFile = Resources.Load<TextAsset>("example");
        if(csvFile != null)
        {
            // '\n' 로 행을 구별
            // ','으로 각 행의 원소를 구별
            string[] rows = csvFile.text.Split('\n');
            foreach (string row in rows)
            {
                string[]fields = row.Split(',');
                List<string> rowData = new List<string>(fields);
                csvData.Add(rowData);
            }

            // 파일 확인 또는 사용
            int row_num = 0;
            foreach(List<string> row in csvData)
            {
                Debug.Log($"[ {row_num + 1} ]행");
                int field_num = 0;
                foreach(string field in row)
                {
                    switch(field_num)
                    {
                        case 0: Debug.Log($"{++field_num}열 " + int.Parse(field)); break;
                        case 1: Debug.Log($"{++field_num}열 " + int.Parse(field)); break;
                        case 2: Debug.Log($"{++field_num}열 " + float.Parse(field)); break;
                        case 3: Debug.Log($"{++field_num}열 " + float.Parse(field)); break;
                    }
                }
                row_num++;
            }
            
        }
        else
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
    }

    void Update()
    {
        
    }
}

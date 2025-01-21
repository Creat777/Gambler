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

    public virtual string[] GetText(CsvInfo csvInfo,Interactive interactive)
    {
        string[] csvFileNames = csvInfo.CsvFileName.Split("_");

        // 에러 검사
        if (csvFileNames[0] == "Interactive")
        {
            List<string> csvRows = new List<string>();
            string gameStage = GameManager.Instance.gameStage; // 현재 진행정도를 확인하기 위한 변수

            foreach (var csvRow in csvInfo.csvData)// csv데이터(행렬)에서 각 행을 꺼냄
            {
                // 현재 스테이지가 아닌부분은 패스
                if (csvRow[0] != gameStage)
                {
                    continue; 
                }

                // 현재 스테이지의 경우
                else if(csvRow[0] == gameStage)
                {
                    // 에러검사
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
            Debug.LogError("잘못된 csv파일 전달");
        }

        return null;
    }

    public CsvInfo FindCsvInfo(string objectName, CsvInfo[] csvInfos, Interactive interactive)
    {
        foreach (var csvInfo in csvInfos)
        {
            string[] fileNameWord = csvInfo.CsvFileName.Split('_');

            // 예외처리
            if (fileNameWord.Length >=2)
            {
                // 이름이 같으면 해당하는 csvinfo를 찾은것임
                if (fileNameWord[1] == objectName) 
                {
                    return csvInfo;
                }
            }
            else
            {
                //디버깅
            }
            
        }
        return null;
    }
    void Update()
    {
        
    }
}

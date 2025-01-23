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
            // csv파일을 \n 문자를 기준으로 분할
            string[] rows = csvFile.text.Split('\n');

            // 각행을 ,로 분리해서 저장
            foreach (string row in rows)
            {
                // ,을 기준으로 분할
                string[] fileds = row.Split(',');

                // list로 변환
                List<string> rowData = fileds.ToList<string>();

                // 공백 행은 제거
                if (rowData.Count >= 2)
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

    public virtual string[] GetText(CsvInfo csvInfo, Interactive interactive)
    {
        string[] csvFileNames = csvInfo.CsvFileName.Split("_");

        // 에러 검사
        if (csvFileNames[0] == "Interactive")
        {
            Debug.Log($"분할완료 : {csvFileNames[0]}_{csvFileNames[1]}");

            // 출력할 텍스트를 저장할 변수(반환예정)
            List<string> csvRows = new List<string>();

            // 현재 진행정도를 확인하기 위한 변수
            string gameStage = GameManager.Instance.curruentGameStage; 

            // 게임 스테이지값을 제대로 받지 못했다면 함수종료
            if (gameStage == null)
            {
                return null;
            }

            // csv데이터(행렬)에서 각 행을 꺼냄
            foreach (var csvRow in csvInfo.csvData)
            {
                // 현재 스테이지가 아닌부분은 패스
                if (csvRow[0] != gameStage)
                {
                    Debug.Log($"현재 사용하지 않는 데이터 -> csvRow[0] : {csvRow[0]} != gameStage : {gameStage}");
                    continue;
                }

                // 현재 스테이지의 경우
                else if (csvRow[0] == gameStage)
                {
                    Debug.Log("게임스테이지에 해당하는 스크립트 참조 성공");
                    // 에러검사(데이터 : stage, 스크립트)
                    if (csvRow.Count >= 2)
                    {
                        // 스크립트 목록에 추가
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
            // ex) Interactive_Bed

            // 이름이 같으면 해당하는 csvinfo를 찾은것임
            if (csvInfo.CsvFileName == objectName)
            {
                return csvInfo;
            }
        }
        return null;
    }
}



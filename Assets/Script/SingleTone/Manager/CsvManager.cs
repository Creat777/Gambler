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


public struct Interactive
{
    private byte unUsed;
}


public class CsvManager : Singleton<CsvManager>
{
    // 실사용 안함
    Interactive interactive;

    // 스크립트에서 수정
    //public CsvInfo[] Interactable_CsvList {  get; private set; } // 상호작용 가능한 객체들의 스크립트csv
    Dictionary<string, CsvInfo> Interactable_CsvDict; // csv파일명과 csv 데이터를 맵핑


    protected override void Awake()
    {
        base.Awake();
        Interactable_CsvDict = new Dictionary<string, CsvInfo>();

        // List는 최초 한번만 존재하고 삭제됨
        List<CsvInfo>  Interactable_CsvList = new List<CsvInfo>();
        InitCsvManager(Interactable_CsvList);
        foreach (var csvInfo in Interactable_CsvList)
        {
            CsvProcess(csvInfo, interactive);
        }
        
    }

    private void Start()
    {
        
    }
    public void InitCsvManager(List<CsvInfo> Interactable_CsvList)
    {
        // 집 내부
        Interactable_CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Bed"));
        Interactable_CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Cabinet"));
        Interactable_CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Clock"));
        Interactable_CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Computer"));
        Interactable_CsvList.Add(new CsvInfo("Inside_Of_House/Interactable_Door"));

        // 집 외부
        Interactable_CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_OutsideDoor"));
        Interactable_CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_Box_Full"));
        Interactable_CsvList.Add(new CsvInfo("OutSide_Of_House/Interactable_Box_Empty"));

        InitCsvDict(Interactable_CsvDict, Interactable_CsvList);
    }

    public void InitCsvDict(Dictionary<string, CsvInfo> csvDict, List<CsvInfo> csvInfos)
    {
        csvDict.Clear();
        foreach (var csvInfo in csvInfos)
        {
            csvDict.Add(csvInfo.CsvFileName, csvInfo);
        }
    }

    private void CsvProcess(CsvInfo csvInfo, Interactive unused)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvInfo.Path);
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
            for (int i = 0; i < rows.Length; i++)
            {
                // 첫행은 버림
                if(i == 0)
                {
                    continue;
                }

                // ,을 기준으로 분할
                string[] fileds = rows[i].Split(',');

                // list로 변환
                List<string> rowData = fileds.ToList<string>();

                // 공백 행은 제거
                if (rowData.Count >= 3)
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

    public virtual List<string[]> GetText(string objName, Interactive interactive)
    {
        // 오브젝트에 해당하는 csvInfo를 저장
        CsvInfo csvInfo = FindCsvInfo(objName, interactive);

        // 디버깅을 위해 csvInfo에 저장된 파일명을 저장
        string csvFileName = csvInfo.CsvFileName;

        // 에러 검사(올바른 데이터를 찾았다면 if문 진입)
        if (objName == csvFileName)
        {
            // 출력할 텍스트를 저장할 변수(반환예정)
            List<string[]> csvRows = new List<string[]>();

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
                    Debug.Log("게임스테이지에 해당하는 데이터 참조 성공");

                    // stage번호를 제외한 모든 정보를 분리하여 저장
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
            Debug.LogError("잘못된 csv파일 전달");
        }

        return null;
    }

    public CsvInfo FindCsvInfo(string objectName, Interactive interactive)
    {
        Interactable_CsvDict.TryGetValue(objectName, out var csvInfo);
        return csvInfo;
    }
}



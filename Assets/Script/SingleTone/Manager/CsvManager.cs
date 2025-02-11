using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;



public class CsvManager : Singleton<CsvManager>
{
    // csv파일 저장을 위한 배열
    // List<IteractableInfoList> : 파일 내 스테이지별 데이터
    private List<cIteractableInfoList>[][] InteractableInfoFiles;
    private List<cPlayerMonologueInfoList>[][] PlayerMonologueFiles;

    public List<cIteractableInfoList> GetInteractableCsv(eCsvFile_InterObj eCsv, eStage eStage)
    {
        return InteractableInfoFiles[(int)eCsv][(int)eStage];
    }

    public List<cPlayerMonologueInfoList> GetPlayerMonologueCsv(eCsvFile_PlayerMono eCsv, eStage eStage)
    {
        return PlayerMonologueFiles[(int)eCsv][(int)eStage];
    }

    protected override void Awake()
    {
        base.Awake();

        // 
        int StageCount = Enum.GetValues(typeof(eStage)).Length;

        // 파일개수에서 None은 제외

        // 상호작용 관련
        int InteractionFileCount = Enum.GetValues(typeof(eCsvFile_InterObj)).Length - 1 ;
        InteractableInfoFiles = new List<cIteractableInfoList>[InteractionFileCount][];
        NewCsvFile(InteractionFileCount, StageCount, InteractableInfoFiles);

        // 독백 관련
        int MonologueFileCount = Enum.GetValues(typeof(eCsvFile_PlayerMono)).Length - 1;
        PlayerMonologueFiles = new List<cPlayerMonologueInfoList>[InteractionFileCount][];
        NewCsvFile(MonologueFileCount, StageCount, PlayerMonologueFiles);
    }

    public void NewCsvFile<T>(int FileCount, int StageCount, List<T>[][] CsvFileInfoPerStage)
    {
        for (int i = 0; i < FileCount; i++)
        {
            // 각 파일들에 스테이지관리를 위한 저장공간
            CsvFileInfoPerStage[i] = new List<T>[StageCount];

            for (int j = 0; j < StageCount; j++)
            {
                // 각 스테이지마다 갖는 데이터 공간
                CsvFileInfoPerStage[i][j] = new List<T>(); // 리스트 초기화
            }
        }
    }

    private void Start()
    {
        PrecessCsvOfInteraction();
        PrecessCsvOfMonologue();
    }

    private void PrecessCsvOfInteraction()
    {
        // 상호작용 객체에 대한 csv
        string path = "CSV/InteractableObject/";
        ProcessCsv<eCsvFile_InterObj, cIteractableInfoList>(path, LoadInteractableCsv ,InteractableInfoFiles);
        
    }

    private void PrecessCsvOfMonologue()
    {
        // 플레이어 모놀로그에 대한 csv
        string path = "CSV/PlayerMonologue/";
        ProcessCsv<eCsvFile_PlayerMono, cPlayerMonologueInfoList>(path, LoadMonologueCsv, PlayerMonologueFiles);
    }

    private void ProcessCsv<T_enum, T_class>(string path, Action<string, int> LoadIndividualCsv,List<T_class>[][] CsvFileInfoPerStage) where T_enum : Enum
    {
        foreach (var eFileName in Enum.GetValues(typeof(T_enum)))
        {
            // 마지막 순서로 None이 있음
            if ((int)eFileName == Enum.GetValues(typeof(T_enum)).Length - 1) continue;

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // 데이터 처리
            LoadIndividualCsv(FilePath, eFileCode);

            // 처리된 데이터의 확인
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();

                // 정상적인 스테이지 코드의 경우만 실행
                if(eStageCode != 0)
                {
                    // 각 파일에서 행을 하나씩 뽑아서 데이터를 올바르게 처리했는지 확인
                    foreach (T_class info in CsvFileInfoPerStage[eFileCode][eStageCode])
                    {
                        PrintProperties(info);
                    }
                }    
                
            }
        }
    }

    public void LoadInteractableCsv(string path, int fileEnum)
    {
        if ((eCsvFile_InterObj)fileEnum == eCsvFile_InterObj.None) return;

        LoadCsv<cIteractableInfoList>(path,
            (row, info)=>
            {
                if (info == null) return;

                eStage stage = eStage.None;
                int field_num = 0;
                foreach (string field in row)
                {
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            if (int.TryParse(field, out int intField))
                            {
                                switch (intField)
                                {
                                    case 1: Debug.Log($"입력된 스테이지 코드 : {intField}"); stage = eStage.Stage1; break;
                                    case 2: Debug.Log($"입력된 스테이지 코드 : {intField}"); stage = eStage.Stage2; break;
                                    default: Debug.LogWarning($"{field}는 정의되지 않은 스테이지 코드"); break;
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]는 스테이지 코드값이 숫자가 아님");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                        case 3: 
                            if(int.TryParse(field, out int intField2)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eSelection), intField2)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.eSelect = (eSelection)intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField2}는 {typeof(eSelection).Name}에 정의되지 않았음");
                                }
                            }
                            break;
                        default:
                            // 선택지가 존재하는 경우에만
                            if (info.eSelect == eSelection.Exist)
                            {
                                if((field_num % 2) == 0)
                                {
                                    // 선택지 스크립트
                                    info.selection.Add(field);
                                }
                                else
                                {
                                    // 선택지에 따른 처리
                                    if (int.TryParse(field, out int intField3))
                                    {
                                        info.callback.Add(CallbackManager.Instance.CallBackList(intField3));
                                    }
                                }
                            }
                            break;
                    }
                    field_num++;
                }

                // 정의되지 않은 스테이지의 경우 무시
                if(stage != eStage.None)
                {
                    // 각 행의 원소들을 처리한 후 스테이지별 csv파일에 삽입
                    InteractableInfoFiles[fileEnum][(int)stage].Add(info);
                }
                
            }
            );
    }

    public void LoadMonologueCsv(string path, int fileEnum)
    {
        if ((eCsvFile_PlayerMono)fileEnum == eCsvFile_PlayerMono.None) return;

        LoadCsv<cPlayerMonologueInfoList>(path,
            (row, info) =>
            {
                if (info == null) return;

                eStage stage = eStage.None;
                int field_num = 0;
                foreach (string field in row)
                {
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            if (int.TryParse(field, out int intField))
                            {
                                switch (intField)
                                {
                                    case 1: Debug.Log($"입력된 스테이지 코드 : {intField}"); stage = eStage.Stage1; break;
                                    case 2: Debug.Log($"입력된 스테이지 코드 : {intField}"); stage = eStage.Stage2; break;
                                    default: Debug.LogWarning($"{field}는 정의되지 않은 스테이지 코드"); break;
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]는 스테이지 코드값이 숫자가 아님");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                    }
                    field_num++;
                }

                // 정의되지 않은 스테이지의 경우 무시
                if (stage != eStage.None)
                {
                    // 각 행의 원소들을 처리한 후 스테이지별 csv파일에 삽입
                    PlayerMonologueFiles[fileEnum][(int)stage].Add(info);
                }

            }
            );
    }

    void LoadCsv<T>(string resourceName, Action<List<string>, T> RowCallBack) where T : new()
        // where T : new() : 제네릭 타입 T가 매개변수 없는 기본 생성자를 가진 클래스여야 한다는 조건을 의미
    {
        List<List<string>> csvData = new List<List<string>>();

        csvData.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
        if (csvFile != null)
        {
            Debug.Log($"{resourceName} 파일이 존재합니다.");
            string[] rows = csvFile.text.Split('\n');

            // 데이터 1차 가공 (string원소 행렬화)
            foreach (string row in rows)
            {
                string[] fields = row.Split(',');
                List<string> rowData = new List<string>(fields);

                // 각 행을 csvData 행렬에 집어넣음
                csvData.Add(rowData);
            }

            // 데이터 2차 가공 (각 자료형에 맞게 데이터를 정리)
            int row_num = 0;
            foreach (List<string> row in csvData)
            {
                if (row_num == 0) // 첫 번째 행(헤더) 스킵
                {
                    row_num++;
                    continue;
                }

                //Debug.Log($"{row_num} 행");
                T info = new T();

                RowCallBack(row, info); // 전달된 델리게이트 실행
                row_num++;
            }
        }
        else
        {
            Debug.LogWarning($"{resourceName} 파일이 존재하지 않습니다.");
        }
    }

    // 프로퍼티가 설정된 데이터멤버만 출력됨
    void PrintProperties(object obj)
    {
        Type type = obj.GetType(); // 객체의 타입 가져오기
        PropertyInfo[] properties = type.GetProperties(); // 모든 속성 가져오기
        foreach (PropertyInfo prop in properties)
        {
            // object? : nullable object(null값을 반환받을 수 있음)
            object? value = prop.GetValue(obj); // 속성 값 가져오기
            if (value is List<string>)
            {
                int i = 0;
                Debug.Log($"{prop.Name} 개수 {(value as List<string>).Count}");
                foreach(string a in (value as List<string>))
                {
                    Debug.Log($"{prop.Name}[{i++}]: {a}");
                }
            }
            else if(value is List<UnityAction>)
            {
                int i = 0;
                foreach (UnityAction callback in (value as List<UnityAction>))
                {
                    MethodInfo methodInfo = callback.Method;
                    Debug.Log($"{prop.Name}[{i++}]: {methodInfo.Name}");
                }
            }
            else
            {
                Debug.Log($"{prop.Name}: {value}");
            }
            
        }
    }
}



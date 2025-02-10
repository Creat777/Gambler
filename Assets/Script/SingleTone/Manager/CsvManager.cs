using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;




public class CsvManager : Singleton<CsvManager>
{
    // stage별로 폴더를 나눌까?


    public enum eCsvFile_InterObj
    {
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,
        Box_Empty,
        Box_Full,
        OutsideDoor
    }

    public class IteractableInfoList
    {
        public enum eSelection
        {
            NoneExist,
            Exist
            
        }
        public string speaker { get; set; }
        public string script { get; set; }
        public eSelection eSelect { get; set; }
        public List<string> selection { get; set; }
        public List<UnityAction> callback { get; set; }

        public IteractableInfoList()
        {
            selection = new List<string>();
            callback = new List<UnityAction>();
        }

    }

    // csv파일 저장을 위한 배열
    // List<IteractableInfoList> : 파일 내 스테이지별 데이터
    private List<IteractableInfoList>[][] InteractableInfoFiles;
    private List<IteractableInfoList>[][] PlayerMonologueFiles;

    public List<IteractableInfoList> GetInteractableCsv(eCsvFile_InterObj eCsv, eStage eStage)
    {
        return InteractableInfoFiles[(int)eCsv][(int)eStage];
    }

    enum eCsvFile_PlayerMono
    {

    }
    public class ItemInfo
    {

    }



    protected override void Awake()
    {
        base.Awake();

        // 
        int FileCount = Enum.GetValues(typeof(eCsvFile_InterObj)).Length;
        int StageCount = Enum.GetValues(typeof(eStage)).Length;

        InteractableInfoFiles = new List<IteractableInfoList>[FileCount][];
        PlayerMonologueFiles = new List<IteractableInfoList>[FileCount][];
        NewCsvFile(FileCount, StageCount,InteractableInfoFiles);
        NewCsvFile(FileCount, StageCount, PlayerMonologueFiles);
    }

    public void NewCsvFile(int FileCount, int StageCount, List<IteractableInfoList>[][] CsvFileInfoPerStage)
    {
        
        for (int i = 0; i < FileCount; i++)
        {
            // 각 파일들에 스테이지관리를 위한 저장공간
            CsvFileInfoPerStage[i] = new List<IteractableInfoList>[StageCount];

            for (int j = 0; j < StageCount; j++)
            {
                // 각 스테이지마다 갖는 데이터 공간
                CsvFileInfoPerStage[i][j] = new List<IteractableInfoList>(); // 리스트 초기화
            }
        }
    }

    private void Start()
    {
        PrecessCsvOfInteraction();
        //PrecessCsvOfMonologue();
    }

    private void PrecessCsvOfInteraction()
    {
        // 상호작용 객체에 대한 csv
        string path = "CSV/InteractableObject/";
        ProcessCsv<eCsvFile_InterObj>(path, InteractableInfoFiles);
        /*
        foreach (var eFileName in Enum.GetValues(typeof(eCsvFile_InterObj)))
        {
            //// 각 파일 데이터를 로드할 저장공간 추가
            //InteractableInfoFiles[eFileName.GetHashCode()] = new List<IteractableInfoList>();

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // 파일저장경로와 파일명, 파일 데이터를 로드할 메모리를 넘김
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();
                LoadInteractableCsv(FilePath, eFileCode, eStageCode);

                // 각 파일에서 행을 하나씩 뽑아서 데이터를 올바르게 처리했는지 확인
                foreach (IteractableInfoList info in InteractableInfoFiles[eFileCode][eStageCode])
                {
                    PrintProperties(info);
                }
            }
        }
        */
    }

    private void PrecessCsvOfMonologue()
    {
        // 플레이어 모놀로그에 대한 csv
        string path = "CSV/PlayerMonologue/";
        ProcessCsv<eCsvFile_PlayerMono>(path, PlayerMonologueFiles);
    }

    private void ProcessCsv<T>(string path, List<IteractableInfoList>[][] CsvFileInfoPerStage) where T : Enum
    {
        foreach (var eFileName in Enum.GetValues(typeof(T)))
        {
            //// 각 파일 데이터를 로드할 저장공간 추가
            //InteractableInfoFiles[eFileName.GetHashCode()] = new List<IteractableInfoList>();

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // 데이터 처리
            LoadInteractableCsv(FilePath, eFileCode);

            // 처리된 데이터의 확인
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();

                // 각 파일에서 행을 하나씩 뽑아서 데이터를 올바르게 처리했는지 확인
                foreach (IteractableInfoList info in CsvFileInfoPerStage[eFileCode][eStageCode])
                {
                    PrintProperties(info);
                }
            }
        }
    }

    public void LoadInteractableCsv(string path, int i)
    {

        LoadCsv<IteractableInfoList>(path,
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
                                Debug.LogWarning($"[{field}]는 스테이지 코드의 값이 숫자가 아님");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                        case 3: 
                            if(int.TryParse(field, out int intField2)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(IteractableInfoList.eSelection), intField2)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.eSelect = (IteractableInfoList.eSelection)intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField2}는 {typeof(IteractableInfoList.eSelection).Name}에 정의되지 않았음");
                                }
                            }
                            break;
                        default:
                            // 선택지가 존재하는 경우에만
                            if (info.eSelect == IteractableInfoList.eSelection.Exist)
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
                                        info.callback.Add(CallBackManager.Instance.CallBackList(intField3));
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
                    InteractableInfoFiles[i][(int)stage].Add(info);
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
            Debug.LogAssertion($"{resourceName} 파일이 존재하지 않습니다.");
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



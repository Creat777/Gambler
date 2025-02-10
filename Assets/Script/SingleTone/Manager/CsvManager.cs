using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;




public class CsvManager : Singleton<CsvManager>
{
    // stage���� ������ ������?


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

    // csv���� ������ ���� �迭
    // List<IteractableInfoList> : ���� �� ���������� ������
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
            // �� ���ϵ鿡 �������������� ���� �������
            CsvFileInfoPerStage[i] = new List<IteractableInfoList>[StageCount];

            for (int j = 0; j < StageCount; j++)
            {
                // �� ������������ ���� ������ ����
                CsvFileInfoPerStage[i][j] = new List<IteractableInfoList>(); // ����Ʈ �ʱ�ȭ
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
        // ��ȣ�ۿ� ��ü�� ���� csv
        string path = "CSV/InteractableObject/";
        ProcessCsv<eCsvFile_InterObj>(path, InteractableInfoFiles);
        /*
        foreach (var eFileName in Enum.GetValues(typeof(eCsvFile_InterObj)))
        {
            //// �� ���� �����͸� �ε��� ������� �߰�
            //InteractableInfoFiles[eFileName.GetHashCode()] = new List<IteractableInfoList>();

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // ���������ο� ���ϸ�, ���� �����͸� �ε��� �޸𸮸� �ѱ�
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();
                LoadInteractableCsv(FilePath, eFileCode, eStageCode);

                // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
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
        // �÷��̾� ���α׿� ���� csv
        string path = "CSV/PlayerMonologue/";
        ProcessCsv<eCsvFile_PlayerMono>(path, PlayerMonologueFiles);
    }

    private void ProcessCsv<T>(string path, List<IteractableInfoList>[][] CsvFileInfoPerStage) where T : Enum
    {
        foreach (var eFileName in Enum.GetValues(typeof(T)))
        {
            //// �� ���� �����͸� �ε��� ������� �߰�
            //InteractableInfoFiles[eFileName.GetHashCode()] = new List<IteractableInfoList>();

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // ������ ó��
            LoadInteractableCsv(FilePath, eFileCode);

            // ó���� �������� Ȯ��
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();

                // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
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
                    //Debug.Log($"{field_num}�� : " + field);
                    switch (field_num)
                    {
                        // ó���� �����͸� ���� Stage�� ����
                        case 0:
                            if (int.TryParse(field, out int intField))
                            {
                                switch (intField)
                                {
                                    case 1: Debug.Log($"�Էµ� �������� �ڵ� : {intField}"); stage = eStage.Stage1; break;
                                    case 2: Debug.Log($"�Էµ� �������� �ڵ� : {intField}"); stage = eStage.Stage2; break;
                                    default: Debug.LogWarning($"{field}�� ���ǵ��� ���� �������� �ڵ�"); break;
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]�� �������� �ڵ��� ���� ���ڰ� �ƴ�");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                        case 3: 
                            if(int.TryParse(field, out int intField2)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(IteractableInfoList.eSelection), intField2)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.eSelect = (IteractableInfoList.eSelection)intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField2}�� {typeof(IteractableInfoList.eSelection).Name}�� ���ǵ��� �ʾ���");
                                }
                            }
                            break;
                        default:
                            // �������� �����ϴ� ��쿡��
                            if (info.eSelect == IteractableInfoList.eSelection.Exist)
                            {
                                if((field_num % 2) == 0)
                                {
                                    // ������ ��ũ��Ʈ
                                    info.selection.Add(field);
                                }
                                else
                                {
                                    // �������� ���� ó��
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

                // ���ǵ��� ���� ���������� ��� ����
                if(stage != eStage.None)
                {
                    // �� ���� ���ҵ��� ó���� �� ���������� csv���Ͽ� ����
                    InteractableInfoFiles[i][(int)stage].Add(info);
                }
                
            }
            );
    }

    void LoadCsv<T>(string resourceName, Action<List<string>, T> RowCallBack) where T : new()
        // where T : new() : ���׸� Ÿ�� T�� �Ű����� ���� �⺻ �����ڸ� ���� Ŭ�������� �Ѵٴ� ������ �ǹ�
    {
        List<List<string>> csvData = new List<List<string>>();

        csvData.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
        if (csvFile != null)
        {
            Debug.Log($"{resourceName} ������ �����մϴ�.");
            string[] rows = csvFile.text.Split('\n');

            // ������ 1�� ���� (string���� ���ȭ)
            foreach (string row in rows)
            {
                string[] fields = row.Split(',');
                List<string> rowData = new List<string>(fields);

                // �� ���� csvData ��Ŀ� �������
                csvData.Add(rowData);
            }

            // ������ 2�� ���� (�� �ڷ����� �°� �����͸� ����)
            int row_num = 0;
            foreach (List<string> row in csvData)
            {
                if (row_num == 0) // ù ��° ��(���) ��ŵ
                {
                    row_num++;
                    continue;
                }

                //Debug.Log($"{row_num} ��");
                T info = new T();

                RowCallBack(row, info); // ���޵� ��������Ʈ ����
                row_num++;
            }
        }
        else
        {
            Debug.LogAssertion($"{resourceName} ������ �������� �ʽ��ϴ�.");
        }
    }

    // ������Ƽ�� ������ �����͸���� ��µ�
    void PrintProperties(object obj)
    {
        Type type = obj.GetType(); // ��ü�� Ÿ�� ��������
        PropertyInfo[] properties = type.GetProperties(); // ��� �Ӽ� ��������
        foreach (PropertyInfo prop in properties)
        {
            // object? : nullable object(null���� ��ȯ���� �� ����)
            object? value = prop.GetValue(obj); // �Ӽ� �� ��������
            if (value is List<string>)
            {
                int i = 0;
                Debug.Log($"{prop.Name} ���� {(value as List<string>).Count}");
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



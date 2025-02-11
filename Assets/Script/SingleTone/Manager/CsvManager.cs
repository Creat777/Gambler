using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;



public class CsvManager : Singleton<CsvManager>
{
    // csv���� ������ ���� �迭
    // List<IteractableInfoList> : ���� �� ���������� ������
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

        // ���ϰ������� None�� ����

        // ��ȣ�ۿ� ����
        int InteractionFileCount = Enum.GetValues(typeof(eCsvFile_InterObj)).Length - 1 ;
        InteractableInfoFiles = new List<cIteractableInfoList>[InteractionFileCount][];
        NewCsvFile(InteractionFileCount, StageCount, InteractableInfoFiles);

        // ���� ����
        int MonologueFileCount = Enum.GetValues(typeof(eCsvFile_PlayerMono)).Length - 1;
        PlayerMonologueFiles = new List<cPlayerMonologueInfoList>[InteractionFileCount][];
        NewCsvFile(MonologueFileCount, StageCount, PlayerMonologueFiles);
    }

    public void NewCsvFile<T>(int FileCount, int StageCount, List<T>[][] CsvFileInfoPerStage)
    {
        for (int i = 0; i < FileCount; i++)
        {
            // �� ���ϵ鿡 �������������� ���� �������
            CsvFileInfoPerStage[i] = new List<T>[StageCount];

            for (int j = 0; j < StageCount; j++)
            {
                // �� ������������ ���� ������ ����
                CsvFileInfoPerStage[i][j] = new List<T>(); // ����Ʈ �ʱ�ȭ
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
        // ��ȣ�ۿ� ��ü�� ���� csv
        string path = "CSV/InteractableObject/";
        ProcessCsv<eCsvFile_InterObj, cIteractableInfoList>(path, LoadInteractableCsv ,InteractableInfoFiles);
        
    }

    private void PrecessCsvOfMonologue()
    {
        // �÷��̾� ���α׿� ���� csv
        string path = "CSV/PlayerMonologue/";
        ProcessCsv<eCsvFile_PlayerMono, cPlayerMonologueInfoList>(path, LoadMonologueCsv, PlayerMonologueFiles);
    }

    private void ProcessCsv<T_enum, T_class>(string path, Action<string, int> LoadIndividualCsv,List<T_class>[][] CsvFileInfoPerStage) where T_enum : Enum
    {
        foreach (var eFileName in Enum.GetValues(typeof(T_enum)))
        {
            // ������ ������ None�� ����
            if ((int)eFileName == Enum.GetValues(typeof(T_enum)).Length - 1) continue;

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // ������ ó��
            LoadIndividualCsv(FilePath, eFileCode);

            // ó���� �������� Ȯ��
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();

                // �������� �������� �ڵ��� ��츸 ����
                if(eStageCode != 0)
                {
                    // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
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
                                Debug.LogWarning($"[{field}]�� �������� �ڵ尪�� ���ڰ� �ƴ�");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                        case 3: 
                            if(int.TryParse(field, out int intField2)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(eSelection), intField2)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.eSelect = (eSelection)intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField2}�� {typeof(eSelection).Name}�� ���ǵ��� �ʾ���");
                                }
                            }
                            break;
                        default:
                            // �������� �����ϴ� ��쿡��
                            if (info.eSelect == eSelection.Exist)
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
                                        info.callback.Add(CallbackManager.Instance.CallBackList(intField3));
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
                                Debug.LogWarning($"[{field}]�� �������� �ڵ尪�� ���ڰ� �ƴ�");
                            }
                            break;

                        case 1: info.speaker = field; break;
                        case 2: info.script = field; break;
                    }
                    field_num++;
                }

                // ���ǵ��� ���� ���������� ��� ����
                if (stage != eStage.None)
                {
                    // �� ���� ���ҵ��� ó���� �� ���������� csv���Ͽ� ����
                    PlayerMonologueFiles[fileEnum][(int)stage].Add(info);
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
            Debug.LogWarning($"{resourceName} ������ �������� �ʽ��ϴ�.");
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



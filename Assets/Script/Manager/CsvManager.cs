using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;



public class CsvManager : Singleton<CsvManager>
{
    // ������
    [SerializeField] private ItemTable itemPlusInfoTable;

    // ��ũ��Ʈ
    // OnlyOneLives PlayerInfo �ڷᱸ��
    private Dictionary<eCharacter,cCharacterInfo> CharacterInfoDict;

    // ��ȣ�ۿ� ��ü �ڷᱸ��
    private List<cTextScriptInfo>[,] InteractableInfoFiles;

    // ������ ��ü �ڷᱸ��
    private Dictionary<eItemSerialNumber, cItemInfo> ItemInfoDict;

    public Dictionary<eCharacter, cCharacterInfo> GetCharacterInfoDict()
    {
        return CharacterInfoDict;
    }
    public cCharacterInfo GetCharacterInfo(eCharacter characterEnum)
    {
        return CharacterInfoDict[characterEnum];
    }

    public List<cTextScriptInfo> GetTextScript(eTextScriptFile eCsv, eStage eStage)
    {
        return InteractableInfoFiles[(int)eCsv,(int)eStage];
    }

    public cItemInfo GetItemInfo(eItemSerialNumber eSerialNumber)
    {
        return ItemInfoDict[eSerialNumber];
    }

    

    protected override void Awake()
    {
        base.Awake();
        NewCsvStorage();

        TotalCsvProCess();
    }
    
    private void NewCsvStorage()
    {
        // �������� ����
        int StageCount = Enum.GetValues(typeof(eStage)).Length;

        // ���ϰ������� None�� ����

        // ��ȣ�ۿ� ����
        int textScriptFileCount = Enum.GetValues(typeof(eTextScriptFile)).Length - 1;
        InteractableInfoFiles = new List<cTextScriptInfo>[textScriptFileCount, StageCount];

        for(int i = 0; i < InteractableInfoFiles.Length; i++)
        {
            InteractableInfoFiles[i / StageCount, i% StageCount]
                = new List<cTextScriptInfo>();
        }

        // ������ ����
        ItemInfoDict = new Dictionary<eItemSerialNumber, cItemInfo>();

        // OnlyOneLives �÷��̾� ���� ����
        CharacterInfoDict = new Dictionary<eCharacter, cCharacterInfo>();
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

    public void TotalCsvProCess()
    {
        ProcessCsvOfCharacterInfo(); // ĳ���� ������ �ε��� �Ŀ� TextCsvó���� ������
        ProcessCsvOfTextScript();
        ProcessCsvOfItem();
    }

    private void ProcessCsvOfTextScript()
    {
        // ��ȣ�ۿ� ��ü�� ���� csv
        string path = "CSV/TextScript/";
        ProcessScriptCsv<eTextScriptFile, cTextScriptInfo>(path, LoadTextCsv ,InteractableInfoFiles);
        
    }

    private void ProcessCsvOfItem()
    {
        string fileNamePath = "CSV/Item/Item";

        LoadCsv<cItemInfo>(fileNamePath,
            (row, itemInfo) =>
            {
                // ��������� �Ҵ���� ���� ���
                if (itemInfo == null) return;

                int field_num = 0;

                // �� ���� ó�� ����
                foreach (string field in row)
                {
                    int intField = 0;
                    //Debug.Log($"{field_num}�� : " + field);
                    switch (field_num)
                    {
                        // ó���� �����͸� ���� Stage�� ����
                        case 0:
                            if (eItemSerialNumber.TryParse(field, out eItemSerialNumber enumField))
                            {
                                itemInfo.serialNumber = enumField;
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]�� ������ �ø����ȣ�� �� �� ����");
                            }
                            break;

                        case 1: itemInfo.name = field; break;

                        case 2:
                            {
                                //'_'�� �ٹٲ����� ��ȯ
                                string[] scripts = field.Split('_');
                                itemInfo.description = string.Join('\n', scripts);
                            } break;

                        case 3:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if(intField == 0)
                                {
                                    itemInfo.isAvailable = false;
                                }
                                else if(intField == 1)
                                {
                                    itemInfo.isAvailable = true;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� ���ǵ��� ���� ������");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� ������ ĳ���� �Ұ�");
                            }
                            break;

                        // �Ҹ� ����
                        case 4:

                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (intField == 0)
                                {
                                    itemInfo.isConsumable = false;
                                }
                                else if (intField == 1)
                                {
                                    itemInfo.isConsumable = true;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� ���ǵ��� ���� ������");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� ������ ĳ���� �Ұ�");
                            }
                            break;

                            // ���� ������
                        case 5:
                            if (float.TryParse(field, out float floatField))
                            {
                                itemInfo.value_Use = floatField;
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]�� ��밪�� �� �� ����");
                            }
                            break;

                            // �ǸŰ��� ����
                        case 6:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (intField == 0)
                                {
                                    itemInfo.isForSale = false;
                                }
                                else if (intField == 1)
                                {
                                    itemInfo.isForSale = true;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� ���ǵ��� ���� ������");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� ������ ĳ���� �Ұ�");
                            }
                            break;

                            // �ǸŽ� ����
                        case 7:
                            if (int.TryParse(field, out int intField2))
                            {
                                itemInfo.value_Sale = intField2;
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]�� ��밪�� �� �� ����");
                            }
                            break;

                        default: Debug.LogAssertion($"{field}�� �߸��� �׸� ��ġ��");
                            break;
                    }
                    field_num++;
                }

                // ������ ��ųʸ��� ����
                if(ItemInfoDict.ContainsKey(itemInfo.serialNumber) == false)
                {
                    ItemInfoDict.Add(itemInfo.serialNumber, itemInfo);
                }
                else
                {
                    ItemInfoDict[itemInfo.serialNumber] = itemInfo;
                }
                
            }
            );
        
        // �߰����� ������ ó��
        foreach(ItemPlusInfo itemPlusInfo in itemPlusInfoTable.item_PlusInfoList)
        {
            eItemSerialNumber serailNumber = itemPlusInfo.serialNumber;

            // �����ۿ� �ش��ϴ� �������� ����
            if(ItemInfoDict.ContainsKey(serailNumber))
            {
                ItemInfoDict[serailNumber].itemPrefab = itemPlusInfo.itemPrefab;
            }
            else
            {
                Debug.LogAssertion($"serailNumber{serailNumber}�� ��ųʸ� Ű�� �����ϴ�.");
            }

            // ��� ������ ���
            if (ItemInfoDict[serailNumber].isAvailable)
            {

                // �ݹ鸮��Ʈ���� �����ۿ� �ش��ϴ� �ݹ��Լ��� �����ϵ��� ��
                ItemInfoDict[serailNumber].itemCallback +=
                    CallbackManager.Instance.CallBackList_Item_Quest(itemPlusInfo.itemCallbackIndex);
            }

        }
        
        // ó���� �������� Ȯ��
        foreach(eItemSerialNumber serail in Enum.GetValues(typeof(eItemSerialNumber)))
        {
            if (serail == eItemSerialNumber.None) continue;

            //Debug.Log($"csv Item({ItemInfoDict[serail].name}) ����Ʈ ����");
            //PrintProperties(ItemInfoDict[serail]);
        }
    }

    private void ProcessCsvOfCharacterInfo()
    {
        string fileNamePath = "CSV/Character/CharacterInfo";

        // ������ ó��
        LoadCsv<cCharacterInfo>(
            fileNamePath,
            (row, CharacterInfo) =>
            {
                // ��������� �Ҵ���� ���� ���
                if (CharacterInfo == null) return;

                int intField = 0;

                int field_num = 0;
                // �� ���� ó�� ����
                foreach (string field in row)
                {
                    switch (field_num)
                    {
                        // ó���� �����͸� ���� Stage�� ����
                        case 0: 
                            if(int.TryParse(field, out intField))
                            {
                                if(Enum.IsDefined(typeof(eCharacter), intField))
                                {
                                    CharacterInfo.CharaterIndex = (eCharacter)intField; break;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{intField}�� eCharacter�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                Debug.LogAssertion($"{field}�� ������ �ƴ�");
                            }
                            break;

                        case 1: CharacterInfo.CharacterName = field; break;
                        case 2: CharacterInfo.CharacterAge = field; break;
                        case 3: CharacterInfo.CharacterClan = field; break;
                        case 4: CharacterInfo.CharacterFeature = field; break;
                    }
                    field_num++;
                }

                // Character ��ųʸ��� ����
                if (CharacterInfoDict.ContainsKey(CharacterInfo.CharaterIndex) == false)
                {
                    CharacterInfoDict.Add(CharacterInfo.CharaterIndex, CharacterInfo);
                }
                else
                {
                    CharacterInfoDict[CharacterInfo.CharaterIndex] = CharacterInfo;
                }
            }
            );

        // ó���� �������� Ȯ��
        foreach (var row in CharacterInfoDict)
        {
            PrintProperties(row);
        }
    }

    private void ProcessScriptCsv<T_enum, T_class>(string path, Action<string, int> LoadCsv,
        List<T_class>[,] CsvFileInfoPerStage) where T_enum : Enum
    {
        foreach (var eFileName in Enum.GetValues(typeof(T_enum)))
        {
            // ������ ������ None�� ����
            if ((int)eFileName == Enum.GetValues(typeof(T_enum)).Length - 1) continue;

            string FilePath = path + eFileName.ToString();
            int eFileCode = eFileName.GetHashCode();

            // ������ ó��
            LoadCsv(FilePath, eFileCode);

            // ó���� �������� Ȯ��
            foreach (var eStage in Enum.GetValues(typeof(eStage)))
            {
                int eStageCode = eStage.GetHashCode();

                // �������� �������� �ڵ��� ��츸 ����
                if(eStageCode != 0)
                {
                    // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
                    foreach (T_class info in CsvFileInfoPerStage[eFileCode, eStageCode])
                    {
                        //Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) ����Ʈ ����");
                        //PrintProperties(info);
                    }
                }    
                
            }
        }
    }


    public void LoadTextCsv(string path, int fileEnum)
    {
        if ((eTextScriptFile)fileEnum == eTextScriptFile.None) return;

        LoadCsv<cTextScriptInfo>(path,
            (row, info)=>
            {
                if (info == null) return;

                eStage stage = eStage.None;

                int intField = 0;
                int field_num = 0;
                foreach (string field in row)
                {
                    //Debug.Log($"{field_num}�� : " + field);
                    switch (field_num)
                    {
                        // ó���� �����͸� ���� Stage�� ����
                        case 0:
                            if (int.TryParse(field, out intField))
                            {
                                switch (intField)
                                {
                                    case 1: 
                                        //Debug.Log($"�Էµ� �������� �ڵ� : {intField}"); 
                                        stage = eStage.Stage1; break;
                                    case 2: 
                                        //Debug.Log($"�Էµ� �������� �ڵ� : {intField}"); 
                                        stage = eStage.Stage2; break;
                                    case 3: 
                                        //Debug.Log($"�Էµ� �������� �ڵ� : {intField}"); 
                                        stage = eStage.Stage3; break;
                                    default: 
                                        Debug.LogWarning($"{field}�� ���ǵ��� ���� �������� �ڵ�"); 
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]�� �������� �ڵ尪�� ���ڰ� �ƴ�");
                            }
                            break;

                        // ĳ���� �ε��� ó��
                        case 1:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(eCharacter), intField)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.characterEnum = (eCharacter)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� {typeof(eCharacter).Name}�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� �������� �ƴմϴ�.");
                            }
                            break;

                        // ĳ������ ���̾�α׾����� �ε��� ó��
                        case 2:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                info.DialogueIconIndex = intField;
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� �������� �ƴմϴ�.");
                            }
                            break;
                        // ��ũ��Ʈ ó��
                        case 3: info.script = field; break;

                        // �����ݹ� ó��
                        case 4:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(eHasEndCallback), intField)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.hasEndCallback = (eHasEndCallback)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� {typeof(eHasEndCallback).Name}�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� �������� �ƴմϴ�.");
                            }
                            break;
                        // �����ݹ��� �����ϴ� ��츸(�⺻�� NO)
                        case 5:
                            if(info.hasEndCallback == eHasEndCallback.yes)
                            {
                                if (int.TryParse(field, out intField))
                                {
                                    info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_Text(intField));
                                }
                                else
                                {
                                    Debug.LogAssertion($"{field}�� ������ �Ľ��� �� ����");
                                }
                            }
                            break;

                        // ������ ó��
                        case 6: 
                            if(int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(eHasSelection), intField)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.hasSelection = (eHasSelection)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}�� {typeof(eHasSelection).Name}�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� �������� �ƴմϴ�.");
                            }
                            break;
                        // �������� �����ϴ� ��쿡��(�⺻�� NO)
                        default:
                            if (info.hasSelection == eHasSelection.yes)
                            {
                                if((field_num % 2) == 1)
                                {
                                    // ������ ��ũ��Ʈ
                                    info.selectionScript.Add(field);
                                }
                                else
                                {
                                    // �������� ���� ó��
                                    if (int.TryParse(field, out int intField3))
                                    {
                                        info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_Text(intField3));
                                    }
                                    else
                                    {
                                        Debug.LogAssertion($"{field}�� ������ �Ľ��� �� ����");
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
                    InteractableInfoFiles[fileEnum,(int)stage].Add(info);
                }
                
            }
            );
    }

    public void LoadCsv<T>(string resourceName, Action<List<string>, T> RowCallback) where T : new()
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

                RowCallback(row, info); // ���޵� ��������Ʈ ����
                row_num++;
            }
        }
        else
        {
            Debug.LogAssertion($"{resourceName} ������ �������� �ʽ��ϴ�.");
        }
    }

    // ������Ƽ�� ������ �����͸���� ��µ�
    public void PrintProperties(object obj)
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



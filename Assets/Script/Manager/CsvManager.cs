using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;
using UnityEditor.SceneManagement;



public class CsvManager : Singleton<CsvManager>
{
    // ������
    [SerializeField] private ItemTable itemPlusInfoTable;

    // ��ũ��Ʈ
    // OnlyOneLives PlayerInfo �ڷᱸ��
    private Dictionary<eCharacterType,cCharacterInfo> CharacterInfo_Dict;

    // TextList�� �����ϱ����� �ڷᱸ��
    // ���ٹ� : TextInfoDicts[eTextScriptFile.None][eStage.None]
    private Dictionary<eTextScriptFile , Dictionary<eStage, List<cTextScriptInfo>>> TextScriptInfo_2dDict;

    // ������ ��ü �ڷᱸ��
    private Dictionary<eItemType, cItemInfo> ItemInfo_Dict;

    public Dictionary<eCharacterType, cCharacterInfo> GetCharacterInfoDict()
    {
        return CharacterInfo_Dict;
    }
    public cCharacterInfo GetCharacterInfo(eCharacterType characterEnum)
    {
        return CharacterInfo_Dict[characterEnum];
    }

    public List<cTextScriptInfo> GetTextScript(eTextScriptFile File, eStage Stage)
    {
        // �ش� ���������� ����� �ؽ�Ʈ�� ������ ��������
        if (TextScriptInfo_2dDict[File][Stage].Count !=0)
        {
            return TextScriptInfo_2dDict[File][Stage];
        }

        //�׷��� ������ ����Ʈ��(0)�� �ؽ�Ʈ�� ������
        else
        {
            return TextScriptInfo_2dDict[File][eStage.Defualt];
        }
        
    }

    public cItemInfo GetItemInfo(eItemType itemType)
    {
        return ItemInfo_Dict[itemType];
    }

    

    protected override void Awake()
    {
        base.Awake();
        NewCsvStorage();

        TotalCsvProCess();
    }
    
    private void NewCsvStorage()
    {
        // OnlyOneLives �÷��̾� ���� ����
        CharacterInfo_Dict = new Dictionary<eCharacterType, cCharacterInfo>();

        // text ����
        // ��ü ��Ʈ �޸� Ȯ��
        TextScriptInfo_2dDict = new Dictionary<eTextScriptFile, Dictionary<eStage, List<cTextScriptInfo>>>();
        foreach(eTextScriptFile file in Enum.GetValues(typeof(eTextScriptFile)))
        {
            // ����Ű�� ����� �޸� Ȯ��
            TextScriptInfo_2dDict.Add(file, new Dictionary<eStage, List<cTextScriptInfo>>());

            foreach(eStage stage in Enum.GetValues(typeof(eStage)))
            {
                // ��������Ű�� ����� �޸� Ȯ��
                TextScriptInfo_2dDict[file].Add(stage, new List<cTextScriptInfo>());
            }
        }

        // ������ ����
        ItemInfo_Dict = new Dictionary<eItemType, cItemInfo>();

        
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
        string path = "CSV/TextScript/Interactable/";
        foreach (eTextScriptFile eFileName in Enum.GetValues(typeof(eTextScriptFile)))
        {
            if(eFileName == eTextScriptFile.None) continue;

            if(eFileName == eTextScriptFile.PlayerMonologue)
            {
                path = "CSV/TextScript/NoneInteractable/";
            }

            string FilePath = path + eFileName.ToString();

            // ������ ó��
            LoadTextCsv(FilePath, eFileName);

            // ó���� �������� Ȯ��
            foreach (eStage eStageEnum in Enum.GetValues(typeof(eStage)))
            {
                // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
                foreach (cTextScriptInfo info in TextScriptInfo_2dDict[eFileName][eStageEnum])
                {
                    //Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) ����Ʈ ����");
                    //PrintProperties(info);
                }

            }
        }
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
                            if (eItemType.TryParse(field, out eItemType enumField))
                            {
                                itemInfo.serialNumber = enumField;
                            }
                            else
                            {
                                if(field != "")
                                {
                                    Debug.LogWarning($"[{field}]�� ������ �ø����ȣ�� �� �� ����");
                                }
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
                            if(itemInfo.isAvailable)
                            {
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
                            }
                            break;

                            // ���� ������
                        case 5:
                            if(itemInfo.isAvailable)
                            {
                                if (float.TryParse(field, out float floatField))
                                {
                                    itemInfo.value_Use = floatField;
                                }
                                else
                                {
                                    Debug.LogWarning($"[{field}]�� ��밪�� �� �� ����");
                                }
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
                            if(itemInfo.isForSale)
                            {
                                if (int.TryParse(field, out int intField2))
                                {
                                    itemInfo.value_Sale = intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"[{field}]�� ��밪�� �� �� ����");
                                }
                            }
                            
                            break;

                        default: Debug.LogAssertion($"{field}�� �߸��� �׸� ��ġ��");
                            break;
                    }
                    field_num++;
                }

                // ������ ��ųʸ��� ����
                if(ItemInfo_Dict.ContainsKey(itemInfo.serialNumber) == false)
                {
                    ItemInfo_Dict.Add(itemInfo.serialNumber, itemInfo);
                }
                else
                {
                    ItemInfo_Dict[itemInfo.serialNumber] = itemInfo;
                }
                
            }
            );
        
        // �߰����� ������ ó��
        foreach(ItemPlusInfo itemPlusInfo in itemPlusInfoTable.item_PlusInfoList)
        {
            eItemType itemType = itemPlusInfo.serialNumber;

            // �����ۿ� �ش��ϴ� �������� ����
            if(ItemInfo_Dict.ContainsKey(itemType))
            {
                ItemInfo_Dict[itemType].itemPrefab = itemPlusInfo.itemPrefab;
            }
            else
            {
                Debug.LogAssertion($"serailNumber{itemType}�� ��ųʸ� Ű�� �����ϴ�.");
            }

            // ��� ������ ���
            if (ItemInfo_Dict[itemType].isAvailable)
            {

                // �ݹ鸮��Ʈ���� �����ۿ� �ش��ϴ� �ݹ��Լ��� �����ϵ��� ��
                ItemInfo_Dict[itemType].itemCallback +=
                    CallbackManager.Instance.CallBackList_Item(itemPlusInfo.itemCallbackIndex);
            }

        }
        
        // ó���� �������� Ȯ��
        foreach(eItemType serail in Enum.GetValues(typeof(eItemType)))
        {
            if (serail == eItemType.None) continue;

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
                                if(Enum.IsDefined(typeof(eCharacterType), intField))
                                {
                                    CharacterInfo.CharaterIndex = (eCharacterType)intField; break;
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
                if (CharacterInfo_Dict.ContainsKey(CharacterInfo.CharaterIndex) == false)
                {
                    CharacterInfo_Dict.Add(CharacterInfo.CharaterIndex, CharacterInfo);
                }
                else
                {
                    CharacterInfo_Dict[CharacterInfo.CharaterIndex] = CharacterInfo;
                }
            }
            );

        // ó���� �������� Ȯ��
        //foreach(eCharacter key in CharacterInfo_Dict.Keys)
        //{
        //    PrintProperties(CharacterInfo_Dict[key]);
        //}
    }

    //private void ProcessScriptCsv<T_enum, T_class>(string path, Action<string, int> LoadCsv,
    //    Dictionary<eTextScriptFile, Dictionary<eStage, List<T_class>>> CsvFileInfoPerStage) where T_enum : Enum
    //{
    //    foreach (var eFileName in Enum.GetValues(typeof(T_enum)))
    //    {
    //        // ������ ������ None�� ����
    //        if ((int)eFileName == Enum.GetValues(typeof(T_enum)).Length - 1) continue;

    //        string FilePath = path + eFileName.ToString();
    //        int eFileCode = eFileName.GetHashCode();

    //        // ������ ó��
    //        LoadCsv(FilePath, eFileCode);

    //        // ó���� �������� Ȯ��
    //        foreach (var eStage in Enum.GetValues(typeof(eStage)))
    //        {
    //            int eStageCode = eStage.GetHashCode();

    //            // �������� �������� �ڵ��� ��츸 ����
    //            if(eStageCode != 0)
    //            {
    //                // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
    //                foreach (T_class info in CsvFileInfoPerStage[eFileCode, eStageCode])
    //                {
    //                    //Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) ����Ʈ ����");
    //                    //PrintProperties(info);
    //                }
    //            }    
                
    //        }
    //    }
    //}


    public void LoadTextCsv(string path, eTextScriptFile fileEnum)
    {
        LoadCsv<cTextScriptInfo>(path,
            (row, info)=>
            {
                if (info == null) return;

                int intStage = int.MinValue;

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
                                if(Enum.IsDefined(typeof(eStage), intField))
                                {
                                    intStage = intField;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{path}�� \"{field}\"�� eStage�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                if(field != "")
                                {
                                    Debug.LogWarning($"����{fileEnum.ToString()}�� [{field}]�� �������� �ڵ尪�� ���ڰ� �ƴ�");
                                }
                            }
                            break;

                        // ĳ���� �ε��� ó��
                        case 1:
                            if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                            {
                                if (Enum.IsDefined(typeof(eCharacterType), intField)) // �������� enum�� ���ǵǾ����� Ȯ��
                                {
                                    info.characterEnum = (eCharacterType)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"����{fileEnum.ToString()}�� [{intField}]�� {typeof(eCharacterType).Name}�� ���ǵ��� �ʾ���");
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
                                Debug.LogWarning($"����{fileEnum.ToString()}�� {field}�� �������� �ƴմϴ�.");
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
                                    info.endCallback = CallbackManager.Instance.CallBackList_Text(intField);
                                }
                                else
                                {
                                    Debug.LogAssertion($"����{fileEnum.ToString()}�� {field}�� ������ �Ľ��� �� ����");
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
                                // ����ִ°�� ����
                                if (field.Length > 0)
                                {
                                    if ((field_num % 2) == 1)
                                    {

                                        // ������ ��ũ��Ʈ
                                        info.selectionScript.Add(field);
                                    }
                                    else
                                    {
                                        // �������� ���� ó��
                                        if (int.TryParse(field, out intField))
                                        {
                                            info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_Text(intField));
                                        }
                                        else
                                        {
                                            Debug.LogAssertion($"����{fileEnum.ToString()}�� {field}�� ������ �Ľ��� �� ����");
                                        }
                                    }
                                }
                                
                            }
                            break;
                    }
                    
                    field_num++;
                }

                // ���ǵ��� ���� ���������� ��� ����
                if(Enum.IsDefined(typeof(eStage), intStage)) // intStage�� ���� �Ҵ���� �ʾ����� int.MinValue�� ����Ǿ�����
                {
                    // �� ���� ���ҵ��� ó���� �� ���������� csv���Ͽ� ����
                    TextScriptInfo_2dDict[fileEnum][ (eStage)intStage].Add(info);
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



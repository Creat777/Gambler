using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;
using System.IO;



public class CsvManager : Singleton<CsvManager>
{
    // ������
    [SerializeField] private ItemTable itemPlusInfoTable;

    // ��ũ��Ʈ
    // OnlyOneLives PlayerInfo �ڷᱸ��
    private Dictionary<eCharacterType,cCharacterInfo> CharacterInfo_Dict;

    // ���� ���������� ���� TextList�� �����ϱ����� �ڷᱸ��
    // ���ٹ� : TextInfoDicts[eTextScriptFile.None][eStage.None]
    private Dictionary<eTextScriptFile , Dictionary<eStage, List<cTextScriptInfo>>> TextScriptInfoList_2dDict;

    // onlyOneLives�� ���� ������ ���� TextList�� �����ϱ����� �ڷᱸ��
    private Dictionary<eOOLProgress, List<cTextScriptInfo>> TextScriptInfoList_OnlyOneLives_Dict;

    // ������ ��ü �ڷᱸ��
    private Dictionary<eItemType, cItemInfo> ItemInfo_Dict;

    // ����Ʈ ��ü �ڷᱸ��
    private Dictionary<eQuestType, cQuestInfo> QuestInfo_Dict;



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
        if (TextScriptInfoList_2dDict[File][Stage].Count !=0)
        {
            return TextScriptInfoList_2dDict[File][Stage];
        }

        //�׷��� ������ ����Ʈ��(0)�� �ؽ�Ʈ�� ������
        else
        {
            return TextScriptInfoList_2dDict[File][eStage.Defualt];
        }
        
    }
    public List<cTextScriptInfo> GetTextScript(eOOLProgress progress)
    {
        return TextScriptInfoList_OnlyOneLives_Dict[progress];
    }

    public cItemInfo GetItemInfo(eItemType itemType)
    {
        return ItemInfo_Dict[itemType];
    }

    public cQuestInfo GetQuestInfo(eQuestType questType)
    {
        return QuestInfo_Dict[questType];
    }



    protected override void Awake()
    {
        base.Awake();
        NewCsvStorage();
    }

    private void Start()
    {
        TotalCsvProCess();
    }

    private void NewCsvStorage()
    {
        // OnlyOneLives �÷��̾� ���� ����
        CharacterInfo_Dict = new Dictionary<eCharacterType, cCharacterInfo>();

        // text ����
        // ��ü ��Ʈ �޸� Ȯ��
        TextScriptInfoList_2dDict = new Dictionary<eTextScriptFile, Dictionary<eStage, List<cTextScriptInfo>>>();
        foreach(eTextScriptFile file in Enum.GetValues(typeof(eTextScriptFile)))
        {
            // ����Ű�� ����� �޸� Ȯ��
            TextScriptInfoList_2dDict.Add(file, new Dictionary<eStage, List<cTextScriptInfo>>());

            foreach(eStage stage in Enum.GetValues(typeof(eStage)))
            {
                // ��������Ű�� ����� �޸� Ȯ��
                TextScriptInfoList_2dDict[file].Add(stage, new List<cTextScriptInfo>());
            }
        }

        TextScriptInfoList_OnlyOneLives_Dict = new Dictionary<eOOLProgress, List<cTextScriptInfo>>();
        foreach (eOOLProgress progress in Enum.GetValues(typeof(eOOLProgress)))
        {
            // ���μ��� Ű�� ����� �޸� Ȯ��
            TextScriptInfoList_OnlyOneLives_Dict.Add(progress, new List<cTextScriptInfo>());
        }


        // ������ ����
        ItemInfo_Dict = new Dictionary<eItemType, cItemInfo>();

        // ����Ʈ ����
        QuestInfo_Dict = new Dictionary<eQuestType, cQuestInfo>();
    }
    

    //public void NewCsvFile<T>(int FileCount, int StageCount, List<T>[][] CsvFileInfoPerStage)
    //{
    //    for (int i = 0; i < FileCount; i++)
    //    {
    //        // �� ���ϵ鿡 �������������� ���� �������
    //        CsvFileInfoPerStage[i] = new List<T>[StageCount];

    //        for (int j = 0; j < StageCount; j++)
    //        {
    //            // �� ������������ ���� ������ ����
    //            CsvFileInfoPerStage[i][j] = new List<T>(); // ����Ʈ �ʱ�ȭ
    //        }
    //    }
    //}

    /// <summary>
    /// callbackManager�� ����ϴ� start���� �����ؾ���
    /// </summary>
    public void TotalCsvProCess()
    {
        ProcessCsvOfCharacterInfo(); // ĳ���� ������ �ε��� �Ŀ� TextCsvó���� ������
        ProcessCsvOfTextScript();
        ProcessCsvOfTextScript_OnlyOneLives();
        ProcessCsvOfItemInfo();
        ProcessCsvOfQuestInfo();
        ProcessCsvOfQuestDescription();
    }

    private void ProcessCsvOfCharacterInfo()
    {
        string path = Path.Combine("CSV", "Character", "CharacterInfo");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
        }


        // ������ ó��
        LoadCsv<cCharacterInfo>(
            path,
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
                            if (int.TryParse(field, out intField))
                            {
                                if (Enum.IsDefined(typeof(eCharacterType), intField))
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

    private void ProcessCsvOfTextScript()
    {
        string folderPath = Path.Combine("CSV", "TextScript", "Interactable");
        string path = string.Empty;

        foreach (eTextScriptFile eFileName in Enum.GetValues(typeof(eTextScriptFile)))
        {
            if(eFileName == eTextScriptFile.None) continue;

            if (eFileName == eTextScriptFile.PlayerMonologue)
            {
                //path = "CSV/TextScript/NoneInteractable";
                folderPath = Path.Combine("CSV", "TextScript", "NoneInteractable");
            }

            path = Path.Combine(folderPath, eFileName.ToString());

            if (Application.platform == RuntimePlatform.Android)
            {
                path = Path.Combine(Application.streamingAssetsPath, path);
            }

            // ������ ó��
            LoadTextCsv(path, eFileName);

            // ó���� �������� Ȯ��
            foreach (eStage eStageEnum in Enum.GetValues(typeof(eStage)))
            {
                // �� ���Ͽ��� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
                foreach (cTextScriptInfo info in TextScriptInfoList_2dDict[eFileName][eStageEnum])
                {
                    //Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) ����Ʈ ����");
                    //PrintProperties(info);
                }

            }
        }
    }

    private void ProcessCsvOfTextScript_OnlyOneLives()
    {
        //string path = "CSV/TextScript/OnlyOneLivesProcedure";

        string path = Path.Combine("CSV","TextScript","OnlyOneLivesProcedure");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
        }

        LoadCsv<cTextScriptInfo>(path,
            (row, info) =>
            {
                if (info == null) return;

                int intProgress = int.MinValue;

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
                                if (Enum.IsDefined(typeof(eOOLProgress), intField))
                                {
                                    intProgress = intField;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{path}�� \"{field}\"�� eOnlyOneLivesProgress�� ���ǵ��� �ʾ���");
                                }
                            }
                            else
                            {
                                if (field != "")
                                {
                                    Debug.LogWarning($"[{field}]�� �������� �ƴ�");
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
                                    Debug.LogWarning($"[{intField}]�� {typeof(eCharacterType).Name}�� ���ǵ��� �ʾ���");
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
                                Debug.LogWarning($"[{field}]�� �������� �ƴմϴ�.");
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
                            if (info.hasEndCallback == eHasEndCallback.yes)
                            {
                                if (int.TryParse(field, out intField))
                                {
                                    info.endCallback = CallbackManager.Instance.CallbackList_OnlyOneLivesText(intField);
                                }
                                else
                                {
                                    Debug.LogAssertion($"[{field}]�� �������� �ƴ�");
                                }
                            }
                            break;

                        //// ������ ó��
                        //case 6:
                        //    if (int.TryParse(field, out intField)) // ���ڿ��� ���������� ĳ����
                        //    {
                        //        if (Enum.IsDefined(typeof(eHasSelection), intField)) // �������� enum�� ���ǵǾ����� Ȯ��
                        //        {
                        //            info.hasSelection = (eHasSelection)intField;
                        //        }
                        //        else
                        //        {
                        //            Debug.LogWarning($"{intField}�� {typeof(eHasSelection).Name}�� ���ǵ��� �ʾ���");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        Debug.LogWarning($"{field}�� �������� �ƴմϴ�.");
                        //    }
                        //    break;
                        //// �������� �����ϴ� ��쿡��(�⺻�� NO)
                        //default:
                        //    if (info.hasSelection == eHasSelection.yes)
                        //    {
                        //        // ����ִ°�� ����
                        //        if (field.Length > 0)
                        //        {
                        //            if ((field_num % 2) == 1)
                        //            {

                        //                // ������ ��ũ��Ʈ
                        //                info.selectionScript.Add(field);
                        //            }
                        //            else
                        //            {
                        //                // �������� ���� ó��
                        //                if (int.TryParse(field, out intField))
                        //                {
                        //                    info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_DefaultText(intField));
                        //                }
                        //                else
                        //                {
                        //                    Debug.LogAssertion($"[{field}]�� �������� �ƴ�");
                        //                }
                        //            }
                        //        }

                        //    }
                        //    break;
                    }

                    field_num++;
                }

                // ���ǵ��� ���� Progress�� ��� ����
                if (Enum.IsDefined(typeof(eOOLProgress), intProgress)) // Progress�� ���� �Ҵ���� �ʾ����� int.MinValue�� ����Ǿ�����
                {
                    // �� ���� ���ҵ��� ó���� �� ���������� csv���Ͽ� ����
                    TextScriptInfoList_OnlyOneLives_Dict[(eOOLProgress)intProgress].Add(info);
                }
            }
            );

        foreach (eOOLProgress progress in Enum.GetValues(typeof(eOOLProgress)))
        {
            // �� �������� ���� �ϳ��� �̾Ƽ� �����͸� �ùٸ��� ó���ߴ��� Ȯ��
            foreach (cTextScriptInfo info in TextScriptInfoList_OnlyOneLives_Dict[progress])
            {
                Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) ����Ʈ ����");
                PrintProperties(info);
            }
        }
    }

    private void ProcessCsvOfItemInfo()
    {
        //string path = "CSV/Item/Item";

        string path = Path.Combine("CSV","Item","Item");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
        }
        LoadCsv<cItemInfo>(path,
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
                                itemInfo.type = enumField;
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
                if(ItemInfo_Dict.ContainsKey(itemInfo.type) == false)
                {
                    ItemInfo_Dict.Add(itemInfo.type, itemInfo);
                }
                else
                {
                    ItemInfo_Dict[itemInfo.type] = itemInfo;
                }
                
            }
            );
        
        // �߰����� ������ ó��
        foreach(ItemPlusInfo itemPlusInfo in itemPlusInfoTable.item_PlusInfoList)
        {
            eItemType itemType = itemPlusInfo.type;

            //// �����ۿ� �ش��ϴ� �������� ����
            //if(ItemInfo_Dict.ContainsKey(itemType))
            //{
            //    ItemInfo_Dict[itemType].itemPrefab = itemPlusInfo.itemPrefab;
            //}
            //else
            //{
            //    Debug.LogAssertion($"serailNumber{itemType}�� ��ųʸ� Ű�� �����ϴ�.");
            //}

            // ��� ������ ���
            if (ItemInfo_Dict[itemType].isAvailable)
            {

                // �ݹ鸮��Ʈ���� �����ۿ� �ش��ϴ� �ݹ��Լ��� �����ϵ��� ��
                ItemInfo_Dict[itemType].itemCallback +=
                    CallbackManager.Instance.CallBackList_Item(itemPlusInfo.itemCallbackIndex);
            }

        }
        
        //// ó���� �������� Ȯ��
        //foreach(eItemType serail in Enum.GetValues(typeof(eItemType)))
        //{
        //    if (serail == eItemType.None) continue;

        //    Debug.Log($"csv Item({ItemInfo_Dict[serail].name}) ����Ʈ ����");
        //    PrintProperties(ItemInfo_Dict[serail]);
        //}
    }

    private void ProcessCsvOfQuestInfo()
    {
        string path = Path.Combine("CSV", "Quest", "Quest");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
        }
        LoadCsv<cQuestInfo>(path,
            (row, questInfo) =>
            {
                // ��������� �Ҵ���� ���� ���
                if (questInfo == null) return;

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
                            if (eQuestType.TryParse(field, out eQuestType enumField))
                            {
                                questInfo.type = enumField; // �⺻Ű
                                questInfo.callback_endConditionCheck = CallbackManager.Instance.CallBackList_Quest((int)enumField); // üũ
                            }
                            else
                            {
                                if (field != "")
                                {
                                    Debug.LogWarning($"[{field}]�� ����Ʈ �ø����ȣ�� �� �� ����");
                                }
                            }
                            break;

                        case 1: questInfo.name = field; break;


                        case 2:
                            if (int.TryParse(field, out intField))
                            {
                                questInfo.rewardCoin = intField;
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� ������ ĳ���� �Ұ�");
                            }
                            break;

                        case 3:
                            if (eItemType.TryParse(field, out eItemType itemType)) 
                            {
                                questInfo.rewardItem = itemType;
                            }
                            else
                            {
                                Debug.LogWarning($"{field}�� ������ ĳ���� �Ұ�");
                            }
                            break;

                        case 4:
                            if (int.TryParse(field, out intField)) 
                            {
                                if (intField == 0)
                                {
                                    questInfo.isRepeatable = false;
                                }
                                else if (intField == 1)
                                {
                                    questInfo.isRepeatable = true;
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

                        
                        default:
                            Debug.LogAssertion($"{field}�� �߸��� �׸� ��ġ��");
                            break;
                    }
                    field_num++;
                }

                // ������ ��ųʸ��� ����
                if (QuestInfo_Dict.ContainsKey(questInfo.type) == false)
                {
                    QuestInfo_Dict.Add(questInfo.type, questInfo);
                }
                else
                {
                    QuestInfo_Dict[questInfo.type] = questInfo;
                }

            }
            );
    }

    private void ProcessCsvOfQuestDescription()
    {
        string path = Path.Combine("CSV", "Quest", "QuestDescription");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
        }
        LoadCsv<cQuestInfo>(path,
            (row, None) =>
            {
                //// ��������� �Ҵ���� ���� ���
                //if (None == null) return;

                int field_num = 0;
                eQuestType questType = eQuestType.None;
                // �� ���� ó�� ����
                foreach (string field in row)
                {
                    switch (field_num)
                    {
                        case 0:
                            if (eQuestType.TryParse(field, out eQuestType type))
                            {
                                questType = type;
                            }
                            else
                            {
                                if (field != "")
                                {
                                    Debug.LogWarning($"[{field}]�� ����Ʈ �ø����ȣ�� �� �� ����");
                                }
                            }
                            break;

                        case 1:
                            QuestInfo_Dict[questType].description.Add(field);
                            break;

                        default:
                            Debug.LogAssertion($"{field}�� �߸��� �׸� ��ġ��");
                            break;
                    }
                    field_num++;
                }
            }
            );

        // ó���� �������� Ȯ��
        foreach (eQuestType type in Enum.GetValues(typeof(eQuestType)))
        {
            if (type == eQuestType.None) continue;

            //Debug.Log($"csv Item({QuestInfo_Dict[type].name}) ����Ʈ ����");
            PrintProperties(QuestInfo_Dict[type]);
        }
    }





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
                                    info.endCallback = CallbackManager.Instance.CallBackList_DefaultText(intField);
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
                                            info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_DefaultText(intField));
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
                    TextScriptInfoList_2dDict[fileEnum][ (eStage)intStage].Add(info);
                }
            }
            );
    }

    public void LoadCsv<T_Class>(string resourceName, Action<List<string>, T_Class> RowCallback) where T_Class : class, new()
        // where T : new() : ���׸� Ÿ�� T�� �Ű����� ���� �⺻ �����ڸ� ���� Ŭ�������� �Ѵٴ� ������ �ǹ�
    {
        List<List<string>> csvData = new List<List<string>>();
        csvData.Clear();

        string fixedPath = resourceName.Replace('\\', '/');

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
                T_Class info = new T_Class();

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



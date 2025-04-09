using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using PublicSet;
using System.IO;



public class CsvManager : Singleton<CsvManager>
{
    // 에디터
    [SerializeField] private ItemTable itemPlusInfoTable;

    // 스크립트
    // OnlyOneLives PlayerInfo 자료구조
    private Dictionary<eCharacterType,cCharacterInfo> CharacterInfo_Dict;

    // 게임 스테이지에 따른 TextList를 저장하기위한 자료구조
    // 접근법 : TextInfoDicts[eTextScriptFile.None][eStage.None]
    private Dictionary<eTextScriptFile , Dictionary<eStage, List<cTextScriptInfo>>> TextScriptInfoList_2dDict;

    // onlyOneLives의 게임 절차에 따른 TextList를 저장하기위한 자료구조
    private Dictionary<eOOLProgress, List<cTextScriptInfo>> TextScriptInfoList_OnlyOneLives_Dict;

    // 아이템 객체 자료구조
    private Dictionary<eItemType, cItemInfo> ItemInfo_Dict;

    // 퀘스트 객체 자료구조
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
        // 해당 스테이지에 출력할 텍스트가 있으면 가져오고
        if (TextScriptInfoList_2dDict[File][Stage].Count !=0)
        {
            return TextScriptInfoList_2dDict[File][Stage];
        }

        //그렇지 않으면 디폴트값(0)의 텍스트를 가져옴
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
        // OnlyOneLives 플레이어 정보 관련
        CharacterInfo_Dict = new Dictionary<eCharacterType, cCharacterInfo>();

        // text 관련
        // 전체 딕트 메모리 확보
        TextScriptInfoList_2dDict = new Dictionary<eTextScriptFile, Dictionary<eStage, List<cTextScriptInfo>>>();
        foreach(eTextScriptFile file in Enum.GetValues(typeof(eTextScriptFile)))
        {
            // 파일키에 저장될 메모리 확보
            TextScriptInfoList_2dDict.Add(file, new Dictionary<eStage, List<cTextScriptInfo>>());

            foreach(eStage stage in Enum.GetValues(typeof(eStage)))
            {
                // 스테이지키에 저장될 메모리 확보
                TextScriptInfoList_2dDict[file].Add(stage, new List<cTextScriptInfo>());
            }
        }

        TextScriptInfoList_OnlyOneLives_Dict = new Dictionary<eOOLProgress, List<cTextScriptInfo>>();
        foreach (eOOLProgress progress in Enum.GetValues(typeof(eOOLProgress)))
        {
            // 프로세스 키에 저장될 메모리 확보
            TextScriptInfoList_OnlyOneLives_Dict.Add(progress, new List<cTextScriptInfo>());
        }


        // 아이템 관련
        ItemInfo_Dict = new Dictionary<eItemType, cItemInfo>();

        // 퀘스트 관련
        QuestInfo_Dict = new Dictionary<eQuestType, cQuestInfo>();
    }
    

    //public void NewCsvFile<T>(int FileCount, int StageCount, List<T>[][] CsvFileInfoPerStage)
    //{
    //    for (int i = 0; i < FileCount; i++)
    //    {
    //        // 각 파일들에 스테이지관리를 위한 저장공간
    //        CsvFileInfoPerStage[i] = new List<T>[StageCount];

    //        for (int j = 0; j < StageCount; j++)
    //        {
    //            // 각 스테이지마다 갖는 데이터 공간
    //            CsvFileInfoPerStage[i][j] = new List<T>(); // 리스트 초기화
    //        }
    //    }
    //}

    /// <summary>
    /// callbackManager를 써야하니 start에서 시작해야함
    /// </summary>
    public void TotalCsvProCess()
    {
        ProcessCsvOfCharacterInfo(); // 캐릭터 정보가 로딩된 후에 TextCsv처리가 가능함
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


        // 데이터 처리
        LoadCsv<cCharacterInfo>(
            path,
            (row, CharacterInfo) =>
            {
                // 저장공간을 할당받지 못한 경우
                if (CharacterInfo == null) return;

                int intField = 0;

                int field_num = 0;
                // 각 행의 처리 시작
                foreach (string field in row)
                {
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            if (int.TryParse(field, out intField))
                            {
                                if (Enum.IsDefined(typeof(eCharacterType), intField))
                                {
                                    CharacterInfo.CharaterIndex = (eCharacterType)intField; break;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{intField}는 eCharacter에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogAssertion($"{field}는 정수가 아님");
                            }
                            break;

                        case 1: CharacterInfo.CharacterName = field; break;
                        case 2: CharacterInfo.CharacterAge = field; break;
                        case 3: CharacterInfo.CharacterClan = field; break;
                        case 4: CharacterInfo.CharacterFeature = field; break;
                    }
                    field_num++;
                }

                // Character 딕셔너리에 삽입
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

        // 처리된 데이터의 확인
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

            // 데이터 처리
            LoadTextCsv(path, eFileName);

            // 처리된 데이터의 확인
            foreach (eStage eStageEnum in Enum.GetValues(typeof(eStage)))
            {
                // 각 파일에서 행을 하나씩 뽑아서 데이터를 올바르게 처리했는지 확인
                foreach (cTextScriptInfo info in TextScriptInfoList_2dDict[eFileName][eStageEnum])
                {
                    //Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) 프린트 생략");
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
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:

                            if (int.TryParse(field, out intField))
                            {
                                if (Enum.IsDefined(typeof(eOOLProgress), intField))
                                {
                                    intProgress = intField;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{path}의 \"{field}\"는 eOnlyOneLivesProgress에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                if (field != "")
                                {
                                    Debug.LogWarning($"[{field}]는 정수값이 아님");
                                }
                            }
                            break;

                        // 캐릭터 인덱스 처리
                        case 1:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eCharacterType), intField)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.characterEnum = (eCharacterType)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"[{intField}]는 {typeof(eCharacterType).Name}에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                            }
                            break;

                        // 캐릭터의 다이얼로그아이콘 인덱스 처리
                        case 2:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                info.DialogueIconIndex = intField;
                            }
                            else
                            {
                                Debug.LogWarning($"[{field}]는 정수값이 아닙니다.");
                            }
                            break;
                        // 스크립트 처리
                        case 3: info.script = field; break;

                        // 엔드콜백 처리
                        case 4:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eHasEndCallback), intField)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.hasEndCallback = (eHasEndCallback)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}는 {typeof(eHasEndCallback).Name}에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                            }
                            break;
                        // 엔드콜백이 존재하는 경우만(기본값 NO)
                        case 5:
                            if (info.hasEndCallback == eHasEndCallback.yes)
                            {
                                if (int.TryParse(field, out intField))
                                {
                                    info.endCallback = CallbackManager.Instance.CallbackList_OnlyOneLivesText(intField);
                                }
                                else
                                {
                                    Debug.LogAssertion($"[{field}]는 정수값이 아님");
                                }
                            }
                            break;

                        //// 선택지 처리
                        //case 6:
                        //    if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                        //    {
                        //        if (Enum.IsDefined(typeof(eHasSelection), intField)) // 정수값이 enum에 정의되었는지 확인
                        //        {
                        //            info.hasSelection = (eHasSelection)intField;
                        //        }
                        //        else
                        //        {
                        //            Debug.LogWarning($"{intField}는 {typeof(eHasSelection).Name}에 정의되지 않았음");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                        //    }
                        //    break;
                        //// 선택지가 존재하는 경우에만(기본값 NO)
                        //default:
                        //    if (info.hasSelection == eHasSelection.yes)
                        //    {
                        //        // 비어있는경우 제외
                        //        if (field.Length > 0)
                        //        {
                        //            if ((field_num % 2) == 1)
                        //            {

                        //                // 선택지 스크립트
                        //                info.selectionScript.Add(field);
                        //            }
                        //            else
                        //            {
                        //                // 선택지에 따른 처리
                        //                if (int.TryParse(field, out intField))
                        //                {
                        //                    info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_DefaultText(intField));
                        //                }
                        //                else
                        //                {
                        //                    Debug.LogAssertion($"[{field}]는 정수값이 아님");
                        //                }
                        //            }
                        //        }

                        //    }
                        //    break;
                    }

                    field_num++;
                }

                // 정의되지 않은 Progress의 경우 무시
                if (Enum.IsDefined(typeof(eOOLProgress), intProgress)) // Progress는 새로 할당받지 않았으면 int.MinValue가 저장되어있음
                {
                    // 각 행의 원소들을 처리한 후 스테이지별 csv파일에 삽입
                    TextScriptInfoList_OnlyOneLives_Dict[(eOOLProgress)intProgress].Add(info);
                }
            }
            );

        foreach (eOOLProgress progress in Enum.GetValues(typeof(eOOLProgress)))
        {
            // 각 절차에서 행을 하나씩 뽑아서 데이터를 올바르게 처리했는지 확인
            foreach (cTextScriptInfo info in TextScriptInfoList_OnlyOneLives_Dict[progress])
            {
                Debug.Log($"csv TextScript({(info as cTextScriptInfo).script}) 프린트 생략");
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
                // 저장공간을 할당받지 못한 경우
                if (itemInfo == null) return;

                int field_num = 0;

                // 각 행의 처리 시작
                foreach (string field in row)
                {
                    int intField = 0;
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            if (eItemType.TryParse(field, out eItemType enumField))
                            {
                                itemInfo.type = enumField;
                            }
                            else
                            {
                                if(field != "")
                                {
                                    Debug.LogWarning($"[{field}]는 아이템 시리얼번호가 될 수 없음");
                                }
                            }
                            break;

                        case 1: itemInfo.name = field; break;

                        case 2:
                            {
                                //'_'를 줄바꿈으로 변환
                                string[] scripts = field.Split('_');
                                itemInfo.description = string.Join('\n', scripts);
                            } break;

                        case 3:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
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
                                    Debug.LogWarning($"{intField}는 정의되지 않은 데이터");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
                            }
                            break;

                        // 소모성 여부
                        case 4:
                            if(itemInfo.isAvailable)
                            {
                                if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
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
                                        Debug.LogWarning($"{intField}는 정의되지 않은 데이터");
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
                                }
                            }
                            break;

                            // 사용시 데이터
                        case 5:
                            if(itemInfo.isAvailable)
                            {
                                if (float.TryParse(field, out float floatField))
                                {
                                    itemInfo.value_Use = floatField;
                                }
                                else
                                {
                                    Debug.LogWarning($"[{field}]는 사용값이 될 수 없음");
                                }
                            }
                            break;

                            // 판매가능 여부
                        case 6:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
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
                                    Debug.LogWarning($"{intField}는 정의되지 않은 데이터");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
                            }
                            break;

                            // 판매시 가격
                        case 7:
                            if(itemInfo.isForSale)
                            {
                                if (int.TryParse(field, out int intField2))
                                {
                                    itemInfo.value_Sale = intField2;
                                }
                                else
                                {
                                    Debug.LogWarning($"[{field}]는 사용값이 될 수 없음");
                                }
                            }
                            
                            break;

                        default: Debug.LogAssertion($"{field}는 잘못된 항목에 위치함");
                            break;
                    }
                    field_num++;
                }

                // 아이템 딕셔너리에 삽입
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
        
        // 추가적인 데이터 처리
        foreach(ItemPlusInfo itemPlusInfo in itemPlusInfoTable.item_PlusInfoList)
        {
            eItemType itemType = itemPlusInfo.type;

            //// 아이템에 해당하는 프리팹을 연결
            //if(ItemInfo_Dict.ContainsKey(itemType))
            //{
            //    ItemInfo_Dict[itemType].itemPrefab = itemPlusInfo.itemPrefab;
            //}
            //else
            //{
            //    Debug.LogAssertion($"serailNumber{itemType}는 딕셔너리 키에 없습니다.");
            //}

            // 사용 가능한 경우
            if (ItemInfo_Dict[itemType].isAvailable)
            {

                // 콜백리스트에서 아이템에 해당하는 콜백함수를 저장하도록 함
                ItemInfo_Dict[itemType].itemCallback +=
                    CallbackManager.Instance.CallBackList_Item(itemPlusInfo.itemCallbackIndex);
            }

        }
        
        //// 처리된 데이터의 확인
        //foreach(eItemType serail in Enum.GetValues(typeof(eItemType)))
        //{
        //    if (serail == eItemType.None) continue;

        //    Debug.Log($"csv Item({ItemInfo_Dict[serail].name}) 프린트 생략");
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
                // 저장공간을 할당받지 못한 경우
                if (questInfo == null) return;

                int field_num = 0;

                // 각 행의 처리 시작
                foreach (string field in row)
                {
                    int intField = 0;
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            if (eQuestType.TryParse(field, out eQuestType enumField))
                            {
                                questInfo.type = enumField; // 기본키
                                questInfo.callback_endConditionCheck = CallbackManager.Instance.CallBackList_Quest((int)enumField); // 체크
                            }
                            else
                            {
                                if (field != "")
                                {
                                    Debug.LogWarning($"[{field}]는 퀘스트 시리얼번호가 될 수 없음");
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
                                Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
                            }
                            break;

                        case 3:
                            if (eItemType.TryParse(field, out eItemType itemType)) 
                            {
                                questInfo.rewardItem = itemType;
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
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
                                    Debug.LogWarning($"{intField}는 정의되지 않은 데이터");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수로 캐스팅 불가");
                            }
                            break;

                        
                        default:
                            Debug.LogAssertion($"{field}는 잘못된 항목에 위치함");
                            break;
                    }
                    field_num++;
                }

                // 아이템 딕셔너리에 삽입
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
                //// 저장공간을 할당받지 못한 경우
                //if (None == null) return;

                int field_num = 0;
                eQuestType questType = eQuestType.None;
                // 각 행의 처리 시작
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
                                    Debug.LogWarning($"[{field}]는 퀘스트 시리얼번호가 될 수 없음");
                                }
                            }
                            break;

                        case 1:
                            QuestInfo_Dict[questType].description.Add(field);
                            break;

                        default:
                            Debug.LogAssertion($"{field}는 잘못된 항목에 위치함");
                            break;
                    }
                    field_num++;
                }
            }
            );

        // 처리된 데이터의 확인
        foreach (eQuestType type in Enum.GetValues(typeof(eQuestType)))
        {
            if (type == eQuestType.None) continue;

            //Debug.Log($"csv Item({QuestInfo_Dict[type].name}) 프린트 생략");
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
                    //Debug.Log($"{field_num}열 : " + field);
                    switch (field_num)
                    {
                        // 처리된 데이터를 넣을 Stage를 저장
                        case 0:
                            
                            if (int.TryParse(field, out intField))
                            {
                                if(Enum.IsDefined(typeof(eStage), intField))
                                {
                                    intStage = intField;
                                }
                                else
                                {
                                    Debug.LogAssertion($"{path}의 \"{field}\"는 eStage에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                if(field != "")
                                {
                                    Debug.LogWarning($"파일{fileEnum.ToString()}의 [{field}]는 스테이지 코드값이 숫자가 아님");
                                }
                            }
                            break;

                        // 캐릭터 인덱스 처리
                        case 1:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eCharacterType), intField)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.characterEnum = (eCharacterType)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"파일{fileEnum.ToString()}의 [{intField}]는 {typeof(eCharacterType).Name}에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                            }
                            break;

                        // 캐릭터의 다이얼로그아이콘 인덱스 처리
                        case 2:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                info.DialogueIconIndex = intField;
                            }
                            else
                            {
                                Debug.LogWarning($"파일{fileEnum.ToString()}의 {field}는 정수값이 아닙니다.");
                            }
                            break;
                        // 스크립트 처리
                        case 3: info.script = field; break;

                        // 엔드콜백 처리
                        case 4:
                            if (int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eHasEndCallback), intField)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.hasEndCallback = (eHasEndCallback)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}는 {typeof(eHasEndCallback).Name}에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                            }
                            break;
                        // 엔드콜백이 존재하는 경우만(기본값 NO)
                        case 5:
                            if(info.hasEndCallback == eHasEndCallback.yes)
                            {
                                if (int.TryParse(field, out intField))
                                {
                                    info.endCallback = CallbackManager.Instance.CallBackList_DefaultText(intField);
                                }
                                else
                                {
                                    Debug.LogAssertion($"파일{fileEnum.ToString()}의 {field}는 정수로 파스할 수 없음");
                                }
                            }
                            break;

                        // 선택지 처리
                        case 6: 
                            if(int.TryParse(field, out intField)) // 문자열을 정수형으로 캐스팅
                            {
                                if (Enum.IsDefined(typeof(eHasSelection), intField)) // 정수값이 enum에 정의되었는지 확인
                                {
                                    info.hasSelection = (eHasSelection)intField;
                                }
                                else
                                {
                                    Debug.LogWarning($"{intField}는 {typeof(eHasSelection).Name}에 정의되지 않았음");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"{field}는 정수값이 아닙니다.");
                            }
                            break;
                        // 선택지가 존재하는 경우에만(기본값 NO)
                        default:
                            if (info.hasSelection == eHasSelection.yes)
                            {
                                // 비어있는경우 제외
                                if (field.Length > 0)
                                {
                                    if ((field_num % 2) == 1)
                                    {

                                        // 선택지 스크립트
                                        info.selectionScript.Add(field);
                                    }
                                    else
                                    {
                                        // 선택지에 따른 처리
                                        if (int.TryParse(field, out intField))
                                        {
                                            info.SelectionCallback.Add(CallbackManager.Instance.CallBackList_DefaultText(intField));
                                        }
                                        else
                                        {
                                            Debug.LogAssertion($"파일{fileEnum.ToString()}의 {field}는 정수로 파스할 수 없음");
                                        }
                                    }
                                }
                                
                            }
                            break;
                    }
                    
                    field_num++;
                }

                // 정의되지 않은 스테이지의 경우 무시
                if(Enum.IsDefined(typeof(eStage), intStage)) // intStage는 새로 할당받지 않았으면 int.MinValue가 저장되어있음
                {
                    // 각 행의 원소들을 처리한 후 스테이지별 csv파일에 삽입
                    TextScriptInfoList_2dDict[fileEnum][ (eStage)intStage].Add(info);
                }
            }
            );
    }

    public void LoadCsv<T_Class>(string resourceName, Action<List<string>, T_Class> RowCallback) where T_Class : class, new()
        // where T : new() : 제네릭 타입 T가 매개변수 없는 기본 생성자를 가진 클래스여야 한다는 조건을 의미
    {
        List<List<string>> csvData = new List<List<string>>();
        csvData.Clear();

        string fixedPath = resourceName.Replace('\\', '/');

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
                T_Class info = new T_Class();

                RowCallback(row, info); // 전달된 델리게이트 실행
                row_num++;
            }
        }
        else
        {
            Debug.LogAssertion($"{resourceName} 파일이 존재하지 않습니다.");
        }
    }

    // 프로퍼티가 설정된 데이터멤버만 출력됨
    public void PrintProperties(object obj)
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



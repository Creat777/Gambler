using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// 완료되었던 퀘스트를 포함한 모든 퀘스트
    /// </summary>
    static public HashSet<sQuest> questHashSet { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        questHashSet = new HashSet<sQuest>();
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId에서 반환되는 id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (questHashSet.Count == 0)
        {
            Debug.Log("저장된 데이터가 없음");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach (sQuest quest in questHashSet)
        {
            if (quest.id > maxItemNumber)
            {
                maxItemNumber = quest.id;
            }
        }
        return maxItemNumber;
    }

    // 새로운 아이템 정보를 저장하기 위한 함수
    public void PlayerGetQuest(eQuestType questType)
    {
        int questId = GetNewLastId();

        sQuest newItem = new sQuest(questId, questType);

        if (questHashSet.Contains(newItem))
        {
            // 이미 존재하면 추가하지 않음
            Debug.LogWarning($"Item {questId} 는 이미 존재하는 아이템 항목.");
            return;
        }
        // 존재하지 않는다면 아이템 목록에 추가
        questHashSet.Add(newItem);
        Debug.Log($"Item {questId} 획득 성공.");

        // 아이템 목록에 변화가 생겼으니 팝업 리프레시
        InventoryPopUp.Instance.RefreshPopUp();
        //GameManager.connector_InGame.popUpView_Script.inventoryPopUp.RefreshPopUp();
    }

    public void PlayerCompleteQuest(eQuestType questType)
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);

        Debug.Log("애니메이션 필요");
        questInfo.isComplete = true;
    }
}

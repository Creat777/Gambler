using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// �Ϸ�Ǿ��� ����Ʈ�� ������ ��� ����Ʈ
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
        Debug.Log($"GetNewLastId���� ��ȯ�Ǵ� id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (questHashSet.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
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

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetQuest(eQuestType questType)
    {
        int questId = GetNewLastId();

        sQuest newItem = new sQuest(questId, questType);

        if (questHashSet.Contains(newItem))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Item {questId} �� �̹� �����ϴ� ������ �׸�.");
            return;
        }
        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        questHashSet.Add(newItem);
        Debug.Log($"Item {questId} ȹ�� ����.");

        // ������ ��Ͽ� ��ȭ�� �������� �˾� ��������
        InventoryPopUp.Instance.RefreshPopUp();
        //GameManager.connector_InGame.popUpView_Script.inventoryPopUp.RefreshPopUp();
    }

    public void CheckAllQuest()
    {
        foreach(sQuest quest in questHashSet)
        {
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

            // �Ϸ����� �����͸� üũ
            if (questInfo.isComplete) continue;

            questInfo.callback_endConditionCheck();
        }
    }

    public void PlayerCompleteQuest(eQuestType questType)
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isComplete = true;

        // ������ ���� ���� ���� �ٷ� �Ϸ���°� �ǵ��� ����
        if (questInfo.rewardCoin == 0 && questInfo.rewardItemType == eItemType.None)
            questInfo.hasReceivedReward = true;

        EventManager.Instance.SetEventMessage($"����Ʈ\n{questInfo.name}\n����!");
        EventManager.Instance.PlaySequnce_EventAnimation();
    }

}

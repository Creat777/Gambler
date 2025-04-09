using UnityEngine;
using UnityEngine.UI;
using PublicSet;

public class QuestDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;
    [SerializeField] private Text questReward;

    bool NeedLineFeed;
    bool coinDone;
    bool itemDone;

    public void SetPanel(cQuestInfo questInfo)
    {
        // 판넬의 이름 변경
        questName.text = questInfo.name;

        // 설명리스트에서 한 문장씩 가져오고 문장 사이에 개행문자 삽입
        questDescription.text = string.Empty;
        foreach (string description in questInfo.descriptionList)
        {
            questDescription.text += $"{description}\n";
        }


        // 퀘스트 보상 정리
        NeedLineFeed = false;
        coinDone = false;
        itemDone = false;
        questReward.text = string.Empty;
        if (questInfo.rewardCoin != 0)
        {
            questReward.text += $"{questInfo.rewardCoin.ToString()}";
            coinDone = true;
            NeedLineFeed = true;
        }

        if(questInfo.rewardItemType != eItemType.None)
        {
            if(NeedLineFeed)
            {
                questReward.text += "\n"; // 두 문장 이상일때만 개행문자 삽입
            }
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(questInfo.rewardItemType);
            questReward.text += itemInfo.name;
            itemDone = true;
        }

        // 보상이 없는 경우
        if (!(coinDone || itemDone))
        {
            questReward.text = "무언가를 간접적으로 얻을지도?";
        }
        
    }
}

using UnityEngine;
using UnityEngine.UI;
using PublicSet;

public class QuestDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;
    [SerializeField] private Text questReward;
    [SerializeField] private RewardButton rewardButton;

    bool NeedLineFeed;
    bool coinDone;
    bool itemDone;

    public void SetPanel(cQuestInfo questInfo)
    {
        // �ǳ��� �̸� ����
        questName.text = questInfo.name;

        // ������Ʈ���� �� ���徿 �������� ���� ���̿� ���๮�� ����
        questDescription.text = string.Empty;
        foreach (string description in questInfo.descriptionList)
        {
            questDescription.text += $"{description}\n";
        }


        // ����Ʈ ���� ����
        NeedLineFeed = false;
        coinDone = false;
        itemDone = false;
        questReward.text = string.Empty;

        if (questInfo.rewardCoin != 0)
        {
            questReward.text += $"���� {questInfo.rewardCoin.ToString()}��";
            coinDone = true;
            NeedLineFeed = true;
        }

        if(questInfo.rewardItemType != eItemType.None)
        {
            if(NeedLineFeed)
            {
                questReward.text += "\n"; // �� ���� �̻��϶��� ���๮�� ����
            }
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(questInfo.rewardItemType);
            questReward.text += $"������ {itemInfo.name}";
            itemDone = true;
        }

        // ������ 1���� �ִ� ���
        if (coinDone || itemDone)
        {
            rewardButton.BindQuestToButton(questInfo);
        }
        else // ������ ���� ���
        {
            questReward.text = "���𰡸� ���������� ��������?";
        }

        // ����Ʈ�� �Ϸ��ϰ� ������ �ȹ��� ���
        if (questInfo.isComplete && questInfo.hasReceivedReward == false)
        {
            rewardButton.TryActivate_Button();
        }
        else
        {
            rewardButton.TryDeactivate_Button();
        }
        
        
    }
}

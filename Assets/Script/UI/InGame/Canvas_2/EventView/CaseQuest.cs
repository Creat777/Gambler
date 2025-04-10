using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class CaseQuest : MonoBehaviour
{
    public CheckPopUp checkPopUp;

    public Text questNameText;
    public GameObject rewardCoin;
    public GameObject rewardItem;

    public Text coinNum;
    public Image itemImage;
    public Text itemName;
    


    public void SetPanel(cQuestInfo quest)
    {
        checkPopUp.PopUpUpChange(checkCase.QuestComplete);

        // �гο� ���� ����Ʈ���� ����
        questNameText.text = quest.name;

        // ���� ������ ������� Ȱ��ȭ
        if (quest.rewardCoin == 0) rewardCoin.SetActive(false);
        else
        {
            rewardCoin.SetActive(true);
            coinNum.text = $"x{quest.rewardCoin.ToString()}";
            PlayManager.Instance.AddPlayerMoney(quest.rewardCoin);
        }
        
        // ������ ������ ������� Ȱ��ȭ
        if(quest.rewardItemType == eItemType.None) rewardItem.SetActive(false);
        else
        {
            rewardItem.SetActive(true);
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(quest.rewardItemType);
            itemName.text = $"{itemInfo.name}";
            ItemManager.Instance.PlayerGetItem(quest.rewardItemType);
        }

    }
}

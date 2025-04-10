using PublicSet;
using UnityEngine;

public class RewardButton : Deactivatable_ButtonBase
{
    public cQuestInfo questInfo { get; private set; }


    private void Start()
    {
        SetButtonCallback(PlayerGetReward);
    }
    
    public void BindQuestToButton(cQuestInfo questInfo)
    {
        this.questInfo = questInfo;
    }

    public void PlayerGetReward()
    {
        if(questInfo.rewardCoin >0)
        {
            PlayManager.Instance.AddPlayerMoney(questInfo.rewardCoin);
        }
        if(questInfo.rewardItemType != eItemType.None)
        {
            ItemManager.Instance.PlayerGetItem(questInfo.rewardItemType);
        }

        // ������ 1����
        TryDeactivate_Button();
        questInfo.hasReceivedReward = true;

        // ������� �˾��� ��������
        GameManager.connector_InGame.popUpView_Script.questListPopUp.RefreshPopUp();
    }
}

using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class QuestElementPanel : ButtonBase
{
    public sQuest quest;
    public cQuestInfo questInfo {  get; private set; }
    public Text TextOfButton;
    public Text TextOfStatus;

    public void InitQuestdata(int id, eQuestType type , cQuestInfo cQuest)
    {
        quest = new sQuest(id, type);
        questInfo = cQuest;
    }
    public void SetQuestdata(sQuest quest, cQuestInfo cQuest)
    {
        this.quest = new sQuest(quest);
        questInfo = cQuest;
    }

    public void InitQuestPanel()
    {
        TextOfButton.text = questInfo.name;
        if (questInfo.isComplete)
        {
            TextOfStatus.color = Color.blue;
            TextOfStatus.text = "완료";
        }
        else
        {
            TextOfStatus.color = Color.red; 
            TextOfStatus.text = "진행중";
        }
    }
}

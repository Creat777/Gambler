using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestListPopUp : PopUpBase<QuestListPopUp>
{

    [SerializeField] private QuestContentPopUp _questContentPopUp;
    public QuestContentPopUp questContentPopUp
    {
        get
        {
            if(_questContentPopUp == null)
            {
                Debug.LogAssertion("QuestListPopUp에 QuestContentPopUp가 연결되지 않았음");
            }
            return _questContentPopUp;
        }
    }

    HashSet<sQuest> playerQuestHash
    {
        get { return QuestManager.questHashSet; }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePool(10);
        questContentPopUp.InitPopUp();
    }

    private void OnEnable()
    {
        RefreshPopUp();
    }

    public override void RefreshPopUp()
    {
        Debug.Log($"playerQuestHash.Count == {playerQuestHash.Count}");
        RefreshPopUp(playerQuestHash.Count,
            () =>
            {
                foreach (sQuest quest in playerQuestHash)
                {
                    // 아이템정보로 초기화될 객체
                    QuestElementPanel questPanel = ActiveObjList[quest.id].GetComponent<QuestElementPanel>(); ;

                    // 아이템 종합정보를 호출
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

                    // 활성화된 각 객체에 정보를 초기화
                    if (questInfo != null)
                    {
                        questPanel.SetQuestdata(quest, questInfo);
                        questPanel.InitPanel();

                        // 퀘스트 항목을 클릭시 호출
                        questPanel.SetButtonCallback(
                            () =>
                            {
                                questContentPopUp.descriptionPanel.SetPanel(questInfo);
                                GameManager.connector_InGame.popUpView_Script.QuestContentPopUpOpen();
                            });
                    }
                    else
                    {
                        Debug.LogAssertion($"{questPanel.gameObject.name}은 QuestElementPanel == null");
                    }
                }
            });
    }
}

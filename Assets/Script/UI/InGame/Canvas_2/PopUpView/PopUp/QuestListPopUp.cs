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
                Debug.LogAssertion("QuestListPopUp�� QuestContentPopUp�� ������� �ʾ���");
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
                    // ������������ �ʱ�ȭ�� ��ü
                    QuestElementPanel questPanel = ActiveObjList[quest.id].GetComponent<QuestElementPanel>(); ;

                    // ������ ���������� ȣ��
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    if (questInfo != null)
                    {
                        questPanel.SetQuestdata(quest, questInfo);
                        questPanel.InitPanel();

                        // ����Ʈ �׸��� Ŭ���� ȣ��
                        questPanel.SetButtonCallback(
                            () =>
                            {
                                questContentPopUp.descriptionPanel.SetPanel(questInfo);
                                GameManager.connector_InGame.popUpView_Script.QuestContentPopUpOpen();
                            });
                    }
                    else
                    {
                        Debug.LogAssertion($"{questPanel.gameObject.name}�� QuestElementPanel == null");
                    }
                }
            });
    }
}

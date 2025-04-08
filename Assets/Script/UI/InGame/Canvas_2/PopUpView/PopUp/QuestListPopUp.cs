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
                GameObject[] quests =  GameObject.FindGameObjectsWithTag("Quest");
                if (quests.Length != 2) Debug.LogAssertion("�±װ� �������� �ʾ���");
                foreach(GameObject quest in quests)
                {
                    if (quest == gameObject) continue;
                    else
                    {
                        _questContentPopUp = quest.GetComponent<QuestContentPopUp>();
                    }
                    if(_questContentPopUp == null)
                    {
                        Debug.LogAssertion("QuestContentPopUp�� ��ũ��Ʈ ������ �ȵ���");
                    }
                }
            }
            return _questContentPopUp;
        }
    }

    public override void RefreshPopUp()
    {
        Debug.LogWarning("�����ǵ��� �ʾ���");
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePool(1);
    }
    private void Start()
    {
        GameObject obj = GetObject();
        Debug.Log("�ӽ÷� �������");

        QuestElementPanel script = obj.GetComponent<QuestElementPanel>();
        if (script != null)
        {
            script.SetButtonCallback(
                (GameManager.connector as Connector_InGame).popUpView_Script.QuestContentPopUpOpen);
            ChangeContentRectTransform();
        }

    }
}

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
                if (quests.Length != 2) Debug.LogAssertion("태그가 부착되지 않았음");
                foreach(GameObject quest in quests)
                {
                    if (quest == gameObject) continue;
                    else
                    {
                        _questContentPopUp = quest.GetComponent<QuestContentPopUp>();
                    }
                    if(_questContentPopUp == null)
                    {
                        Debug.LogAssertion("QuestContentPopUp에 스크립트 연결이 안됐음");
                    }
                }
            }
            return _questContentPopUp;
        }
    }

    public override void RefreshPopUp()
    {
        Debug.LogWarning("재정의되지 않았음");
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePool(1);
    }
    private void Start()
    {
        GameObject obj = GetObject();
        Debug.Log("임시로 연결됐음");

        QuestElementPanel script = obj.GetComponent<QuestElementPanel>();
        if (script != null)
        {
            script.SetButtonCallback(
                (GameManager.connector as Connector_InGame).popUpView_Script.QuestContentPopUpOpen);
            ChangeContentRectTransform();
        }

    }
}

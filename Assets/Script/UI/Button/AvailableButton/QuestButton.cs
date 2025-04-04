using UnityEngine;

public class QuestButton : ButtonBase
{
    [SerializeField] private PopUpView_InGame _popUpView;
    public PopUpView_InGame popUpView
    {
        get
        {
            if (_popUpView == null)
            {
                Debug.LogWarning($"{gameObject.name}���� popUpView �翬�� �õ�");
                _popUpView = (GameManager.connector as Connector_InGame).popUpView_Script;
                if (_popUpView != null)
                {
                    Debug.LogWarning("�翬�� ����");
                }
                else
                {
                    Debug.LogWarning("�翬�� ����");
                }
            }
            return _popUpView;
        }
    }
    private void Start()
    {
        if (popUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(popUpView.QuestListPopUpOpen);
    }
}

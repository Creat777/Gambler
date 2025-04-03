using UnityEngine;

public class GameAssistantButton : ButtonBase
{
    [SerializeField] private PopUpView _popUpView;
    public PopUpView popUpView
    { get
        {
            if(_popUpView == null)
            {
                Debug.LogWarning($"{gameObject.name}에서 popUpView 재연결 시도");
                _popUpView = (GameManager.connector as Connector_InGame).popUpView_Script;
                if(_popUpView != null)
                {
                    Debug.LogWarning("재연결 성공");
                }
                else
                {
                    Debug.LogWarning("재연결 실패");
                }
            }
            return _popUpView;
        }
    }

    private void Start()
    {
        if (popUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(popUpView.GameAssistantPopUpOpen_OnlyOneLives);
    }


}

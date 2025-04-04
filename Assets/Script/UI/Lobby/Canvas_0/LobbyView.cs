using UnityEngine;
using UnityEngine.SceneManagement;
using PublicSet;

public class LobbyView : MonoBehaviour
{
    public PopUPView_Lobby popUpView;
    // ���ν����ϱ� ��ư
    public void StartNewGame()
    {
        // ��� �������� ����
        GameManager.Instance.SetPlayerSaveKey(ePlayerSaveKey.None);
        GameManager.Instance.SceneUnloadView(()=> SceneManager.LoadScene("InGame"));
    }

    // �̾��ϱ� ��ư -> ������ �˾� ����
    public void ContinuePopUpOpen()
    {
        popUpView.ContinuePopUpOpen();
    }

    // ������� -> ���� �÷��� ����� �������� �̴ϰ��� �˾� ����
    public void FreeModePopUpOpen()
    {

    }

    // �ɼ� -> �ɼ��˾� ����
    public void OptionPopUpOpen()
    {

    }

    // �ְ��� -> ��������� �����ݾ��� ������� �ݾ� 3���� ǥ��
    public void BestScorePopUpOpen()
    {

    }

    public void QuitGamePopUpOpen()
    {

    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyView : MonoBehaviour
{
    // ���ν����ϱ� ��ư
    public void StartNewGame()
    {
        // ��� �������� ����
        GameManager.Instance.SceneUnloadView(()=> SceneManager.LoadScene("InGame"));
    }

    // �̾��ϱ� ��ư -> ������ �˾� ����
    public void ContinuePopUpOpen()
    {

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

    // �������� ��ư
    public void QuitGame()
    {
        // ���� ����
        Application.Quit();

#if UNITY_EDITOR
        // �����Ϳ����� ���� ���� ��� ���� ��� ����
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

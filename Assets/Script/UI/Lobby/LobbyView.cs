using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyView : MonoBehaviour
{
    // ���ν����ϱ� ��ư
    public void StartNewGame()
    {
        // ��� �������� ����
        SceneManager.LoadScene("InGame");
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

        // �����Ϳ����� ���� ���� ��� ���� ��� ����
        UnityEditor.EditorApplication.isPlaying = false;
        
//#if UNITY_EDITOR

//#endif
    }
}

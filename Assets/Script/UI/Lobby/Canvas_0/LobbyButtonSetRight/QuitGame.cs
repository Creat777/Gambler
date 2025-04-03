using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // ���� ���� �Լ�
    public void ExitGame()
    {
        // �����Ϳ����� EditorApplication.isPlaying�� ����Ͽ� ������ ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ���� ���� ���� ���� �� (�ȵ���̵� ��) ���� ����
        Application.Quit();
#endif
    }
}

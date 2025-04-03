using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // 게임 종료 함수
    public void ExitGame()
    {
        // 에디터에서는 EditorApplication.isPlaying을 사용하여 게임을 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 게임 실행 중일 때 (안드로이드 등) 게임 종료
        Application.Quit();
#endif
    }
}

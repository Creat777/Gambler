using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyView : MonoBehaviour
{
    // 새로시작하기 버튼
    public void StartNewGame()
    {
        // 즉시 게임으로 입장
        GameManager.Instance.SceneUnloadView(()=> SceneManager.LoadScene("InGame"));
    }

    // 이어하기 버튼 -> 저장기록 팝업 오픈
    public void ContinuePopUpOpen()
    {

    }

    // 자유모드 -> 최종 플레이 기록을 바탕으로 미니게임 팝업 오픈
    public void FreeModePopUpOpen()
    {

    }

    // 옵션 -> 옵션팝업 오픈
    public void OptionPopUpOpen()
    {

    }

    // 최고기록 -> 게임종료시 최종금액중 가장높은 금액 3개를 표시
    public void BestScorePopUpOpen()
    {

    }

    public void QuitGamePopUpOpen()
    {

    }

    // 게임종료 버튼
    public void QuitGame()
    {
        // 게임 종료
        Application.Quit();

#if UNITY_EDITOR
        // 에디터에서는 게임 종료 대신 실행 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

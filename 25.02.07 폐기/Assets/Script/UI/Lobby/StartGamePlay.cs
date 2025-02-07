using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGamePlay : MonoBehaviour
{
    public void StartPlay()
    {
        // TODO -> stage선택 팝업 추가
        SceneManager.LoadScene("InGame");
    }

}

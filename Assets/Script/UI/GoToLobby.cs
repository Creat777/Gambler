using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLobby : MonoBehaviour
{
    public void MoveToLobby()
    {
        GameManager.Instance.SceneUnloadView(() => SceneManager.LoadScene("Lobby"));
    }
}

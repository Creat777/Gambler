using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{

    public void GameExit()
    {
        SceneManager.LoadScene("Lobby");
    }
}

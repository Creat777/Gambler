using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLobby : MonoBehaviour
{
    public void MoveToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}

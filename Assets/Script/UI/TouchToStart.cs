using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToStart : MonoBehaviour
{
    void Start()
    {
        
    }

    public void MoveToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    
    

    void Start()
    {
        
    }

    public void Button_GameStart()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_GameStart();
    }

    public void Button_CheckScore()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_CheckScore();
    }

    public void Button_Option()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_Option();
    }

    public void Button_LogOut()
    {
        ButtonClickAudio.Instance.PlayClip();
        SceneManager.Instance.Button_LoginScreen();
    }
}

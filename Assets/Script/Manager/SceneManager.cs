using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    // InitializerSet에서 연결
    //public GameObject canvas;

    private void MakeSingleTone()
    {
        if (Instance == null)
        { 
            Instance = this;
            //Debug.Log($"singleTone객체 : {gameObject.name}");
        }
        else
        {
            Destroy(gameObject);
            //Debug.LogError($"객체가 2개 이상이 되어 다음 객체를 파괴 : {gameObject.name}");
            return;
        }
        DontDestroyOnLoad(Instance);
    }
    private void Awake()
    {
        MakeSingleTone();
    }
    private void Start()
    {
        
    }

    
    
    public void Button_GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        BackGroundMusic.Instance.DefaultPlay();
    }

    public void Button_GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        BackGroundMusic.Instance.LetsPlay();
    }


    public void Button_CheckScore()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Score");
        BackGroundMusic.Instance.DefaultPlay();
    }

    public void Button_Option()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Option");
        BackGroundMusic.Instance.DefaultPlay();
    }

    public void Button_LoginScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        BackGroundMusic.Instance.DefaultPlay();
    }
}

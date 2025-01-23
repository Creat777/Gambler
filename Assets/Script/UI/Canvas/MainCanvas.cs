using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    // UI 오브젝트
    public GameObject UI_Box;
    public GameObject popUp;
    public GameObject menuButton;
    public GameObject optionPanel;
    public Image remainTime;

    private float maxTime;

    private void Awake()
    {
        //UI_Box = GameObject.Find("UI_Box");
        //popUp = GameObject.Find("PopUpPanel");
        //menuButton = GameObject.Find("MenuButton");
    }
    void Start()
    {
        if (UI_Box != null)
        {
            UI_Box.SetActive(false);
        }
        else
        {
            Debug.LogError("UI_Box == null");
        }

        if (popUp != null)
        {
            popUp.SetActive(true);
        }
        else
        {
            Debug.LogError("popUp == null");
        }

        if (menuButton != null) 
        { 
            menuButton.SetActive(false);
        }
        else
        {
            Debug.LogError("menuButton == null");
        }

        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("optionPanel == null");
        }

        maxTime = GameManager.Instance.MaxTime_;
    }

    public void Button_OptionWindowTerminate()
    {
        if (optionPanel != null)
        {
            ButtonClickAudio.Instance.PlayClip();
            GameManager.Instance.isGamePause = false;
            GameManager.Instance.gameSpeed = GameManager.Instance.defualtGameSpeed;
            UI_Box.SetActive(true);
            optionPanel.SetActive(false);
        }
    }

    public void Button_OptionWindowOpen()
    {
        if (optionPanel != null)
        {
            ButtonClickAudio.Instance.PlayClip();
            GameManager.Instance.isGamePause = true;
            GameManager.Instance.gameSpeed = 0;
            UI_Box.SetActive(false);
            optionPanel.SetActive(true);
        }
    }

    void Update()
    {
        remainTime.fillAmount = (maxTime - GameManager.Instance.gameTime) / maxTime;
    }
}

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LoginCanvas : MonoBehaviour
{
    // 판넬
    public GameObject LoginPanel;
    public GameObject ID_GeneratePanel;

    // 로그인 화면
    public Text StateOfLogin;
    public InputField inputID;
    public InputField inputPassword;

    // 계정생성 화면
    public Text StateOfGenerate;
    public InputField generateID;
    public InputField generatePassword;
    public InputField checkPassword;

    void Start()
    {
        BackToLoginPanel();
    }

    public void Button_BackToLoginPanel()
    {
        ButtonClickAudio.Instance.PlayClip();
        BackToLoginPanel();
    }

    private void BackToLoginPanel()
    {
        LoginPanel.SetActive(true);
        ID_GeneratePanel.SetActive(false);
        StateOfLogin.text = "게임에 접속하신 것을 환영합니다!";
        inputID.text = "";
        inputPassword.text = "";
    }

    public void Button_SwitchGeneratePanel()
    {
        ButtonClickAudio.Instance.PlayClip();
        LoginPanel.SetActive(false);
        ID_GeneratePanel.SetActive(true);
        StateOfGenerate.text = "원하는 ID와 비밀번호를 입력해주세요";
        generateID.text = "";
        generatePassword.text = "";
        checkPassword.text = "";
    }

    public void Button_Login()
    {
        ButtonClickAudio.Instance.PlayClip();
        string password = PlayerInfoManager.Instance.LoadData(inputID.text, "nullPassword");

        if (password == "nullPassword")
        {
            StateOfLogin.text = "존재하지 않는 ID입니다.";
        }
        else if (password == inputPassword.text)
        {
            // 로그인 성공
            GameManager.Instance.playerID = $"ID_{inputID.text}";
            SceneManager.Instance.Button_GoToMenu();
        }
        else
        {
            StateOfLogin.text = "로그인에 실패하였습니다. 비밀번호를 확인해주세요";
        }
    }

    public void Button_GenerateID()
    {
        ButtonClickAudio.Instance.PlayClip();
        bool checkId = PlayerInfoManager.Instance.LoadData(generateID.text, "nullPassword") == "nullPassword";
        if(checkId) // 기존 아이디가 없음
        {
            Debug.Log($"입력 ID : {generateID.text}");
            Debug.Log($"입력 PW : {generatePassword.text}");
            Debug.Log($"확인 PW : {checkPassword.text}");
            if (generatePassword.text == checkPassword.text)
            {
                PlayerInfoManager.Instance.SaveData(generateID.text, generatePassword.text);
                BackToLoginPanel();
            }
            else
            {
                StateOfGenerate.text = "비밀번호를 다시 확인해주세요";
            }
        }
        else
        {
            StateOfGenerate.text = "이미 존재하는 ID입니다.";
        }
    }

    public void Button_QuitGame()
    {
        // 게임 종료
        Application.Quit();

        // 에디터에서는 게임 종료 대신 실행 모드 종료
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

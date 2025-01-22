using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class TextWindowView : MonoBehaviour
{
    // 에디터에서 연결
    public Text textWindow;
    CsvInfo csvInfo;
    private string[] TextSet;

    // 스크립트 수정
    Interactive interactive;
    bool isInteractive;
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    string currentText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        interactive = new Interactive();
        isInteractive = false;
        isTypingReady = true;
        typingDelay = 0.05f;
    }

    private void OnEnable()
    {
        if (Player.Instance != null)
        {
            // 시작 텍스트
            TextIndex = 0;

            InteractiveProcess();

            // 상호작용을 시작할때 즉시 스크립트가 출력되도록 만듬
            if (isInteractive && TextSet.Length >= 1)
            {
                if (isTypingReady)
                {
                    PrintText();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.Instance!=null)
        {
            InteractiveProcess();

            // 스페이스바를 누르면 이어서 스크립트 처리를 시작함
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 상호작용했고 스크립트를 읽어왔으면
                if (isInteractive && TextSet.Length >= 1)
                {
                    if (isTypingReady)
                    {
                        PrintText();
                    }
                    else
                    {
                        // true로 바꿔서 typing을 끝냄
                        isTypingReady = true;
                    }

                }
            }
        }
        
    }


    // 상호작용을 위한 기본 처리
    private void InteractiveProcess()
    {
        // 플레이어의 레이캐스트에 걸리는 객체
        string objectName = Player.Instance.hitObjectName;

        // 플레이어가 현재 상호작용할 수 있는 객체가 있으면
        if (objectName != null)
        {
            Debug.Log($"현재 플레이어가 상호작용하는 객체 : {objectName}");
            // 상호작용하여 스크립트를 읽어오지 않았으면
            if (isInteractive == false)
            {
                // 스크립트를 읽어오고
                GetTextSet(objectName);

                // 스크립트 읽어왔음을 저장
                isInteractive = true;
                return;
            }
        }

        // 플레이어가 현재 상호작용할 수 있는 객체가 없으면
        else
        {
            // 새롭게 상호작용하여 스크립트를 읽어올 수 있도록 만듬
            isInteractive = false;
            return;
        }
    }

    public void GetTextSet(string objectName)
    {
        CsvInfo[] csvInfos = CsvManager.Instance.InteractiveCsvInfos;

        // 플레이어가 상호작용할 수 있는 객체인지 확인
        csvInfo = CsvProcessor.Instance.FindCsvInfo(objectName, csvInfos, interactive);

        // 객체가 있으면 재생할 스크립트를 반환
        if (csvInfo != null)
        {
            Debug.Log($"GetTextSet로 읽어온 스크립트파일 : {csvInfo.CsvFileName}");
            TextSet = CsvProcessor.Instance.GetText(csvInfo, interactive);
        }
    }

    private void PrintText()
    {
        if (TextIndex < TextSet.Length)
        {
            // 텍스트 전환
            currentText = TextSet[TextIndex++];

            // 텍스트 순차적으로 보이게함
            StartCoroutine(TypeDialogue(currentText));

            // 마지막 대화에서 다음 대화 이미지 삭제
            if (TextIndex == TextSet.Length)
                //arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        if(TextIndex >= TextSet.Length)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTypingReady = false;
        textWindow.text = "";
        foreach (char letter in dialogue)
        {
            if (isTypingReady == false) // 다시 버튼을 안눌렀으면 문자가 하나씩 나타남
            {
                // UI에 글자를 타이핑
                textWindow.text += letter;

                // 공백을 제외한 문자만 타이핑 딜레이를 적용
                if(letter != ' ')
                {
                    yield return new WaitForSeconds(typingDelay);
                }
                
            }
            else // 타이핑 도중 화면을 클릭하면 타이핑을 멈추고 전체 문장을 보여줌
            {
                textWindow.text = dialogue;
                break;
            }
        }
        isTypingReady = true;
    }

    // DOTO -> 코루틴으로 텍스트를 화면에 출력

}

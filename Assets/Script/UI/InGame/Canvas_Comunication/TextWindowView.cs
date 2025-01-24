using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Interactive_ID
//{
//    None = 0,
//    Interactive_Bed,
//    Interactive_Cabinet,
//    Interactive_Clock,
//    Interactive_Computer,
//    Interactive_Door
//}

public class TextWindowView : MonoBehaviour
{

    // 에디터에서 연결
    public Text textWindow;
    CsvInfo csvInfo;
    private List<string[]> TextSet;
    public RectTransform arrowImageTrans;
    public GameObject SelectionView;

    // 스크립트 수정
    Interactive interactive;
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    string currentText;
    string LastObjectName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        interactive = new Interactive();
        isTypingReady = true;
        typingDelay = 0.05f;
        LastObjectName = "";
    }

    private void OnEnable()
    {
        GameManager.Instance.Pause_theGame();

        // 대화를 시작할때마다 화살표 이미지 살림
        arrowImageTrans.gameObject.SetActive(true);

        // 화살표 이미지 애니메이션
        ArrowDoTween();


        if (Player.Instance != null)
        {
            // 시작 텍스트 인덱스
            TextIndex = 0;

            // 상호작용을 위한 기본처리
            InteractiveProcess();

            // 상호작용을 시작할때 즉시 스크립트가 출력되도록 만듬
            if (TextSet.Count >= 1)
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
                NextPrintButton();
            }
        }
    }

    public void NextPrintButton()
    {
        // 상호작용했고 스크립트를 읽어왔으면
        if (TextSet.Count >= 1)
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


    // 상호작용을 위한 기본 처리
    private void InteractiveProcess()
    {
        // 플레이어의 레이캐스트에 걸리는 객체
        string curruntObjectName = Player.Instance.hitObjectName;

        // 상호작용하여 스크립트를 읽어오지 않았으면
        if (LastObjectName != curruntObjectName)
        {
            // 스크립트를 읽어오고
            InitTextSet(curruntObjectName);

            // 마지막 상호작용한 객체의 이름을 저장
            LastObjectName = curruntObjectName;
            return;
        }
        else
        {
            //Debug.Log("이미 스크립트 목록을 읽어왔습니다");
        }
    }

    public void InitTextSet(string objectName)
    {
        TextSet = CsvManager.Instance.GetText(objectName, interactive);
    }

    public void PrintText()
    {
        // 배열 범위오류 제한
        if (TextIndex < TextSet.Count)
        {
            // 텍스트 전환
            currentText = TextSet[TextIndex++][1];

            // 텍스트 순차적으로 보이게함
            StartCoroutine(TypeDialogue(currentText));

            // 마지막 대화에서 다음 대화 이미지 삭제
            if (TextIndex == TextSet.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // 텍스트가 다 끝난경우
        if(TextIndex >= TextSet.Count)
        {
            GameManager.Instance.Continue_theGame();
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

    private void ArrowDoTween()
    {
        // doTween에서만 쓰일 변수
        Vector3 targetScale = Vector3.one * 1.2f; // 커지는 크기
        float duration = 0.5f; // 애니메이션 지속 시간

        // DOTween Sequence 생성
        Sequence sequence = DOTween.Sequence();

        // 현재 크기 저장
        Vector3 originalScale = arrowImageTrans.localScale;
        sequence.Append(arrowImageTrans.DOScale(targetScale, duration)) // 커지는 애니메이션
                .Append(arrowImageTrans.DOScale(originalScale, duration)) // 복귀 애니메이션
                .SetLoops(-1); // 무한 반복
    }

}

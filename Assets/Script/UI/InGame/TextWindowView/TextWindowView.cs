using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextWindowView : MonoBehaviour
{

    // 에디터에서 연결
    public Text textWindow;
    public Text Speaker;
    private List<string[]> TextSet;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // 스크립트 수정
    Interactive interactive;
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    string[] currentTextData;
    GameObject LastObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        interactive = new Interactive();
        isTypingReady = true;
        typingDelay = 0.05f;
    }

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Pause_theGame();
        }
        
        // 대화를 시작할때마다 화살표 이미지 살림
        arrowImageTrans.gameObject.SetActive(true);

        // 화살표 이미지 애니메이션
        ArrowDoTween();


        if (PlayerMoveAndAnime.Instance != null)
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

    private void OnDisable()
    {
        // 셀렉션뷰를 끔으로써 셀렉션뷰가 초기화되도록만듬
        selectionView.gameObject.SetActive(false);

        if(GameManager.Instance != null)
        {
            GameManager.Instance.Continue_theGame();
        }
    }

    //void Update()
    //{
    //    if (PlayerMoveAndAnime.Instance != null)
    //    {
    //        InteractiveProcess();

    //        // 스페이스바를 누르면 이어서 스크립트 처리를 시작함
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            NextPrintButton();
    //        }
    //    }
    //}

    public void NextPrintButton()
    {
        // index 0 : 말하는 사람
        // index 1 : 스크립트
        // index 2 : selection 유무(0, 1)
        // index 3 : 1번째 선택지
        // index 4 : 2번째 선택지
        // index 5 : 1번 콜백번호
        // index 6 : 2번 콜백번호

        // 상호작용했고 스크립트를 읽어왔으면
        if (TextSet.Count >= 1 )
        {
            // selection이 없으면
            if (currentTextData[2] == "0")
            {
                // 타이핑이 끝난경우 다음 타이핑을 시작
                if (isTypingReady)
                {
                    PrintText();
                }
                // 타이핑이 아직 안끝난 경우 타이핑을 끝냄
                else
                {
                    // true로 바꿔서 typing을 끝냄
                    isTypingReady = true;
                }
            }

            // selection이 있으면
            else if (currentTextData[2] == "1")
            {
                // 타이핑이 끝난 경우 selectionView를 활성화
                if(isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView SV = selectionView.GetComponent<SelectionView>();

                    // currentTextData의 인덱스
                    // index 5 : 1번 콜백번호
                    // index 6 : 2번 콜백번호
                    int index_1; int.TryParse(currentTextData[5], out index_1);
                    int index_2; int.TryParse(currentTextData[6], out index_2);

                    SV.RegisterButtonClick_Selection1(CallBackManager.Instance.CallBackList(index_1, LastObject ));
                    SV.RegisterButtonClick_Selection2(CallBackManager.Instance.CallBackList(index_2, LastObject ));
                }
                // 타이핑이 안끝났으면 타이핑을 끝내기
                else
                {
                    isTypingReady = true;
                }
            }

        }
    }


    // 상호작용을 위한 기본 처리
    private void InteractiveProcess()
    {
        // 플레이어의 레이캐스트에 걸리는 객체
        GameObject curruntObject = PlayerMoveAndAnime.Instance.hitObject;

        // 스크립트를 읽어오고
        InitTextSet(curruntObject.name);

        // 마지막 상호작용한 객체를 저장
        LastObject = curruntObject;
        return;
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
            // 텍스트 데이터 전환
            // index 0 : 말하는 사람
            // index 1 : 스크립트
            // index 2 : selection 유무(0, 1)
            // index 3 : 1번째 선택지
            // index 4 : 2번째 선택지
            // index 5 : 1번 콜백번호
            // index 6 : 2번 콜백번호
            currentTextData = TextSet[TextIndex++];

            // 텍스트 순차적으로 보이게함
            Speaker.text = currentTextData[0];
            StartCoroutine(TypeDialogue(currentTextData[1]));

            // 마지막 대화에서 다음 대화 이미지(화살표) 삭제
            if (TextIndex == TextSet.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // 텍스트가 다 끝난경우 // currentTextData[2] == "0"은 PrintText 진입전에 검사했음
        if (TextIndex >= TextSet.Count)
        {
            CallBackManager.Instance.TextWindowPopUp_Close();
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
        Vector3 originalScale = Vector3.one;
        sequence.Append(arrowImageTrans.DOScale(targetScale, duration)) // 커지는 애니메이션
                .Append(arrowImageTrans.DOScale(originalScale, duration)) // 복귀 애니메이션
                .SetLoops(-1); // 무한 반복
        
    }

}

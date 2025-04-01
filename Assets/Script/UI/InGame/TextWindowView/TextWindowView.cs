using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PublicSet;


public class TextWindowView : MonoBehaviour
{

    // 에디터에서 연결
    public Text textWindow;
    public Text Speaker;
    public PortraitImage portraitImage;
    private List<cTextScriptInfo> textScriptDataList;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // 스크립트 수정
    public bool isTypingReady {  get; private set; }
    float typingDelay;
    int TextIndex;
    public Coroutine currentCoroutine {  get; private set; }
    private cTextScriptInfo textScriptData { get; set; }
    private eTextType currentTextType { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isTypingReady = true;
        typingDelay = 0.05f;
    }



    private void TextViewStartProcess_FrontEnd()
    {
        if(gameObject.activeInHierarchy == false) gameObject.SetActive(true);

        // 셀렉션뷰가 처음부터 켜져있는 오류 방지
        if (selectionView.activeSelf == true) selectionView.SetActive(false);

        if (currentCoroutine != null) // 대화창이 진행중이면 현재대화창을 안전하게 종료
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Pause_theGame();

            // 대화를 시작할때마다 화살표 이미지 살림
            arrowImageTrans.gameObject.SetActive(true);

            // 화살표 이미지 애니메이션
            ArrowDoTween();

            // 시작 텍스트 인덱스
            TextIndex = 0;
        }
    }
    private void TextViewStartProcess_BackEnd()
    {
        // 상호작용을 시작할때 즉시 스크립트가 출력되도록 만듬
        if (textScriptDataList != null)
        {
            if (isTypingReady)
            {
                PrintText();
            }
        }
        else
        {
            Debug.LogWarning("textScriptDataList가 비어있음");
        }
    }

    public void StartTextWindow( eTextScriptFile textFileEnum = eTextScriptFile.None)
    {
        TextViewStartProcess_FrontEnd();

        // 상호작용을 위한 기본처리
        DefaultProcess(textFileEnum);

        TextViewStartProcess_BackEnd();
    }

    public void StartTextWindow(eOOLProgress progress)
    {
        TextViewStartProcess_FrontEnd();

        // 상호작용을 위한 기본처리
        DefaultProcess(progress);

        TextViewStartProcess_BackEnd();

    }

    private void OnDisable()
    {
        // 셀렉션뷰를 끔으로써 셀렉션뷰가 초기화되도록만듬
        selectionView.gameObject.SetActive(false);

        // 리스트의 count를 0으로 만들어서 오류를 방지
        textScriptDataList = new List<cTextScriptInfo>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Continue_theGame();
        }
    }

    public void NextPrintButton()
    {

        // 상호작용했고 스크립트를 읽어왔으면
        if (textScriptDataList.Count >= 1)
        {
            // selection이 없으면
            if (textScriptData.hasSelection == eHasSelection.No)
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
            else if (textScriptData.hasSelection == eHasSelection.yes)
            {
                // 타이핑이 끝난 경우 selectionView를 활성화
                if (isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView selectionView_Script = selectionView.GetComponent<SelectionView>();

                    // 셀렉션 스크립트 부여 및 콜백 등록
                    for (int i = 0; i < textScriptData.selectionScript.Count && i < textScriptData.SelectionCallback.Count; i++)
                    {
                        if (textScriptData.SelectionCallback[i] != null)
                        {
                            selectionView_Script.RegisterButtonClick_Selection(
                                i, textScriptData.selectionScript[i], textScriptData.SelectionCallback[i]);
                        }
                        else
                        {
                            Debug.LogAssertion("셀렉션 콜백함수가 정의되지 않았음");
                        }
                        
                    }
                }
                // 타이핑이 안끝났으면 타이핑을 끝내기
                else
                {
                    isTypingReady = true;
                }
            }
        }
    }

    // 텍스트 출력을 위한 기본 처리
    private void DefaultProcess(eTextScriptFile fileEnum)
    {
        //현재 읽어오려는 텍스트타입 저장
        currentTextType = eTextType.TextScriptFile;

        // 어떤파일인지 지시하지 않은 경우 ex) 상호작용파일
        if (fileEnum == eTextScriptFile.None)
        {
            // 플레이어의 레이캐스트에 걸리는 객체
            GameObject curruntObject = GameManager.connector.player.GetComponent<Player_MoveAndAnime>().hitObject;

            if (curruntObject != null)
            {
                // csv데이터 가져오기

                InteractableObject Script = curruntObject.GetComponent<InteractableObject>();
                eTextScriptFile interactionFileEnum = Script.GetInteractableEnum();

                // 성공적으로 파일번호를 받아왔으면 데이터를 저장
                if(interactionFileEnum != eTextScriptFile.None)
                {
                    eStage currentStage = GameManager.Instance.currentStage;
                    textScriptDataList = CsvManager.Instance.GetTextScript(interactionFileEnum, currentStage);
                }

                // 파일번호를 아직 받지 못한 객체이면 텍스트창을 종료
                else
                {
                    CallbackManager.Instance.TextWindowPopUp_Close();
                }
                
            }
            else
            {
                Debug.LogWarning($"curruntObject : {curruntObject}");
            }
        }

        // 파일 이름이 지시된 경우
        else
        {
            eStage currentStage = GameManager.Instance.currentStage;
            textScriptDataList = CsvManager.Instance.GetTextScript(fileEnum, currentStage);
        }
    }
    private void DefaultProcess(eOOLProgress progress)
    {
        

        //현재 읽어오려는 텍스트타입 저장
        currentTextType = eTextType.OnlyOneLivesProgress;

        textScriptDataList = CsvManager.Instance.GetTextScript(progress);
    }

    public void PrintText()
    {
        // 배열 범위오류 제한
        if (TextIndex < textScriptDataList.Count)
        {
            textScriptData = textScriptDataList[TextIndex++];
            Speaker.text = CsvManager.Instance.GetCharacterInfo(textScriptData.characterEnum).CharacterName;
            portraitImage.TryChangePortraitImage(textScriptData.characterEnum, textScriptData.DialogueIconIndex);

            // 코루틴을 안전하게 처리
            StartReadText(TypeDialogue(textScriptData.script));
            
            // 마지막 대화에서 다음 대화 이미지(화살표) 삭제
            if (TextIndex == textScriptDataList.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }

        // 텍스트가 다 끝난경우
        if (TextIndex >= textScriptDataList.Count)
        {
            Debug.Log($"현재 실행된 문자열의 개수 == {textScriptDataList.Count}");
            CallbackManager.Instance.TextWindowPopUp_Close();

            // 대화창을 한번 끄고 나서 엔드콜백을 실행함
            if (textScriptData.hasEndCallback == eHasEndCallback.yes)
            {
                if(textScriptData.endCallback !=null)
                {
                    textScriptData.endCallback();
                }
                else
                {
                    Debug.LogAssertion("엔드콜백이 정의되지 않았음");
                }
                
            }
            
        }


    }

    private void StartReadText(IEnumerator textRoutine)
    {
        if(currentCoroutine != null)
        {
            isTypingReady = true;
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(textRoutine);
    }

    public void TextIndexInit(int value)
    {
        TextIndex = value;
    }

    

    IEnumerator TypeDialogue(string dialogue)
    {
        // 텍스트에서 변수 처리
        if(currentTextType == eTextType.TextScriptFile)
        {
            if (dialogue.Contains("{Month}"))
            {
                dialogue = dialogue.Replace("{Month}", GameManager.Instance.Month.ToString());

                if (dialogue.Contains("{Day}"))
                {
                    dialogue = dialogue.Replace("{Day}", GameManager.Instance.Day.ToString());
                }

                if (dialogue.Contains("{d-Day}"))
                {
                    int d_day = 31 - GameManager.Instance.Day;
                    if (d_day > 0)
                    {
                        dialogue = dialogue.Replace("{d-Day}", d_day.ToString() + "일 이겠군");
                    }
                    else if (d_day == 0)
                    {
                        dialogue = dialogue.Replace("{d-Day}", "오늘이 마지막이겠군");
                    }

                }
            }
        }
        else if(currentTextType == eTextType.OnlyOneLivesProgress)
        {
            if (dialogue.Contains("{ATTACKER}"))
            {
                dialogue = dialogue.Replace("{ATTACKER}", CardGamePlayManager.Instance.Attacker.characterInfo.CharacterName);
            }

            if (dialogue.Contains("{DEFENDER}"))
            {
                dialogue = dialogue.Replace("{DEFENDER}", CardGamePlayManager.Instance.Deffender.characterInfo.CharacterName);
            }

            if (dialogue.Contains("{JOKER}"))
            {
                dialogue = dialogue.Replace("{JOKER}", CardGamePlayManager.Instance.Joker.characterInfo.CharacterName);
            }

            if (dialogue.Contains("{VICTIM}"))
            {
                dialogue = dialogue.Replace("{VICTIM}", CardGamePlayManager.Instance.Victim.characterInfo.CharacterName);
            }

            if (dialogue.Contains("{EXPRESSION}"))
            {
                dialogue = dialogue.Replace("{EXPRESSION}", CardGamePlayManager.Instance.ExpressionValue.ToString());
            }
        }
        
        
        isTypingReady = false;
        textWindow.text = "";
        foreach (char letter in dialogue)
        {
            // UI에 글자를 타이핑
            // 줄바꿈 문자를 별도로 구별
            if (letter == '_')
            {
                textWindow.text += '\n';
            }
            else
            {
                textWindow.text += letter;
            }

            // 다시 버튼을 안눌렀으면 문자가 하나씩 나타남
            if (isTypingReady == false)
            {
                // 공백과 개행문자를 제외한 문자만 타이핑 딜레이를 적용
                if (letter != ' ' || letter != '_')
                {
                    yield return new WaitForSeconds(typingDelay);
                }

            }
        }
        isTypingReady = true;
        currentCoroutine = null;
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

    public void SkipText()
    {
        // 현재 텍스트에 셀렉션이 있으면
        if (textScriptData.hasSelection == eHasSelection.yes)
        {
            // 타이핑 종료
            isTypingReady = true;

            // 타이핑 종료 후 선택지 활성화 절차 실행 후 종료
            NextPrintButton();
            return;
        }

        // 셀렉션이 없으면 셀렉션을 포함한 텍스트가 나올 때 까지 반복
        do
        {
            if (TextIndex < textScriptDataList.Count)
            {
                textScriptData = textScriptDataList[TextIndex++];
            }

            // 대화가 끝날때까지 선택지가 없으면 그대로 대화창을 종료
            else
            {
                isTypingReady = true;

                // printText의 if (TextIndex >= textScriptDataList.Count) 분기 실행
                PrintText();
                
                return;
            }
        }
        while (textScriptData.hasSelection == eHasSelection.No);

        // PrintText에서도 다음인덱스의 데이터를 받기때문에 인덱스의 숫자를 1 줄임
        TextIndex--;

        // 셀렉션이 있는 텍스트의 printText 시작
        PrintText();

        // 바로 타이핑 종료
        isTypingReady = true;

        // 타이핑 종료 후 선택지 활성화 절차 실행 및 종료
        // else if (textScriptData.eSelect == eSelection.Exist) 분기 실행
        NextPrintButton();

        return;

    }

}


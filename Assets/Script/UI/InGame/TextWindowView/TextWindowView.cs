using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PublicSet;
using System;

public class TextWindowView : MonoBehaviour
{

    // 에디터에서 연결
    public Text textWindow;
    public Text Speaker;
    private List<eTextScriptInfo> textScriptDataList;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // 스크립트 수정
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    eTextScriptInfo textScriptData { get; set; }
    GameObject LastObject;
    eTextScriptFile currentTextFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isTypingReady = true;
        typingDelay = 0.05f;
    }


    public void StartTestWindow( eTextScriptFile textFileEnum = eTextScriptFile.None)
    {
        currentTextFile = textFileEnum;

        // 셀렉션뷰가 처음부터 켜져있는 오류 방지
        if (selectionView.activeSelf == true) selectionView.SetActive(false);

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

        // 상호작용을 위한 기본처리
        InteractionProcess(textFileEnum);

        // 상호작용을 시작할때 즉시 스크립트가 출력되도록 만듬
        if (textScriptDataList != null)
        {
            if (isTypingReady)
            {
                PrintText();
            }
        }

    }

    private void OnDisable()
    {
        // 셀렉션뷰를 끔으로써 셀렉션뷰가 초기화되도록만듬
        selectionView.gameObject.SetActive(false);

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
            if (textScriptData.eSelect == eSelection.NoneExist)
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
            else if (textScriptData.eSelect == eSelection.Exist)
            {
                // 타이핑이 끝난 경우 selectionView를 활성화
                if (isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView selectionView_Script = selectionView.GetComponent<SelectionView>();

                    // 셀렉션 스크립트 부여 및 콜백 등록
                    for (int i = 0; i < textScriptData.selection.Count && i < textScriptData.callback.Count; i++)
                    {

                        selectionView_Script.RegisterButtonClick_Selection(i, textScriptData.selection[i], textScriptData.callback[i]);
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

    // 상호작용을 위한 기본 처리
    private void InteractionProcess(eTextScriptFile fileEnum)
    {
        // 어떤파일인지 지시하지 않은 경우 ex) 상호작용파일
        if(fileEnum == eTextScriptFile.None)
        {
            // 플레이어의 레이캐스트에 걸리는 객체
            GameObject curruntObject = GameManager.Connector.player.GetComponent<Player_MoveAndAnime>().hitObject;

            if (curruntObject != null)
            {
                // csv데이터 가져오기

                InteractableObject Script = curruntObject.GetComponent<InteractableObject>();
                eTextScriptFile interactionFileEnum = Script.GetInteractableEnum();
                eStage currentStage = GameManager.Instance.currentStage;
                textScriptDataList = CsvManager.Instance.GetTextScript(interactionFileEnum, currentStage);
            }

            // 마지막 상호작용한 객체를 저장
            LastObject = curruntObject;
        }

        // 파일 이름이 지시된 경우
        else
        {
            eStage currentStage = GameManager.Instance.currentStage;
            textScriptDataList = CsvManager.Instance.GetTextScript(fileEnum, currentStage);
        }
    }

    public void PrintText()
    {
        // 배열 범위오류 제한
        if (TextIndex < textScriptDataList.Count)
        {
            textScriptData = textScriptDataList[TextIndex++];
            Speaker.text = textScriptData.speaker;
            StartCoroutine(TypeDialogue(textScriptData.script));

            // 마지막 대화에서 다음 대화 이미지(화살표) 삭제
            if (TextIndex == textScriptDataList.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }

        // 텍스트가 다 끝난경우
        if (TextIndex >= textScriptDataList.Count)
        {
            CallbackManager.Instance.TextWindowPopUp_Close();

            if (currentTextFile == eTextScriptFile.PlayerTutorial
                && GameManager.Instance.currentStage == eStage.Stage1)
            {
                GameManager.Instance.StageAnimation();
            }
        }


    }


    //private void PrintText_Tamplate<T>(List<T> textCsv, Action callback)
    //{
    //    // 배열 범위오류 제한
    //    if (TextIndex < textCsv.Count)
    //    {
    //        textScriptData = textScriptDataList[TextIndex++];
    //        Speaker.text = textScriptData.speaker;
    //        StartCoroutine(TypeDialogue(textScriptData.script));

    //        // 마지막 대화에서 다음 대화 이미지(화살표) 삭제
    //        if (TextIndex == textCsv.Count)
    //            arrowImageTrans.gameObject.SetActive(false);
    //        return;
    //    }

    //    // 텍스트가 다 끝난경우
    //    if (TextIndex >= textCsv.Count)
    //    {
    //        CallbackManager.Instance.TextWindowPopUp_Close();

    //        if (currentTextFile == eTextScriptFile.PlayerTutorial 
    //            && GameManager.Instance.currentStage == eStage.Stage1)
    //        {
    //            GameManager.Instance.StageAnimation();
    //        }
    //    }
    //}

    public void TextIndexInit(int value)
    {
        TextIndex = value;
    }


    IEnumerator TypeDialogue(string dialogue)
    {
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
            //else // 타이핑 도중 화면을 클릭하면 타이핑을 없이 전체 문장을 보여줌
            //{
            //    //textWindow.text = dialogue;
            //    //break;

            //    // 줄바꿈 문제를 별도로 구별
            //    if (letter == '_')
            //    {
            //        textWindow.text += '\n';
            //    }
            //    else
            //    {
            //        textWindow.text += letter;
            //    }
            //}
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

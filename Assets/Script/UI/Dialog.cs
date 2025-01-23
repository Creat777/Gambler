using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dialog : MonoBehaviour
{
    public RectTransform arrowImageTrans;
    public Text dialogueText;
    public float typingSpeed;
    private string currentDialogue;
    private string[] TextArr; // 텍스트파일에서 로딩
    private int TextIndex;
    bool isTyping;

    // csv로 대사 관리를 해보자

    public void TextLoading()
    {
        TextAsset Text = Resources.Load<TextAsset>("PopupScript");
        if (Text != null)
        {
            TextArr = Text.text.Split('\n');
        }
        else
        {
            Debug.LogError("Text파일 로딩에 실패했습니다.");
        }
    }

    public void Button_NextDialogue()
    {
        ButtonClickAudio.Instance.PlayClip();
        // 타이핑
        if (isTyping == false)
        {
            if (TextIndex < TextArr.Length)
            { 
                // 텍스트 전환
                currentDialogue = TextArr[TextIndex++];

                // 텍스트 순차적으로 보이게함
                StartCoroutine(TypeDialogue(currentDialogue));

                // 마지막 대화에서 다음 대화 이미지 삭제
                if(TextIndex == TextArr.Length)
                    arrowImageTrans.gameObject.SetActive(false);
                return;
            }
            else
            {
                //Debug.Log("텍스트 출력 완료됨");
                closeBtn();
            }
        }
        else
        {
            isTyping = false;
        }
    }

    

    void Start()
    {
        isTyping = false;
        TextIndex = 0;
        TextLoading();

        // 화살표 이미지 애니메이션
        ArrowDoTween();

        // 대화창 실행
        currentDialogue = TextArr[TextIndex++];
        StartCoroutine(TypeDialogue(currentDialogue));
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

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in dialogue.ToCharArray())
        {
            if (isTyping == true) // 다시 버튼을 안눌렀으면 문자가 하나씩 나타남
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(1 / typingSpeed);
            }
            else // 타이핑 도중 화면을 클릭하면 타이핑을 멈추고 전체 문장을 보여줌
            {
                dialogueText.text = dialogue;
                break;
            }
        }
        isTyping = false;
    }

    public void closeBtn()
    {
        ButtonClickAudio.Instance.PlayClip();
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }

}

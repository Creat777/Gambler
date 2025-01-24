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

    // �����Ϳ��� ����
    public Text textWindow;
    CsvInfo csvInfo;
    private List<string[]> TextSet;
    public RectTransform arrowImageTrans;
    public GameObject SelectionView;

    // ��ũ��Ʈ ����
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

        // ��ȭ�� �����Ҷ����� ȭ��ǥ �̹��� �츲
        arrowImageTrans.gameObject.SetActive(true);

        // ȭ��ǥ �̹��� �ִϸ��̼�
        ArrowDoTween();


        if (Player.Instance != null)
        {
            // ���� �ؽ�Ʈ �ε���
            TextIndex = 0;

            // ��ȣ�ۿ��� ���� �⺻ó��
            InteractiveProcess();

            // ��ȣ�ۿ��� �����Ҷ� ��� ��ũ��Ʈ�� ��µǵ��� ����
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

            // �����̽��ٸ� ������ �̾ ��ũ��Ʈ ó���� ������
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextPrintButton();
            }
        }
    }

    public void NextPrintButton()
    {
        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (TextSet.Count >= 1)
        {
            if (isTypingReady)
            {
                PrintText();
            }
            else
            {
                // true�� �ٲ㼭 typing�� ����
                isTypingReady = true;
            }

        }
    }


    // ��ȣ�ۿ��� ���� �⺻ ó��
    private void InteractiveProcess()
    {
        // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
        string curruntObjectName = Player.Instance.hitObjectName;

        // ��ȣ�ۿ��Ͽ� ��ũ��Ʈ�� �о���� �ʾ�����
        if (LastObjectName != curruntObjectName)
        {
            // ��ũ��Ʈ�� �о����
            InitTextSet(curruntObjectName);

            // ������ ��ȣ�ۿ��� ��ü�� �̸��� ����
            LastObjectName = curruntObjectName;
            return;
        }
        else
        {
            //Debug.Log("�̹� ��ũ��Ʈ ����� �о�Խ��ϴ�");
        }
    }

    public void InitTextSet(string objectName)
    {
        TextSet = CsvManager.Instance.GetText(objectName, interactive);
    }

    public void PrintText()
    {
        // �迭 �������� ����
        if (TextIndex < TextSet.Count)
        {
            // �ؽ�Ʈ ��ȯ
            currentText = TextSet[TextIndex++][1];

            // �ؽ�Ʈ ���������� ���̰���
            StartCoroutine(TypeDialogue(currentText));

            // ������ ��ȭ���� ���� ��ȭ �̹��� ����
            if (TextIndex == TextSet.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // �ؽ�Ʈ�� �� �������
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
            if (isTypingReady == false) // �ٽ� ��ư�� �ȴ������� ���ڰ� �ϳ��� ��Ÿ��
            {
                // UI�� ���ڸ� Ÿ����
                textWindow.text += letter;

                // ������ ������ ���ڸ� Ÿ���� �����̸� ����
                if(letter != ' ')
                {
                    yield return new WaitForSeconds(typingDelay);
                }
                
            }
            else // Ÿ���� ���� ȭ���� Ŭ���ϸ� Ÿ������ ���߰� ��ü ������ ������
            {
                textWindow.text = dialogue;
                break;
            }
        }
        isTypingReady = true;
    }

    private void ArrowDoTween()
    {
        // doTween������ ���� ����
        Vector3 targetScale = Vector3.one * 1.2f; // Ŀ���� ũ��
        float duration = 0.5f; // �ִϸ��̼� ���� �ð�

        // DOTween Sequence ����
        Sequence sequence = DOTween.Sequence();

        // ���� ũ�� ����
        Vector3 originalScale = arrowImageTrans.localScale;
        sequence.Append(arrowImageTrans.DOScale(targetScale, duration)) // Ŀ���� �ִϸ��̼�
                .Append(arrowImageTrans.DOScale(originalScale, duration)) // ���� �ִϸ��̼�
                .SetLoops(-1); // ���� �ݺ�
    }

}

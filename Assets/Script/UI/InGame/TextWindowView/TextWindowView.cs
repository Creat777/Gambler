using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextWindowView : MonoBehaviour
{

    // �����Ϳ��� ����
    public Text textWindow;
    public Text Speaker;
    private List<string[]> TextSet;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
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
        
        // ��ȭ�� �����Ҷ����� ȭ��ǥ �̹��� �츲
        arrowImageTrans.gameObject.SetActive(true);

        // ȭ��ǥ �̹��� �ִϸ��̼�
        ArrowDoTween();


        if (PlayerMoveAndAnime.Instance != null)
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

    private void OnDisable()
    {
        // �����Ǻ並 �����ν� �����Ǻ䰡 �ʱ�ȭ�ǵ��ϸ���
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

    //        // �����̽��ٸ� ������ �̾ ��ũ��Ʈ ó���� ������
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            NextPrintButton();
    //        }
    //    }
    //}

    public void NextPrintButton()
    {
        // index 0 : ���ϴ� ���
        // index 1 : ��ũ��Ʈ
        // index 2 : selection ����(0, 1)
        // index 3 : 1��° ������
        // index 4 : 2��° ������
        // index 5 : 1�� �ݹ��ȣ
        // index 6 : 2�� �ݹ��ȣ

        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (TextSet.Count >= 1 )
        {
            // selection�� ������
            if (currentTextData[2] == "0")
            {
                // Ÿ������ ������� ���� Ÿ������ ����
                if (isTypingReady)
                {
                    PrintText();
                }
                // Ÿ������ ���� �ȳ��� ��� Ÿ������ ����
                else
                {
                    // true�� �ٲ㼭 typing�� ����
                    isTypingReady = true;
                }
            }

            // selection�� ������
            else if (currentTextData[2] == "1")
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if(isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView SV = selectionView.GetComponent<SelectionView>();

                    // currentTextData�� �ε���
                    // index 5 : 1�� �ݹ��ȣ
                    // index 6 : 2�� �ݹ��ȣ
                    int index_1; int.TryParse(currentTextData[5], out index_1);
                    int index_2; int.TryParse(currentTextData[6], out index_2);

                    SV.RegisterButtonClick_Selection1(CallBackManager.Instance.CallBackList(index_1, LastObject ));
                    SV.RegisterButtonClick_Selection2(CallBackManager.Instance.CallBackList(index_2, LastObject ));
                }
                // Ÿ������ �ȳ������� Ÿ������ ������
                else
                {
                    isTypingReady = true;
                }
            }

        }
    }


    // ��ȣ�ۿ��� ���� �⺻ ó��
    private void InteractiveProcess()
    {
        // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
        GameObject curruntObject = PlayerMoveAndAnime.Instance.hitObject;

        // ��ũ��Ʈ�� �о����
        InitTextSet(curruntObject.name);

        // ������ ��ȣ�ۿ��� ��ü�� ����
        LastObject = curruntObject;
        return;
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
            // �ؽ�Ʈ ������ ��ȯ
            // index 0 : ���ϴ� ���
            // index 1 : ��ũ��Ʈ
            // index 2 : selection ����(0, 1)
            // index 3 : 1��° ������
            // index 4 : 2��° ������
            // index 5 : 1�� �ݹ��ȣ
            // index 6 : 2�� �ݹ��ȣ
            currentTextData = TextSet[TextIndex++];

            // �ؽ�Ʈ ���������� ���̰���
            Speaker.text = currentTextData[0];
            StartCoroutine(TypeDialogue(currentTextData[1]));

            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == TextSet.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // �ؽ�Ʈ�� �� ������� // currentTextData[2] == "0"�� PrintText �������� �˻�����
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
        Vector3 originalScale = Vector3.one;
        sequence.Append(arrowImageTrans.DOScale(targetScale, duration)) // Ŀ���� �ִϸ��̼�
                .Append(arrowImageTrans.DOScale(originalScale, duration)) // ���� �ִϸ��̼�
                .SetLoops(-1); // ���� �ݺ�
        
    }

}

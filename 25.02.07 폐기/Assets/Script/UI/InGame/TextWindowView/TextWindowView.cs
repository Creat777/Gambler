using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextWindowView : MonoBehaviour
{
    public enum TextType
    {
        Interaction,
        Monologue
    }
    

    // �����Ϳ��� ����
    public Text textWindow;
    public Text Speaker;
    private List<string[]> TextSet;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    string[] currentTextData;
    GameObject LastObject;
    const string monologueKey = "PlayerMonologue";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isTypingReady = true;
        typingDelay = 0.05f;
    }


    public void StartTextWindow(TextType currentType)
    {
        if (GameManager.Instance != null)
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
            BasicProcess(currentType);

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
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Continue_theGame();
        }
    }



    // ������ ������ ������ ����
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
            // selection�� ���ų� �������� �ƿ� �������� ������
            if (currentTextData[2] == "0" || currentTextData.Length == 2)
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


    // �ؽ�Ʈ�� �޾ƿ��� ���� �⺻ó��
    private void BasicProcess(TextType currentType)
    {
        string FileName = string.Empty;
        switch (currentType)
        {
            case TextType.Interaction:
                {
                    // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
                    GameObject curruntObject = PlayerMoveAndAnime.Instance.hitObject;
                    FileName = curruntObject.name;

                    // ������ ��ȣ�ۿ��� ��ü�� ����
                    LastObject = curruntObject;
                }break;

            case TextType.Monologue: 
                {
                    FileName = monologueKey;
                }break;
        }

        // ��ũ��Ʈ�� �о����
        InitTextSet(FileName);
        return;
    }

    public void InitTextSet(string fileName)
    {
        TextSet = CsvManager.Instance.GetText(fileName);
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

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
    private List<CsvManager.IteractableInfoList> TextCsv;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    CsvManager.IteractableInfoList iteractableInfo;
    GameObject LastObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
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


        if (GameManager.Connector.player != null)
        {
            // ���� �ؽ�Ʈ �ε���
            TextIndex = 0;

            // ��ȣ�ۿ��� ���� �⺻ó��
            InteractiveProcess();

            // ��ȣ�ۿ��� �����Ҷ� ��� ��ũ��Ʈ�� ��µǵ��� ����
            if (TextCsv != null)
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

    public void NextPrintButton()
    {
        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (TextCsv.Count >= 1 )
        {
            // selection�� ������
            if (iteractableInfo.eSelect == CsvManager.IteractableInfoList.eSelection.NoneExist)
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
            else if (iteractableInfo.eSelect == CsvManager.IteractableInfoList.eSelection.Exist)
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if(isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView SV = selectionView.GetComponent<SelectionView>();

                    // ������ ��ũ��Ʈ �ο� �� �ݹ� ���
                    for (int i = 0; i < iteractableInfo.selection.Count; i++)
                    {
                        SV.RegisterButtonClick_Selection(i, iteractableInfo.selection[i], iteractableInfo.callback[i]);
                    }
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
        GameObject curruntObject = GameManager.Connector.player.GetComponent<Player_MoveAndAnime>().hitObject;

        if(curruntObject != null)
        {
            // csv������ ��������
            InitTextCsv(curruntObject);
        }

        // ������ ��ȣ�ۿ��� ��ü�� ����
        LastObject = curruntObject;
        return;
    }

    public void InitTextCsv(GameObject objectName)
    {
        InteractableObject Script = objectName.GetComponent<InteractableObject>();
        CsvManager.eCsvFile_InterObj currentObject = Script.GetInteractableEnum();
        eStage currentStage = GameManager.Instance.currentStage;
        TextCsv =  CsvManager.Instance.GetInteractableCsv(currentObject, currentStage);
    }

    public void PrintText()
    {
        // �迭 �������� ����
        if (TextIndex < TextCsv.Count)
        {
            iteractableInfo = TextCsv[TextIndex++];

            // �ؽ�Ʈ ���������� ���̰���
            Speaker.text = iteractableInfo.speaker;
            StartCoroutine(TypeDialogue(iteractableInfo.script));

            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == TextCsv.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // �ؽ�Ʈ�� �� ������� // currentTextData[2] == "0"�� PrintText �������� �˻�����
        if (TextIndex >= TextCsv.Count)
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

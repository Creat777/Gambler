using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PublicSet;
using System;

public class TextWindowView : MonoBehaviour
{

    // �����Ϳ��� ����
    public Text textWindow;
    public Text Speaker;
    private List<cIteractableInfo> InteractionTextCsv;
    private List<cPlayerMonologueInfo> MonologueTextCsv;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    cIteractableInfo iteractableInfo { get; set; }
    cPlayerMonologueInfo monologueInfo;
    eTextType currentTextType;
    GameObject LastObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isTypingReady = true;

        typingDelay = 0.05f;
    }


    public void StartTestWindow(eTextType textType, eCsvFile_PlayerMono monologue = eCsvFile_PlayerMono.None )
    {
        currentTextType = textType;

        // �����Ǻ䰡 ó������ �����ִ� ���� ����
        if(selectionView.activeSelf == true) selectionView.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Pause_theGame();

            // ��ȭ�� �����Ҷ����� ȭ��ǥ �̹��� �츲
            arrowImageTrans.gameObject.SetActive(true);

            // ȭ��ǥ �̹��� �ִϸ��̼�
            ArrowDoTween();

            // ���� �ؽ�Ʈ �ε���
            TextIndex = 0;

        }

        
        switch (currentTextType)
        {
            case eTextType.Interaction:
                {
                    // ��ȣ�ۿ��� ���� �⺻ó��
                    InteractionProcess();

                    // ��ȣ�ۿ��� �����Ҷ� ��� ��ũ��Ʈ�� ��µǵ��� ����
                    if (InteractionTextCsv != null)
                    {
                        if (isTypingReady)
                        {
                            PrintText();
                        }
                    }
                }
                break;
            case eTextType.PlayerMonologue:
                {
                    PlayerMonologueProcess(monologue);
                    if (MonologueTextCsv != null)
                    {
                        if (isTypingReady)
                        {
                            PrintText();
                        }
                    }
                }
                break;
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
        switch(currentTextType)
        {
            case eTextType.Interaction:
                NextButton_with_Interaction(); break;

            case eTextType.PlayerMonologue:
                NextButton_with_Monologue();  break;
        }
        
    }

    private void NextButton_with_Interaction()
    {
        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (InteractionTextCsv.Count >= 1)
        {
            // selection�� ������
            if (iteractableInfo.eSelect == eSelection.NoneExist)
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
            else if (iteractableInfo.eSelect == eSelection.Exist)
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if (isTypingReady)
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
    private void NextButton_with_Monologue()
    {
        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (MonologueTextCsv.Count >= 1)
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
    }

    // ��ȣ�ۿ��� ���� �⺻ ó��
    private void InteractionProcess()
    {
        // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
        GameObject curruntObject = GameManager.Connector.player.GetComponent<Player_MoveAndAnime>().hitObject;

        if(curruntObject != null)
        {
            // csv������ ��������

            InteractableObject Script = curruntObject.GetComponent<InteractableObject>();
            eCsvFile_InterObj currentObject = Script.GetInteractableEnum();
            eStage currentStage = GameManager.Instance.currentStage;
            InteractionTextCsv = CsvManager.Instance.GetInteractableCsv(currentObject, currentStage);
        }

        // ������ ��ȣ�ۿ��� ��ü�� ����
        LastObject = curruntObject;
        return;
    }

    private void PlayerMonologueProcess(eCsvFile_PlayerMono monologue)
    {
        eCsvFile_PlayerMono currentMonologue = monologue;
        eStage currentStage = GameManager.Instance.currentStage;
        MonologueTextCsv = CsvManager.Instance.GetPlayerMonologueCsv(currentMonologue, currentStage);
    }

    public void PrintText()
    {
        switch(currentTextType)
        {
            case eTextType.Interaction:
                PrintText_Tamplate(InteractionTextCsv,
                    ()=>
                    {
                        iteractableInfo = InteractionTextCsv[TextIndex++];
                        Speaker.text = iteractableInfo.speaker;
                        StartCoroutine(TypeDialogue(iteractableInfo.script));
                    }); break;

            case eTextType.PlayerMonologue:
                PrintText_Tamplate(MonologueTextCsv,
                    ()=>
                    {
                        monologueInfo = MonologueTextCsv[TextIndex++];
                        Speaker.text = monologueInfo.speaker;
                        StartCoroutine(TypeDialogue(monologueInfo.script));
                    }); break;
        }
        
    }

    /*
    private void PrintText_With_Interaction()
    {
        // �迭 �������� ����
        if (TextIndex < InteractionTextCsv.Count)
        {
            iteractableInfo = InteractionTextCsv[TextIndex++];

            // �ؽ�Ʈ ���������� ���̰���
            Speaker.text = iteractableInfo.speaker;
            StartCoroutine(TypeDialogue(iteractableInfo.script));

            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == InteractionTextCsv.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        // �ؽ�Ʈ�� �� ������� // currentTextData[2] == "0"�� PrintText �������� �˻�����
        if (TextIndex >= InteractionTextCsv.Count)
        {
            CallbackManager.Instance.TextWindowPopUp_Close();
        }
    }
    */

    private void PrintText_Tamplate<T>(List<T> textCsv, Action callback)
    {
        // �迭 �������� ����
        if (TextIndex < textCsv.Count)
        {
            callback();

            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == textCsv.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }

        // �ؽ�Ʈ�� �� �������
        if (TextIndex >= textCsv.Count)
        {
            CallbackManager.Instance.TextWindowPopUp_Close();
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

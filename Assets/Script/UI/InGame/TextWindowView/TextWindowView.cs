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
    private List<eTextScriptInfo> textScriptDataList;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
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

        // �����Ǻ䰡 ó������ �����ִ� ���� ����
        if (selectionView.activeSelf == true) selectionView.SetActive(false);

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

        // ��ȣ�ۿ��� ���� �⺻ó��
        InteractionProcess(textFileEnum);

        // ��ȣ�ۿ��� �����Ҷ� ��� ��ũ��Ʈ�� ��µǵ��� ����
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
        // �����Ǻ並 �����ν� �����Ǻ䰡 �ʱ�ȭ�ǵ��ϸ���
        selectionView.gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Continue_theGame();
        }
    }

    public void NextPrintButton()
    {

        // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
        if (textScriptDataList.Count >= 1)
        {
            // selection�� ������
            if (textScriptData.eSelect == eSelection.NoneExist)
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
            else if (textScriptData.eSelect == eSelection.Exist)
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if (isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView selectionView_Script = selectionView.GetComponent<SelectionView>();

                    // ������ ��ũ��Ʈ �ο� �� �ݹ� ���
                    for (int i = 0; i < textScriptData.selection.Count && i < textScriptData.callback.Count; i++)
                    {

                        selectionView_Script.RegisterButtonClick_Selection(i, textScriptData.selection[i], textScriptData.callback[i]);
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
    private void InteractionProcess(eTextScriptFile fileEnum)
    {
        // ��������� �������� ���� ��� ex) ��ȣ�ۿ�����
        if(fileEnum == eTextScriptFile.None)
        {
            // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
            GameObject curruntObject = GameManager.Connector.player.GetComponent<Player_MoveAndAnime>().hitObject;

            if (curruntObject != null)
            {
                // csv������ ��������

                InteractableObject Script = curruntObject.GetComponent<InteractableObject>();
                eTextScriptFile interactionFileEnum = Script.GetInteractableEnum();
                eStage currentStage = GameManager.Instance.currentStage;
                textScriptDataList = CsvManager.Instance.GetTextScript(interactionFileEnum, currentStage);
            }

            // ������ ��ȣ�ۿ��� ��ü�� ����
            LastObject = curruntObject;
        }

        // ���� �̸��� ���õ� ���
        else
        {
            eStage currentStage = GameManager.Instance.currentStage;
            textScriptDataList = CsvManager.Instance.GetTextScript(fileEnum, currentStage);
        }
    }

    public void PrintText()
    {
        // �迭 �������� ����
        if (TextIndex < textScriptDataList.Count)
        {
            textScriptData = textScriptDataList[TextIndex++];
            Speaker.text = textScriptData.speaker;
            StartCoroutine(TypeDialogue(textScriptData.script));

            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == textScriptDataList.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }

        // �ؽ�Ʈ�� �� �������
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
    //    // �迭 �������� ����
    //    if (TextIndex < textCsv.Count)
    //    {
    //        textScriptData = textScriptDataList[TextIndex++];
    //        Speaker.text = textScriptData.speaker;
    //        StartCoroutine(TypeDialogue(textScriptData.script));

    //        // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
    //        if (TextIndex == textCsv.Count)
    //            arrowImageTrans.gameObject.SetActive(false);
    //        return;
    //    }

    //    // �ؽ�Ʈ�� �� �������
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
            // UI�� ���ڸ� Ÿ����
            // �ٹٲ� ���ڸ� ������ ����
            if (letter == '_')
            {
                textWindow.text += '\n';
            }
            else
            {
                textWindow.text += letter;
            }

            // �ٽ� ��ư�� �ȴ������� ���ڰ� �ϳ��� ��Ÿ��
            if (isTypingReady == false)
            {
                // ����� ���๮�ڸ� ������ ���ڸ� Ÿ���� �����̸� ����
                if (letter != ' ' || letter != '_')
                {
                    yield return new WaitForSeconds(typingDelay);
                }

            }
            //else // Ÿ���� ���� ȭ���� Ŭ���ϸ� Ÿ������ ���� ��ü ������ ������
            //{
            //    //textWindow.text = dialogue;
            //    //break;

            //    // �ٹٲ� ������ ������ ����
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

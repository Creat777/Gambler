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
    public PortraitImage portraitImage;
    private List<cTextScriptInfo> textScriptDataList;
    public RectTransform arrowImageTrans;
    public GameObject selectionView;

    // ��ũ��Ʈ ����
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    public Coroutine currentCoroutine {  get; private set; }
    cTextScriptInfo textScriptData { get; set; }
    GameObject LastObject;
    eTextScriptFile currentTextFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isTypingReady = true;
        typingDelay = 0.05f;
    }




    public void StartTextWindow( eTextScriptFile textFileEnum = eTextScriptFile.None)
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
        else
        {
            Debug.LogWarning("textScriptDataList�� �������");
        }

    }

    private void OnDisable()
    {
        // �����Ǻ並 �����ν� �����Ǻ䰡 �ʱ�ȭ�ǵ��ϸ���
        selectionView.gameObject.SetActive(false);

        // ����Ʈ�� count�� 0���� ���� ������ ����
        textScriptDataList = new List<cTextScriptInfo>();

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
            if (textScriptData.hasSelection == eHasSelection.yes)
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
            else if (textScriptData.hasSelection == eHasSelection.No)
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if (isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView selectionView_Script = selectionView.GetComponent<SelectionView>();

                    // ������ ��ũ��Ʈ �ο� �� �ݹ� ���
                    for (int i = 0; i < textScriptData.selectionScript.Count && i < textScriptData.SelectionCallback.Count; i++)
                    {

                        selectionView_Script.RegisterButtonClick_Selection(i, textScriptData.selectionScript[i], textScriptData.SelectionCallback[i]);
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

                // ���������� ���Ϲ�ȣ�� �޾ƿ����� �����͸� ����
                if(interactionFileEnum != eTextScriptFile.None)
                {
                    eStage currentStage = GameManager.Instance.currentStage;
                    textScriptDataList = CsvManager.Instance.GetTextScript(interactionFileEnum, currentStage);
                }

                // ���Ϲ�ȣ�� ���� ���� ���� ��ü�̸� �ؽ�Ʈâ�� ����
                else
                {
                    CallbackManager.Instance.TextWindowPopUp_Close();
                }
                
            }
            else
            {
                Debug.LogWarning($"curruntObject : {curruntObject}");
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
            Speaker.text = CsvManager.Instance.GetCharacterInfo(textScriptData.characterEnum).CharacterName;
            portraitImage.TryChangePortraitImage(textScriptData.characterEnum);

            // �ڷ�ƾ�� �����ϰ� ó��
            StartReadText(TypeDialogue(textScriptData.script));
            
            // ������ ��ȭ���� ���� ��ȭ �̹���(ȭ��ǥ) ����
            if (TextIndex == textScriptDataList.Count)
                arrowImageTrans.gameObject.SetActive(false);
            return;
        }

        // �ؽ�Ʈ�� �� �������
        if (TextIndex >= textScriptDataList.Count)
        {
            if(textScriptData.hasEndCallback == eHasEndCallback.yes)
            {
                textScriptData.endCallback();
            }
            Debug.Log($"���� ����� ���ڿ��� ���� == {textScriptDataList.Count}");
            CallbackManager.Instance.TextWindowPopUp_Close();


            Debug.LogAssertion("Doto => csv���Ͽ� �ؽ�Ʈ�� ������� ������ �ݹ鿩�� ������ �߰��ϱ�");
            if (currentTextFile == eTextScriptFile.PlayerMonologue
                && GameManager.Instance.currentStage == eStage.Stage1)
            {
                GameManager.Instance.StageAnimation();
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
        // �ؽ�Ʈ���� ���� ó��
        if(dialogue.Contains("{Month}"))
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
                    dialogue = dialogue.Replace("{d-Day}", d_day.ToString() + "�� �̰ڱ�");
                }
                else if (d_day == 0)
                {
                    dialogue = dialogue.Replace("{d-Day}", "������ �������̰ڱ�");
                }

            }
        }
        
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

    public void SkipText()
    {
        // ���� �ؽ�Ʈ�� �������� ������
        if (textScriptData.hasSelection == eHasSelection.No)
        {
            // Ÿ���� ����
            isTypingReady = true;

            // Ÿ���� ���� �� ������ Ȱ��ȭ ���� ���� �� ����
            NextPrintButton();
            return;
        }

        // �������� ������ �������� ������ �ؽ�Ʈ�� ���� �� ���� �ݺ�
        do
        {
            if (TextIndex < textScriptDataList.Count)
            {
                textScriptData = textScriptDataList[TextIndex++];
            }

            // ��ȭ�� ���������� �������� ������ �״�� ��ȭâ�� ����
            else
            {
                isTypingReady = true;

                // printText�� if (TextIndex >= textScriptDataList.Count) �б� ����
                PrintText();
                
                return;
            }
        }
        while (textScriptData.hasSelection == eHasSelection.yes);

        // PrintText������ �����ε����� �����͸� �ޱ⶧���� �ε����� ���ڸ� 1 ����
        TextIndex--;

        // �������� �ִ� �ؽ�Ʈ�� printText ����
        PrintText();

        // �ٷ� Ÿ���� ����
        isTypingReady = true;

        // Ÿ���� ���� �� ������ Ȱ��ȭ ���� ���� �� ����
        // else if (textScriptData.eSelect == eSelection.Exist) �б� ����
        NextPrintButton();

        return;

    }

}


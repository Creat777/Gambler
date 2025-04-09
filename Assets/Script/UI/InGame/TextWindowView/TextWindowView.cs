using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PublicSet;


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

        // �����Ǻ䰡 ó������ �����ִ� ���� ����
        if (selectionView.activeSelf == true) selectionView.SetActive(false);

        if (currentCoroutine != null) // ��ȭâ�� �������̸� �����ȭâ�� �����ϰ� ����
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

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
    }
    private void TextViewStartProcess_BackEnd()
    {
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

    public void StartTextWindow( eTextScriptFile textFileEnum = eTextScriptFile.None)
    {
        TextViewStartProcess_FrontEnd();

        // ��ȣ�ۿ��� ���� �⺻ó��
        DefaultProcess(textFileEnum);

        TextViewStartProcess_BackEnd();
    }

    public void StartTextWindow(eOOLProgress progress)
    {
        TextViewStartProcess_FrontEnd();

        // ��ȣ�ۿ��� ���� �⺻ó��
        DefaultProcess(progress);

        TextViewStartProcess_BackEnd();

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
            if (textScriptData.hasSelection == eHasSelection.No)
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
            else if (textScriptData.hasSelection == eHasSelection.yes)
            {
                // Ÿ������ ���� ��� selectionView�� Ȱ��ȭ
                if (isTypingReady)
                {
                    selectionView.SetActive(true);
                    SelectionView selectionView_Script = selectionView.GetComponent<SelectionView>();

                    // ������ ��ũ��Ʈ �ο� �� �ݹ� ���
                    for (int i = 0; i < textScriptData.selectionScript.Count && i < textScriptData.SelectionCallback.Count; i++)
                    {
                        if (textScriptData.SelectionCallback[i] != null)
                        {
                            selectionView_Script.RegisterButtonClick_Selection(
                                i, textScriptData.selectionScript[i], textScriptData.SelectionCallback[i]);
                        }
                        else
                        {
                            Debug.LogAssertion("������ �ݹ��Լ��� ���ǵ��� �ʾ���");
                        }
                        
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

    // �ؽ�Ʈ ����� ���� �⺻ ó��
    private void DefaultProcess(eTextScriptFile fileEnum)
    {
        //���� �о������ �ؽ�ƮŸ�� ����
        currentTextType = eTextType.TextScriptFile;

        // ��������� �������� ���� ��� ex) ��ȣ�ۿ�����
        if (fileEnum == eTextScriptFile.None)
        {
            // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
            GameObject curruntObject = (GameManager.connector as Connector_InGame).player.GetComponent<Player_MoveAndAnime>().hitObject;

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
        }

        // ���� �̸��� ���õ� ���
        else
        {
            eStage currentStage = GameManager.Instance.currentStage;
            textScriptDataList = CsvManager.Instance.GetTextScript(fileEnum, currentStage);
        }
    }
    private void DefaultProcess(eOOLProgress progress)
    {
        

        //���� �о������ �ؽ�ƮŸ�� ����
        currentTextType = eTextType.OnlyOneLivesProgress;

        textScriptDataList = CsvManager.Instance.GetTextScript(progress);
    }

    public void PrintText()
    {
        // �迭 �������� ����
        if (TextIndex < textScriptDataList.Count)
        {
            textScriptData = textScriptDataList[TextIndex++];
            Speaker.text = CsvManager.Instance.GetCharacterInfo(textScriptData.characterEnum).CharacterName;
            portraitImage.TryChangePortraitImage(textScriptData.characterEnum, textScriptData.DialogueIconIndex);

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
            Debug.Log($"���� ����� ���ڿ��� ���� == {textScriptDataList.Count}");
            CallbackManager.Instance.TextWindowPopUp_Close();

            // ��ȭâ�� �ѹ� ���� ���� �����ݹ��� ������
            if (textScriptData.hasEndCallback == eHasEndCallback.yes)
            {
                if(textScriptData.endCallback !=null)
                {
                    textScriptData.endCallback();
                }
                else
                {
                    Debug.LogAssertion("�����ݹ��� ���ǵ��� �ʾ���");
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
        // �ؽ�Ʈ���� ���� ó��
        if(currentTextType == eTextType.TextScriptFile)
        {
            if (dialogue.Contains("{D-DAY}"))
            {
                dialogue = dialogue.Replace("{D-DAY}", GameManager.Instance.currentRemainingPeriod.ToString());
            }

            if (dialogue.Contains("{Additional coins}"))
            {
                int additionalCoins = 10000 - PlayManager.Instance.currentPlayerStatus.coin;
                dialogue = dialogue.Replace("{Additional coins}", additionalCoins.ToString());
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

            if (dialogue.Contains("{PREY}"))
            {
                dialogue = dialogue.Replace("{PREY}", CardGamePlayManager.Instance.Prey.characterInfo.CharacterName);
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
        currentCoroutine = null;
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
        if (textScriptData.hasSelection == eHasSelection.yes)
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
        while (textScriptData.hasSelection == eHasSelection.No);

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


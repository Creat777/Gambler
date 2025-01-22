using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class TextWindowView : MonoBehaviour
{
    // �����Ϳ��� ����
    public Text textWindow;
    CsvInfo csvInfo;
    private string[] TextSet;

    // ��ũ��Ʈ ����
    Interactive interactive;
    bool isInteractive;
    bool isTypingReady;
    float typingDelay;
    int TextIndex;
    string currentText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        interactive = new Interactive();
        isInteractive = false;
        isTypingReady = true;
        typingDelay = 0.05f;
    }

    private void OnEnable()
    {
        if (Player.Instance != null)
        {
            // ���� �ؽ�Ʈ
            TextIndex = 0;

            InteractiveProcess();

            // ��ȣ�ۿ��� �����Ҷ� ��� ��ũ��Ʈ�� ��µǵ��� ����
            if (isInteractive && TextSet.Length >= 1)
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
                // ��ȣ�ۿ��߰� ��ũ��Ʈ�� �о������
                if (isInteractive && TextSet.Length >= 1)
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
        }
        
    }


    // ��ȣ�ۿ��� ���� �⺻ ó��
    private void InteractiveProcess()
    {
        // �÷��̾��� ����ĳ��Ʈ�� �ɸ��� ��ü
        string objectName = Player.Instance.hitObjectName;

        // �÷��̾ ���� ��ȣ�ۿ��� �� �ִ� ��ü�� ������
        if (objectName != null)
        {
            Debug.Log($"���� �÷��̾ ��ȣ�ۿ��ϴ� ��ü : {objectName}");
            // ��ȣ�ۿ��Ͽ� ��ũ��Ʈ�� �о���� �ʾ�����
            if (isInteractive == false)
            {
                // ��ũ��Ʈ�� �о����
                GetTextSet(objectName);

                // ��ũ��Ʈ �о������ ����
                isInteractive = true;
                return;
            }
        }

        // �÷��̾ ���� ��ȣ�ۿ��� �� �ִ� ��ü�� ������
        else
        {
            // ���Ӱ� ��ȣ�ۿ��Ͽ� ��ũ��Ʈ�� �о�� �� �ֵ��� ����
            isInteractive = false;
            return;
        }
    }

    public void GetTextSet(string objectName)
    {
        CsvInfo[] csvInfos = CsvManager.Instance.InteractiveCsvInfos;

        // �÷��̾ ��ȣ�ۿ��� �� �ִ� ��ü���� Ȯ��
        csvInfo = CsvProcessor.Instance.FindCsvInfo(objectName, csvInfos, interactive);

        // ��ü�� ������ ����� ��ũ��Ʈ�� ��ȯ
        if (csvInfo != null)
        {
            Debug.Log($"GetTextSet�� �о�� ��ũ��Ʈ���� : {csvInfo.CsvFileName}");
            TextSet = CsvProcessor.Instance.GetText(csvInfo, interactive);
        }
    }

    private void PrintText()
    {
        if (TextIndex < TextSet.Length)
        {
            // �ؽ�Ʈ ��ȯ
            currentText = TextSet[TextIndex++];

            // �ؽ�Ʈ ���������� ���̰���
            StartCoroutine(TypeDialogue(currentText));

            // ������ ��ȭ���� ���� ��ȭ �̹��� ����
            if (TextIndex == TextSet.Length)
                //arrowImageTrans.gameObject.SetActive(false);
            return;
        }
        if(TextIndex >= TextSet.Length)
        {
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

    // DOTO -> �ڷ�ƾ���� �ؽ�Ʈ�� ȭ�鿡 ���

}

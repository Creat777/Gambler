using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;
using System.Collections;

public class IconView : MonoBehaviour
{
    // ������ ����
    public RectTransform rectTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public Button quest;
    public Button inventory;
    public Button gameAssistance;
    public Button message;

    public GameObject quest_Lock;
    public GameObject inventory_Lock;
    public GameObject gameAssistance_Lock;
    public GameObject message_Lock;

    // ��ũ��Ʈ ����
    private Vector2 Center_anchoredPos;
    private Vector2 OutOfScreen_anchoredPos;
    bool isIconViewOpen;
    Coroutine currentCoroutine;
    public int OpenedIconCount {  get; private set; }
    public void SetOpendIconCount(int value)
    {
        OpenedIconCount = value;
        if(OpenedIconCount != 0)
        {
            switch(OpenedIconCount)
            {
                case 1:
                    Destroy(inventory_Lock);
                    inventory_Lock = null;
                    break;

                case 2:
                    Destroy(inventory_Lock);
                    Destroy(quest_Lock);
                    inventory_Lock = quest_Lock = null;
                    break;

                case 3:
                    Destroy(inventory_Lock);
                    Destroy(quest_Lock);
                    Destroy(gameAssistance_Lock);
                    inventory_Lock = quest_Lock = gameAssistance_Lock = null;
                    break;

                case 4:
                    Destroy(inventory_Lock);
                    Destroy(quest_Lock);
                    Destroy(gameAssistance_Lock);
                    Destroy(message_Lock);
                    inventory_Lock = quest_Lock = gameAssistance_Lock = message_Lock = null;
                    break;
                default: Debug.LogError($"�߸��� �Է� : SetOpendIconCount(value = {value})");break;

            }

            // ����������� ������ ��� ��ųʸ��� �ٽ� ����
            InitIconDict();
        }
    }

    private Dictionary<eIcon, GameObject> _iconLockDict;
    private Dictionary<eIcon, GameObject> iconLockDict
    {
        get
        {
            if (_iconLockDict == null)
            {
                InitIconDict();
            }
            return _iconLockDict;
        }
        set { _iconLockDict = value; }
    }

    public void InitIconDict()
    {
        _iconLockDict = new Dictionary<eIcon, GameObject>();

        if (quest_Lock != null)
            iconLockDict.Add(eIcon.Quest, quest_Lock);
        if (inventory_Lock != null)
            iconLockDict.Add(eIcon.Inventory, inventory_Lock);
        if (gameAssistance_Lock != null)
            iconLockDict.Add(eIcon.GameAssistant, gameAssistance_Lock);
        if (message_Lock != null)
            iconLockDict.Add(eIcon.Message, message_Lock);
    }

    private void Awake()
    {
        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }
        SetPos();

        InitIconDict();
        OpenedIconCount = 0;
    }

    private void SetPos()
    {
        OutOfScreen_anchoredPos = rectTrans.rect.size;
        OutOfScreen_anchoredPos.x = OutOfScreen_anchoredPos.x - OutOfScreen_anchoredPos.y;
        OutOfScreen_anchoredPos.y = -(OutOfScreen_anchoredPos.y/2);

        Center_anchoredPos = OutOfScreen_anchoredPos;
        Center_anchoredPos.x = 0f;

        rectTrans.anchoredPosition = OutOfScreen_anchoredPos;
    }


    /// <summary>
    /// ������ �䰡 ������ ���� �ð� �� �������� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator IconViewCloseDelay()
    {
        yield return new WaitForSeconds(5f);
        IconViewClose();
        currentCoroutine = null;
    }

    public void IconViewOpen()
    {
        PlaySequnce_IconViewProcess(Center_anchoredPos, true);
    }


    public void IconViewClose()
    {
        PlaySequnce_IconViewProcess(OutOfScreen_anchoredPos, false);
    }

    private void PlaySequnce_IconViewProcess(Vector3 tragetPos ,bool boolActive, Sequence sequencePlus = null)
    {
        // ���� �ʱ�ȭ
        isIconViewOpen = boolActive;
        if(isIconViewOpen)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(IconViewCloseDelay());
        }

        Sequence sequence = DOTween.Sequence();

        // iconView�� �����̰� iconView�� �¿��� ��ư�� ó��
        sequence.Append(rectTrans.DOAnchorPos(tragetPos, ViewOpenDelay))
                .AppendCallback(() => iconViewCloseButton.SetActive(boolActive));

        // ������ �䰡 ������ �� �߰����� ó���� �ʿ��ϸ� sequence�� �߰�
        if (sequencePlus != null)
        {
            sequence.Append(sequencePlus);
        }

        sequence.SetLoops(1);
        sequence.Play();
    }

    public bool TryIconUnLock(eIcon choice)
    {
        if(iconLockDict.ContainsKey(choice))
        {
            OpenedIconCount++;
            Sequence sequence = DOTween.Sequence();

            Transform iconLockTrans = iconLockDict[choice].transform;
            iconLockDict.Remove(choice);

            sequence.Append(iconLockTrans.DOScale(Vector3.one * 2f, 0.3f))
                    .Append(iconLockTrans.DOScale(Vector3.zero, 1f))
                    .AppendCallback(() => { Destroy(iconLockTrans.gameObject); })
                    .SetLoops(1);

            // �������� �����ִ� ���
            if (isIconViewOpen == false)
            {
                // IconViewProcess ���ο��� sequence�� �߰��Ͽ� Ʈ���� ������
                PlaySequnce_IconViewProcess(Center_anchoredPos, true, sequence);
            }
            // �������� �����ִ� ���
            else
            {
                sequence.Play();
            }
            return true;
        }
        else
        {
            Debug.Log($"{choice.ToString()}�� �����ġ�� �̹� �Ҹ�����");
            return false;
        }
        
    }
}

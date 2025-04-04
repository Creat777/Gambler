using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;

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
    float openDuration;

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
    }

    public void InitIconDict()
    {
        _iconLockDict = new Dictionary<eIcon, GameObject>();
        _iconLockDict.Add(eIcon.Quest, quest_Lock);
        _iconLockDict.Add(eIcon.Inventory, inventory_Lock);
        _iconLockDict.Add(eIcon.GameAssistant, gameAssistance_Lock);
        _iconLockDict.Add(eIcon.Message, message_Lock);
    }

    private void Awake()
    {
        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }
        SetPos();

        if(iconLockDict == null) InitIconDict();
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


    private void FixedUpdate()
    {
        if(isIconViewOpen)
        {
            openDuration += Time.deltaTime;
            if (openDuration > 5f)
            {
                IconViewClose();
            }
        }
    }

    public void IconViewOpen()
    {
        IconViewProcess(Center_anchoredPos, true);
    }


    public void IconViewClose()
    {
        IconViewProcess(OutOfScreen_anchoredPos, false);
    }

    private void IconViewProcess(Vector3 tragetPos ,bool boolActive, Sequence sequencePlus = null)
    {
        // ���� �ʱ�ȭ
        openDuration = 0;
        isIconViewOpen = boolActive;

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
                IconViewProcess(Center_anchoredPos, true, sequence);
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
            Debug.Log($"{iconLockDict[choice]}�� �̹� �Ҹ�����");
            return false;
        }
        
    }
}

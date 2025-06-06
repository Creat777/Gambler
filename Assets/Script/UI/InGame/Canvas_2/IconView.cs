using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;
using System.Collections;

public class IconView : MonoBehaviour
{
    // 에디터 편집
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

    // 스크립트 편집
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
                default: Debug.LogError($"잘못된 입력 : SetOpendIconCount(value = {value})");break;

            }

            // 아이콘잠금이 삭제된 경우 딕셔너리를 다시 구성
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
    /// 아이콘 뷰가 열리면 일정 시간 후 닫히도록 설정
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
        // 변수 초기화
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

        // iconView가 움직이고 iconView의 온오프 버튼의 처리
        sequence.Append(rectTrans.DOAnchorPos(tragetPos, ViewOpenDelay))
                .AppendCallback(() => iconViewCloseButton.SetActive(boolActive));

        // 아이콘 뷰가 움직인 후 추가적인 처리가 필요하면 sequence에 추가
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

            // 아이콘이 닫혀있는 경우
            if (isIconViewOpen == false)
            {
                // IconViewProcess 내부에서 sequence를 추가하여 트위닝 시작함
                PlaySequnce_IconViewProcess(Center_anchoredPos, true, sequence);
            }
            // 아이콘이 열려있는 경우
            else
            {
                sequence.Play();
            }
            return true;
        }
        else
        {
            Debug.Log($"{choice.ToString()}의 잠금장치가 이미 소멸했음");
            return false;
        }
        
    }
}

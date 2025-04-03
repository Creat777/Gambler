using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;

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
        // 변수 초기화
        openDuration = 0;
        isIconViewOpen = boolActive;

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
                IconViewProcess(Center_anchoredPos, true, sequence);
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
            Debug.Log($"{iconLockDict[choice]}가 이미 소멸했음");
            return false;
        }
        
    }
}

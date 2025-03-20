using DG.Tweening;
using System.Collections;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class IconView : MonoBehaviour
{
    // 에디터 편집
    public RectTransform rectTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public Button inventory;
    public Button quest;
    public Button status;
    public Button Message;
    
    public GameObject[] iconLock;

    // 스크립트 편집
    private Vector2 Center_anchoredPos;
    private Vector2 OutOfScreen_anchoredPos;
    bool isIconViewOpen;

    Dictionary<eIcon, ePopUpState> iconConditions;


    


    private void Awake()
    {
        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }
        SetPos();
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

    

    
    private void Start()
    {
        PopUpView popUpView = GameManager.Connector.popUpView_Script;

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

    public void IconUnLock(eIcon choice)
    {
        int choice_int = (int)choice;


        if(eIcon.Inventory <= choice && choice <= eIcon.Message)
        {
            if (iconLock[choice_int].activeSelf == false)
            {
                Debug.LogWarning($"{iconLock[choice_int]}가 이미 소멸했음");
                return;
            }
            if (choice_int < iconLock.Length)
            {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(iconLock[choice_int].transform.DOScale(Vector3.one * 2f, 0.3f))
                        .Append(iconLock[choice_int].transform.DOScale(Vector3.zero, 1f))
                        .AppendCallback(() => { Destroy(iconLock[choice_int]); })
                        .SetLoops(1);

                // 아이콘이 닫혀있는 경우
                if (isIconViewOpen == false)
                {
                    // IconViewProcess 내부에서 sequence를 추가하여 트위닝 시작함
                    IconViewProcess(Center_anchoredPos, true, sequence);
                    return;
                }
                // 아이콘이 닫혀있는 경우
                else
                {
                    sequence.Play();
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("IconUnLock의 매개변수 오류");
            return;
        }
    }
}

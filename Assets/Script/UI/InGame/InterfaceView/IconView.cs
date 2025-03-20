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
    // ������ ����
    public RectTransform rectTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public Button inventory;
    public Button quest;
    public Button status;
    public Button Message;
    
    public GameObject[] iconLock;

    // ��ũ��Ʈ ����
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

    public void IconUnLock(eIcon choice)
    {
        int choice_int = (int)choice;


        if(eIcon.Inventory <= choice && choice <= eIcon.Message)
        {
            if (iconLock[choice_int].activeSelf == false)
            {
                Debug.LogWarning($"{iconLock[choice_int]}�� �̹� �Ҹ�����");
                return;
            }
            if (choice_int < iconLock.Length)
            {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(iconLock[choice_int].transform.DOScale(Vector3.one * 2f, 0.3f))
                        .Append(iconLock[choice_int].transform.DOScale(Vector3.zero, 1f))
                        .AppendCallback(() => { Destroy(iconLock[choice_int]); })
                        .SetLoops(1);

                // �������� �����ִ� ���
                if (isIconViewOpen == false)
                {
                    // IconViewProcess ���ο��� sequence�� �߰��Ͽ� Ʈ���� ������
                    IconViewProcess(Center_anchoredPos, true, sequence);
                    return;
                }
                // �������� �����ִ� ���
                else
                {
                    sequence.Play();
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("IconUnLock�� �Ű����� ����");
            return;
        }
    }
}

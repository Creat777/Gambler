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
    public Transform CenterTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public Button inventory;
    public Button quest;
    public Button status;
    public Button Message;
    
    public GameObject[] iconLock;

    // ��ũ��Ʈ ����
    private Vector3 CenterPos;
    private Vector3 OutOpScreenPos;
    bool isIconViewOpen;

    Dictionary<eIcon, ePopUpState> iconConditions;


    


    private void Awake()
    {
        CenterPos = CenterTrans.position;
        OutOpScreenPos = transform.position;

        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }

        //Init_IconToPopUpStateDict();
    }

    

    
    private void Start()
    {
        PopUpView popUpView = GameManager.Connector.popUpView_Script;

        /*
        // ��ư ������Ʈ�� �⺻ ���� �Լ��� ����
        ButtonOnClickUpdate(eIcon.Inventory, inventory, popUpView.InventoryPopUpOpen, popUpView.InventoryPopUpClose);
        ButtonOnClickUpdate(eIcon.Quest, quest, popUpView.QuestPopUpOpen, popUpView.QuestPopUpClose);
        ButtonClickUpdate(status, popUpView. , popUpView. );
        ButtonClickUpdate(Message, popUpView. , popUpView.);
        */
    }

    public void IconViewOpen()
    {
        IconViewProcess(CenterPos, true);
    }


    public void IconViewClose()
    {
        IconViewProcess(OutOpScreenPos, false);
    }

    private void IconViewProcess(Vector3 tragetPos ,bool boolActive, Sequence sequencePlus = null)
    {
        isIconViewOpen = boolActive;

        Sequence sequence = DOTween.Sequence();

        // iconView�� �����̰� iconView�� �¿��� ��ư�� ó��
        sequence.Append(transform.DOMove(tragetPos, ViewOpenDelay))
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
                    IconViewProcess(CenterPos, true, sequence);
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

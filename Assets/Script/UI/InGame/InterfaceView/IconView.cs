using DG.Tweening;
using System.Collections;
using UnityEngine;
using PublicSet;

public class IconView : MonoBehaviour
{
    // ������ ����
    public Transform CenterTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public GameObject[] iconLock;

    // ��ũ��Ʈ ����
    private Vector3 CenterPos;
    private Vector3 OutOpScreenPos;
    bool isIconViewOpen;
    

    private void Awake()
    {
        CenterPos = CenterTrans.position;
        OutOpScreenPos = transform.position;
        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }
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


    

    public void IconUnLock(Icon choice)
    {
        int choice_int = (int)choice;

        if(Icon.Inventory <= choice && choice <= Icon.Message)
        {
            if(choice_int < iconLock.Length)
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

    IEnumerator ProcessDelay(float  delay)
    {
        yield return new WaitForSeconds(delay);
    }
}

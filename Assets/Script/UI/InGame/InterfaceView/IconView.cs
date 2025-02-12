using DG.Tweening;
using System.Collections;
using UnityEngine;
using PublicSet;

public class IconView : MonoBehaviour
{
    // 에디터 편집
    public Transform CenterTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public GameObject[] iconLock;

    // 스크립트 편집
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

        // iconView가 움직이고 iconView의 온오프 버튼의 처리
        sequence.Append(transform.DOMove(tragetPos, ViewOpenDelay))
                .AppendCallback(() => iconViewCloseButton.SetActive(boolActive));

        // 아이콘 뷰가 움직인 후 추가적인 처리가 필요하면 sequence에 추가
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

                // 아이콘이 닫혀있는 경우
                if (isIconViewOpen == false)
                {
                    // IconViewProcess 내부에서 sequence를 추가하여 트위닝 시작함
                    IconViewProcess(CenterPos, true, sequence);
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

    IEnumerator ProcessDelay(float  delay)
    {
        yield return new WaitForSeconds(delay);
    }
}

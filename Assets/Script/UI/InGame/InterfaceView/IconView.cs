using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;

public class IconView : MonoBehaviour
{
    // 에디터 편집
    public Transform CenterTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public GameObject popUpView;
    public GameObject inventoryPopUp;

    // 스크립트 편집
    private Vector3 CenterPos;
    private Vector3 StartPos;
    

    private void Awake()
    {
        CenterPos = CenterTrans.position;
        StartPos = transform.position;
        if (ViewOpenDelay < 0.1f)
        {
            ViewOpenDelay = 0.3f;
        }
    }

    public void IconViewOpen()
    {
        DG.Tweening.Sequence sequnce = DOTween.Sequence();
        sequnce.Append(transform.DOMove(CenterPos, ViewOpenDelay));
        sequnce.AppendCallback(() => iconViewCloseButton.SetActive(true));
        sequnce.SetLoops(1);
        sequnce.Play();
    }

    public void IconViewClose()
    {
        DG.Tweening.Sequence sequnce = DOTween.Sequence();
        sequnce.Append(transform.DOMove(StartPos, ViewOpenDelay));
        sequnce.AppendCallback(() => iconViewCloseButton.SetActive(false));
        sequnce.SetLoops(1);
        sequnce.Play();
    }

    public void InventoryPopUpOpen()
    {
        popUpView.SetActive(true);
        inventoryPopUp.SetActive(true);
    }
}

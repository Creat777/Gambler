using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;

public class IconView : MonoBehaviour
{
    // ������ ����
    public Transform CenterTrans;
    public GameObject iconViewCloseButton;
    public float ViewOpenDelay;

    public GameObject popUpView;
    public GameObject inventoryPopUp;

    // ��ũ��Ʈ ����
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

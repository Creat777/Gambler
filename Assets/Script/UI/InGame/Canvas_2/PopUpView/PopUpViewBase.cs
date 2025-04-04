using UnityEngine;

public abstract class PopUpViewBase : MonoBehaviour
{
    // ÄÝ¹éÆË¾÷
    public YesOrNoPopUp yesOrNoPopUp;
    public CheckPopUp checkPopUp;

    private void Awake()
    {
        MakePopUpSingleTone();
    }

    public abstract void MakePopUpSingleTone();

    public void YesOrNoPopUpOpen()
    {
        //gameObject.SetActive(true);
        yesOrNoPopUp.gameObject.SetActive(true);
        yesOrNoPopUp.transform.SetAsLastSibling();
    }

    public void CheckPopUpOpen()
    {
        //gameObject.SetActive(true);
        checkPopUp.gameObject.SetActive(true);
        checkPopUp.transform.SetAsLastSibling();
    }
}

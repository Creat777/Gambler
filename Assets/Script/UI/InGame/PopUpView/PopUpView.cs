using UnityEngine;

public class PopUpView : MonoBehaviour
{
    public GameObject optionPopUp;
    public GameObject inventoryPopUp;
    public GameObject yesOrNoPopUp;
    public GameObject checkPopUp;
    public GameObject questPopUp;

    void Start()
    {
        
    }

    public void OptionPopUpOpen()
    {
        gameObject.SetActive(true);
        optionPopUp.SetActive(true);

    }

    public void InventoryPopUpOpen()
    {
        gameObject.SetActive(true);
        inventoryPopUp.SetActive(true);

    }

    public void YesOrNoPopUpOpen()
    {
        gameObject.SetActive(true);
        yesOrNoPopUp.SetActive(true);

    }

    public void CheckPopUpOpen()
    {
        gameObject.SetActive(true);
        checkPopUp.SetActive(true);

    }
    public void QuestPopUpOpen()
    {
        gameObject.SetActive(true);
        questPopUp.SetActive(true);

    }
}

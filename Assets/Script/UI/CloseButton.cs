using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void PopUpClose()
    {
        transform.parent.gameObject.SetActive(false);
        transform.parent.GetComponent<PopUp>().PopUpViewClose();
    }
}

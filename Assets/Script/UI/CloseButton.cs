using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void PopUpClose()
    {
        // ������ ���������� ���� ����
        if (GameManager.Instance.isGamePause)
        {
            GameManager.Instance.Continue_theGame();
        }

        //close��ư�� ���� ��ü�� ���� 
        transform.parent.gameObject.SetActive(false);

        // close��ư�� �ִ� â�� �����鼭 ViewPanel�� ����
        //transform.parent.GetComponent<PopUp>().PopUpViewClose();
    }

    public void PopUpClose_InCheckPopUp()
    {
        Transform Bottom = transform.parent;
        Transform CheckPopUp = Bottom.parent;
        CheckPopUp.gameObject.SetActive(false);
    }
}

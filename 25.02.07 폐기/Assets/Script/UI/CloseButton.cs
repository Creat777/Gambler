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

        // close��ư�� �ִ� â�� �����鼭 ViewPanel�� ����
        transform.parent.gameObject.SetActive(false);
        transform.parent.GetComponent<PopUp>().PopUpViewClose();
    }
}

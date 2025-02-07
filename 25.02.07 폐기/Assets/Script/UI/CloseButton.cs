using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void PopUpClose()
    {
        // 게임을 정지했으면 게임 지속
        if (GameManager.Instance.isGamePause)
        {
            GameManager.Instance.Continue_theGame();
        }

        // close버튼이 있는 창을 닫으면서 ViewPanel도 종료
        transform.parent.gameObject.SetActive(false);
        transform.parent.GetComponent<PopUp>().PopUpViewClose();
    }
}

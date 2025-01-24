using Unity.VisualScripting;
using UnityEngine;


// �����ͽ� �̱��� ��ü�� ó�� ���
public class InGameInitilizerSet : MonoBehaviour
{
    public KeyBoardView keyBoardView;
    public Joystick joystick;
    void Start()
    {
        // ���� �ϳ��� �����
        if(keyBoardView == null)
        {
            Player.Instance.keyBoardView = GameObject.Find("KeyBoardView").GetComponent<KeyBoardView>();
        }
        else if(keyBoardView == null)
        {
            //Player.Instance.joyStickVec = GameObject.Find("Joystick").GetComponent<Joystick>();
        }
        
        
        GameManager.Instance.Join_In_Game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

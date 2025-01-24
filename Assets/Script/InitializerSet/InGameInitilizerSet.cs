using Unity.VisualScripting;
using UnityEngine;


// ¾Àº¹±Í½Ã ½Ì±ÛÅæ °´Ã¼ÀÇ Ã³¸® ´ã´ç
public class InGameInitilizerSet : MonoBehaviour
{
    public KeyBoardView keyBoardView;
    public Joystick joystick;
    void Start()
    {
        // µÑÁß ÇÏ³ª´Â ¿¬°áµÊ
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

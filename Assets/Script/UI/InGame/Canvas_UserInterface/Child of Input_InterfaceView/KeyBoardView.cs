using UnityEngine;
using UnityEngine.UI;

public class KeyBoardView : InterfaceView
{
    // 에디터 연결
    public Image Key_Left;
    public Image Key_Down;
    public Image Key_Up;
    public Image Key_Right;
    public Image Key_Space;


    public void ChangeColer(int keyCode)
    {
        if(keyCode == (int)KeyCode.W)
        {
            Key_Up.color = Color.white * 0.8f;
        }
        else if (keyCode == (int)KeyCode.S)
        {
            Key_Down.color = Color.white * 0.8f;
        }
        else if (keyCode == (int)KeyCode.A)
        {
            Key_Left.color = Color.white * 0.8f;
        }
        else if (keyCode == (int)KeyCode.D)
        {
            Key_Right.color = Color.white * 0.8f;
        }
        else if (keyCode == (int)KeyCode.Space && isInteractiveOn == true)
        {
            Key_Space.color = Color.white * 0.8f;
        }

    }

    public void revertColor(int keyCode)
    {
        if (keyCode == (int)KeyCode.W)
        {
            Key_Up.color = Color.white;
        }
        else if (keyCode == (int)KeyCode.S)
        {
            Key_Down.color = Color.white;
        }
        else if (keyCode == (int)KeyCode.A)
        {
            Key_Left.color = Color.white;
        }
        else if (keyCode == (int)KeyCode.D)
        {
            Key_Right.color = Color.white;
        }
        else if (keyCode == (int)KeyCode.Space && isInteractiveOn == true)
        {
            Key_Space.color = Color.white;
        }
        
    }

    public override void InteractButton_On()
    {
        if (isInteractiveOn == false)
        {
            Key_Space.color = Color.white;
            isInteractiveOn = true;
        }
        
    }
    public override void InteractButton_Off()
    {
        if (isInteractiveOn == true)
        {
            Key_Space.color = Color.white * 0.8f;
            isInteractiveOn = false;
        }   
    }
}

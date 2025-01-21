using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class KeyBoardView : MonoBehaviour
{
    public Image Key_Left;
    public Image Key_Down;
    public Image Key_Up;
    public Image Key_Right;
    public Image Key_Space;

    public bool isSpacebarOn;

    void Start()
    {
        isSpacebarOn = true;
        Deactive_KeySpace(); // isSpacebarOn가 true이면 실행됨
    }

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
        else if (keyCode == (int)KeyCode.Space && isSpacebarOn == true)
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
        else if (keyCode == (int)KeyCode.Space && isSpacebarOn == true)
        {
            Key_Space.color = Color.white;
        }
        
    }

    public void Active_KeySpace()
    {
        if (isSpacebarOn == false)
        {
            Key_Space.color = Color.white;
            isSpacebarOn = true;
        }
        
    }
    public void Deactive_KeySpace()
    {
        if (isSpacebarOn == true)
        {
            Key_Space.color = Color.white * 0.8f;
            isSpacebarOn = false;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

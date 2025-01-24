using UnityEngine;

interface interactiveFuc
{
    public abstract void Active_KeySpace();
    public abstract void Deactive_InteractiveButton();
}

public class Input_InterfaceView : MonoBehaviour, interactiveFuc
{
    public bool isInteractiveOn;

    protected virtual void Awake()
    {
        isInteractiveOn = true;
        //Deactive_InteractiveButton(); // isInteractiveOn가 true이면 실행됨
    }

    void interactiveFuc.Active_KeySpace()
    {
        throw new System.NotImplementedException();
    }

    void interactiveFuc.Deactive_InteractiveButton()
    {
        throw new System.NotImplementedException();
    }
}

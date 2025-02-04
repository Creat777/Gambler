using UnityEngine;


public abstract class InterfaceView : MonoBehaviour
{
    // 스크립트 편집
    public bool isInteractiveOn;

    // 에디터에서 연결

    protected virtual void Awake()
    {
        isInteractiveOn = true;
        InteractButton_Off(); // isInteractiveOn가 true이면 최초 1회 실행됨
    }

    public abstract void InteractButton_On();

    public abstract void InteractButton_Off();
}

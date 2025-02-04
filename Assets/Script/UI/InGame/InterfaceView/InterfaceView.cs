using UnityEngine;


public abstract class InterfaceView : MonoBehaviour
{
    // ��ũ��Ʈ ����
    public bool isInteractiveOn;

    // �����Ϳ��� ����

    protected virtual void Awake()
    {
        isInteractiveOn = true;
        InteractButton_Off(); // isInteractiveOn�� true�̸� ���� 1ȸ �����
    }

    public abstract void InteractButton_On();

    public abstract void InteractButton_Off();
}

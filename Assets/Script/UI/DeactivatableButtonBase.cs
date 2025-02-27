using UnityEngine;

public abstract class Deactivatable_Button_Base : MonoBehaviour
{
    public GameObject buttonLock;

    public virtual void Deactivate_Button()
    {
        buttonLock.SetActive(true);
    }

    public virtual void Activate_Button()
    {
        buttonLock.SetActive(false);
    }
}

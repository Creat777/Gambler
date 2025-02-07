using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    CallBackManager callBackManager;
    void Start()
    {
        if(CallBackManager.Instance != null)
        {
            callBackManager = CallBackManager.Instance;
        }
    }

    public void Interaction()
    {
        callBackManager.TextWindowPopUp_Open();
    }
}

using PublicSet;
using UnityEngine;

public class Canvas_InGame : MonoBehaviour
{
    public GameObject interfaceView;
    public GameObject CasinoView;
    public GameObject textWindowView;
    public GameObject PopUPView;
    

    void Start()
    {
        PopUpViewDisable();
        CloseAllOfView();
        interfaceView.SetActive(true);
    }

    public void CloseAllOfView()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void PopUpViewDisable()
    {
        foreach (Transform child in PopUPView.transform)
        {
            if(child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
        }
        PopUPView.SetActive(false);
    }

    public void CasinoViewOpen()
    {
        CloseAllOfView();

        CasinoView.SetActive(true);

        
    }

    public void CasinoViewClose()
    {
        float delay = 2.0f;
        StartCoroutine(CallbackManager.Instance.BlackViewProcess(
            delay,
            ()=>
            {
                CloseAllOfView();
                interfaceView.SetActive(true);
            }

            ));
        
    }

    
}

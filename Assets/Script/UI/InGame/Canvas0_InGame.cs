using PublicSet;
using UnityEngine;

public class Canvas0_InGame : MonoBehaviour
{
    public GameObject interfaceView;
    public CasinoView casinoView;
    public CardGameView cardGameView;
    public TextWindowView textWindowView;



    void Start()
    {
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


    public void CasinoViewOpen()
    {
        CloseAllOfView();
        GameManager.Instance.ChangeCardGameView(true);
        casinoView.gameObject.SetActive(true);
    }

    public void CasinoViewClose()
    {
        float delay = 2.0f;
        GameManager.Instance.ChangeCardGameView(false);
        CallbackManager.Instance.PlaySequnce_BlackViewProcess(
            delay,
            CloseAllOfView,
            () => interfaceView.SetActive(true)
            );
    }

    

}

using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PopUp : MonoBehaviour
{
    public GameObject content;

    public void PopUpViewClose()
    {
        //Debug.Log(transform.parent.gameObject.activeSelf);
        //Debug.Log(transform.parent.gameObject.activeInHierarchy);
        if (transform.parent.gameObject.activeSelf == true)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}

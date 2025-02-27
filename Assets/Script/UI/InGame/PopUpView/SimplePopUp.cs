using UnityEngine;
using UnityEngine.UI;

public class SimplePopUp : MonoBehaviour
{
    public Text mainDescription;

    public virtual void UpdateMainDescription(string script)
    {
        // '_'을 줄바꿈으로 치환
        string[] scriptSplit = script.Split('_');
        mainDescription.text = string.Join('\n', scriptSplit);
    }
}

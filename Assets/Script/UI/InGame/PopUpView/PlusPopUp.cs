using UnityEngine;
using UnityEngine.UI;

public class SimplePopUp : MonoBehaviour
{
    public Text mainDescription;

    public virtual void UpdateMainDescription(string script)
    {
        mainDescription.text = script;
    }
}

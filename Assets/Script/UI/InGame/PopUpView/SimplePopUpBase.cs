using UnityEngine;
using UnityEngine.UI;

public abstract class SimplePopUpBase : MonoBehaviour
{
    public Text mainDescription;

    public virtual void UpdateMainDescription(string script)
    {
        // '_'�� �ٹٲ����� ġȯ
        string[] scriptSplit = script.Split('_');
        mainDescription.text = string.Join('\n', scriptSplit);
    }
}

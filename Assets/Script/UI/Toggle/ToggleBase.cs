
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleBase : MonoBehaviour
{
    private Toggle _toggle;
    protected Toggle toggle
    {
        get 
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
            return _toggle; 
        }
    }

    public void SetInteractable(bool value)
    {
        toggle.interactable = value;
        toggle.isOn = false;
    }

    public void SetToggleCallback(UnityAction<bool> callback)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(callback);
    }

    public void AddToggleCallback(UnityAction<bool> callback)
    {
        toggle.onValueChanged.AddListener(callback);
    }
}

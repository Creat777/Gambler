using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    private Button _button;

    protected Button button
    {
        get
        {
            if (_button == null) _button = GetComponent<Button>(); 

            if(_button != null) return _button;
            else 
            { 
                Debug.LogAssertion($"{gameObject.name} 객체는 버튼컴포넌트를 갖고있지 않음"); 
                return null; 
            }
        }
    }



    /// <summary>
    /// 콜백함수는 오로지 하나만 넣을 수 있으며, 여러 개를 넣을 시 람다함수로 여러 함수를 묶어야함
    /// </summary>
    /// <param name="callback"></param>
    public void SetButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log("button == null");
        }
    }

    /// <summary>
    /// 기존 콜백함수에 추가하는 함수
    /// </summary>
    /// <param name="callback"></param>
    public void AddButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log("button == null");
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    protected Button button;


    protected virtual void Awake()
    {
        InitDefault();
    }

    /// <summary>
    /// 버튼객체의 기본 초기화
    /// </summary>
    protected virtual void InitDefault()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogAssertion($"{gameObject.name} 객체는 버튼컴포넌트를 갖고있지 않음");
            Destroy(gameObject);
            return;
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

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // 싱글톤 아니고 편하게 접근하기 위해서 사용
    [SerializeField] private SelectionView instance;
    public SelectionView Instance_ { get { return instance; } private set { } }

    // 에디터에서 연결
    [SerializeField] private Button selection1;
    [SerializeField] private Button selection2;
    public Button __selection1 {  get { return selection1; } set { selection1 = value; } }
    public Button __selection2 { get { return selection2; } set { selection2 = value; } }

    // 스크립트에서 편집


    private void OnDisable()
    {
        // 할일을 마치고 사라질때 연결된 콜백함수를 제거
        __selection1.onClick.RemoveAllListeners();
        __selection2.onClick.RemoveAllListeners();
    }

    void Start()
    {

    }

    // 매개변수로 받은 함수를 버튼 클릭 이벤트에 등록
    // Action : 기본대리자
    public void RegisterButtonClick_Selection1(UnityAction callback)
    {
        // delegate로 만들어진 Action(대리자)을 직접 전달할 수는 없고 람다식으로 만든 익명함수를 전달함
        __selection1.onClick.AddListener(callback);
        // (parameters) => { function_body }
        // ex) () => Debug.Log("Hello!");
    }

    public void RegisterButtonClick_Selection2(UnityAction callback)
    {
        __selection2.onClick.AddListener(callback);
    }
}

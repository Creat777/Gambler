using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // 에디터에서 연결
    [SerializeField] private Button[] selection;

    // 스크립트에서 편집


    void Start()
    {
        
    }

    // 매개변수로 받은 함수를 버튼 클릭 이벤트에 등록
    // Action : 기본대리자
    public void RegisterButtonClick_Selection(int i, string selectionScript, UnityAction callback)
    {
        for (int j = 0; j < selection.Length; j++)
        {
            selection[i].transform.GetChild(0).GetComponent<Text>().text = selectionScript;
            selection[i].onClick.RemoveAllListeners();
            selection[i].onClick.AddListener(callback);
        }
    }
}

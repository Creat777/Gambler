using DG.Tweening;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionView : MonoBehaviour
{
    // 에디터에서 연결
    [SerializeField] private Button[] selection;

    // 스크립트에서 편집
    Vector3 intervalOfselection;
    Vector3[] selectionPositions;

    private void Awake()
    {
        //선택지간의 간격 (y축)
        intervalOfselection = selection[0].transform.position - selection[1].transform.position;

        // 선택지가 2개 일 때의 기본 포지션
        selectionPositions = new Vector3[selection.Length - 1];
        for (int i = 0; i < selection.Length - 1; i++)
        {
            selectionPositions[i] = selection[i].transform.position;
        }
    }

    // 매개변수로 받은 함수를 버튼 클릭 이벤트에 등록
    // Action : 기본대리자
    public void RegisterButtonClick_Selection(int index, string selectionScript, UnityAction callback)
    {
        if (index >= selection.Length) return;

        selection[index].transform.GetChild(0).GetComponent<Text>().text = selectionScript;

        // 버그 방지를 위해 등록된 모든 콜백함수를 제거
        selection[index].onClick.RemoveAllListeners();

        // 콜백을 실행한 후에 셀렉션뷰가 닫히도록 함
        selection[index].onClick.AddListener(
            () =>
            {
                callback();
                gameObject.SetActive(false);
            }
            );

        // 3 번째 선택지가 존재시
        if (index == 2)
        {
            // 3번 선택지의 활성화, 3번 선택지의 위치가 기존 2번 자리인 것을 확실히 함
            selection[index].gameObject.SetActive(true);
            selection[index].transform.position = selectionPositions[index - 1];

            // 1번과 2번 선택지의 위치를 한칸씩 올림
            for (int i = 0; i < selection.Length-1; i++)
            {
                selection[i].transform.position = selectionPositions[i] + intervalOfselection;
            }
            
        }
    }

    // 선택지의 콜백함수 실행 후 셀렉션뷰는 비활성화됨
    private void OnDisable()
    {
        //선택지가 할일이 끝나면 모두 원위치
        selection[2].gameObject.SetActive(false);

        for (int i = 0; i < selection.Length - 1; i++)
        {
            selection[i].transform.position = selectionPositions[i];
        }
    }
}

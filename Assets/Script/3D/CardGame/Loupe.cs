using System.Collections;
using UnityEngine;

public class Loupe : MonoBehaviour
{
    // 에디터
    public RectTransform roupeBox;
    public RectTransform subScreenLoupe; // 서브스크린 UI (확대된 화면을 보여줌)
    public const float holdTime = 0.3f; // 1초 이상 눌렀을 때 동작

    //스크립트
    private Vector3 touchPosition;
    private bool isHolding = false;
    private float holdTimer = 0f;
    private Vector3 subScreenOriginPos;

    private void Start()
    {
        subScreenOriginPos = subScreenLoupe.transform.position;
    }

    void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

#if UNITY_EDITOR
    // PC(에디터)용 마우스 입력 처리
    void HandleMouseInput()
    {
        // 한번 클릭했으면 다음을 실행
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdTimer = 0f;
        }

        // 일정시간 클릭을 유지하면 클릭되는 곳을 계속해서 확대
        if (Input.GetMouseButton(0) && isHolding)
        {
            if (holdTimer >= holdTime)
            {
                touchPosition = Input.mousePosition;
                ActivateLoupe(touchPosition);
            }
            else
            {
                holdTimer += Time.deltaTime;
            }
        }


        // 화면에서 마우스 클릭을 취소하면 서브스크린을 canvas에서 제거
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            subScreenLoupe.transform.position = subScreenOriginPos;
        }
    }
#endif

    // 안드로이드 터치 입력 처리
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
                isHolding = true;
                holdTimer = 0f;
            }

            if (Input.touchCount > 0 && isHolding)
            {
                if (holdTimer >= holdTime)
                {
                    touchPosition = Input.mousePosition;
                    ActivateLoupe(touchPosition);
                }
                else
                {
                    holdTimer += Time.deltaTime;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isHolding = false;
                subScreenLoupe.transform.position = subScreenOriginPos;
            }
        }
    }

    void ActivateLoupe(Vector3 screenPos)
    {
        subScreenLoupe.transform.position = screenPos;

        // 돋보기 UI를 화면의 터치 위치로 이동
        Vector3 worldPos;
        if (TryGetWorldPosition(screenPos, out worldPos))
        {
            Vector3 targetPos = transform.position;
            targetPos.x = worldPos.x;
            targetPos.z = worldPos.z;

            // 확대 카메라 이동
            transform.position = targetPos;

            // 서브스크린 활성화
            subScreenLoupe.gameObject.SetActive(true);
        }

        // 위쪽, 오른쪽의 경우 돋보기 박스가 화면을 벗어날 수 있음
        switch(IsPivotInsideContainer(subScreenLoupe, roupeBox))
        {
            case 0: subScreenLoupe.pivot = Vector2.zero; break;
            case 1: subScreenLoupe.pivot = new Vector2(1,0); break; // 우측을 벗어난 경우
            case 2: subScreenLoupe.pivot = new Vector2(0, 1); break; // 상측을 벗어난 경우
            case 3: subScreenLoupe.pivot = new Vector2(1, 1); break; // 우측, 상측을 모두 벗어난 경우
        }

    }

    // targetRectTransform의 pivot이 containerRectTransform의 Rect 안에 있는지 확인
    int IsPivotInsideContainer(RectTransform targetTrans, RectTransform containerTrans)
    {
        // targetRectTransform의 pivot 위치를 세계 좌표로 변환
        Vector3 pivotWorldPosition = targetTrans.TransformPoint(targetTrans.pivot);

        // containerRectTransform의 Rect 영역을 확인
        Rect containerRect = containerTrans.rect;

        // containerRectTransform의 세계 좌표를 얻기 위해, RectTransform의 위치와 크기 계산
        Vector3 containerWorldPosition = containerTrans.position;

        //// 정해진 박스의 우측만 벗어난 경우
        //if (pivotWorldPosition.x > containerWorldPosition.x + containerRect.width / 2 && pivotWorldPosition.y <= containerWorldPosition.y + containerRect.height / 2)
        //{
        //    return 1;
        //}
        // 정해진 박스의 상측을 벗어난 경우
        if (pivotWorldPosition.y > containerWorldPosition.y + containerRect.height / 2)
        {
            return 2;
        }
        //// 우측 상측 전부 벗어난 경우
        //if (pivotWorldPosition.x > containerWorldPosition.x + containerRect.width / 2 && pivotWorldPosition.y > containerWorldPosition.y + containerRect.height / 2)
        //{
        //    return 3;
        //}
        // 돋보기로 화면을 보는데 문제가 없는 경우
        return 0;
    }

    bool TryGetWorldPosition(Vector3 screenPos, out Vector3 worldPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            worldPos = hit.point;
            return true;
        }
        worldPos = Vector3.zero;
        return false;
    }
}

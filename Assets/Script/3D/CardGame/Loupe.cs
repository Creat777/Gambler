using System.Collections;
using UnityEngine;

public class Loupe : MonoBehaviour
{
    // 에디터
    public GameObject subScreenLoupe; // 서브스크린 UI (확대된 화면을 보여줌)
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
            subScreenLoupe.SetActive(true);
        }

        
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

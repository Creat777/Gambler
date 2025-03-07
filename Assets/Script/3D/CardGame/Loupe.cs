using System.Collections;
using UnityEngine;

public class Loupe : MonoBehaviour
{
    // ������
    public GameObject subScreenLoupe; // ���꽺ũ�� UI (Ȯ��� ȭ���� ������)
    public const float holdTime = 0.3f; // 1�� �̻� ������ �� ����

    //��ũ��Ʈ
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

    // PC(������)�� ���콺 �Է� ó��
    void HandleMouseInput()
    {
        // �ѹ� Ŭ�������� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdTimer = 0f;
        }

        // �����ð� Ŭ���� �����ϸ� Ŭ���Ǵ� ���� ����ؼ� Ȯ��
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


        // ȭ�鿡�� ���콺 Ŭ���� ����ϸ� ���꽺ũ���� canvas���� ����
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            subScreenLoupe.transform.position = subScreenOriginPos;
        }
    }

    // �ȵ���̵� ��ġ �Է� ó��
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

        // ������ UI�� ȭ���� ��ġ ��ġ�� �̵�
        Vector3 worldPos;
        if (TryGetWorldPosition(screenPos, out worldPos))
        {
            Vector3 targetPos = transform.position;
            targetPos.x = worldPos.x;
            targetPos.z = worldPos.z;

            // Ȯ�� ī�޶� �̵�
            transform.position = targetPos;

            // ���꽺ũ�� Ȱ��ȭ
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

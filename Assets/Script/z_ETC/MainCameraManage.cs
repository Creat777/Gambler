using UnityEngine;

public class MainCameraManage : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;

    public Vector3 Position3d_ZoomOut {  get; private set; }
    public Vector3 Position3d_ZoomIn { get; private set; }
    private Quaternion Rotation3d;

    bool isExecute = false;

    private void Start()
    {
        Position3d_ZoomOut = transform.position;
        Position3d_ZoomIn = Position3d_ZoomOut + new Vector3(0f,-10f, 0f);

        Rotation3d = transform.rotation;

        mainCamera.orthographic = false;
        GameManager.Instance.ChangeCardGameView(false);
    }

    private void FixedUpdate()
    {
        // 2d ���϶� ��� �÷��̾ ����
        if (GameManager.Instance.isCasinoGameView == false)
        {
            Trace2DPlayer();
        }

        // 3d�䰡 �ƴ� �� �ѹ��� ��ȭ
        else if(isExecute == true)
        {
            Change3dCamera();
        }
    }

    private void Trace2DPlayer()
    {
        // 3d���� 2d�� ȭ�� ��ȯ�� ���� 1���� ����
        if(isExecute == false)
        {
            isExecute = true;

            Debug.Log("�÷��̾� ���� ����");
            // ī�޶� �⺻���� ����
            mainCamera.orthographic = true;
            transform.rotation = Quaternion.identity;
        }

        // Debug.Log("�÷��̾� ������");
        // ī�޶� �÷��̾��� ��ġ�� ��� ����ٴ�
        Vector3 playerPos = player.transform.position;
        playerPos.z = -10;
        transform.position = playerPos;
    }

    private void Change3dCamera()
    {
        isExecute = false;

        // ī�޶��� �⺻������ 3d�� ���� �� ������ �Ʒ��� �������� �þ߸� ������ ��
        mainCamera.orthographic = false;
        transform.position = Position3d_ZoomOut;
        transform.rotation = Rotation3d;
    }
}

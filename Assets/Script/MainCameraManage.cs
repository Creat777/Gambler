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
        // 2d 뷰일때 계속 플레이어를 따라감
        if (GameManager.Instance.isCasinoGameView == false)
        {
            Trace2DPlayer();
        }

        // 3d뷰가 아닐 때 한번만 변화
        else if(isExecute == true)
        {
            Change3dCamera();
        }
    }

    private void Trace2DPlayer()
    {
        // 3d에서 2d로 화면 전환시 최초 1번만 실행
        if(isExecute == false)
        {
            isExecute = true;

            Debug.Log("플레이어 추적 시작");
            // 카메라 기본설정 변경
            mainCamera.orthographic = true;
            transform.rotation = Quaternion.identity;
        }

        // Debug.Log("플레이어 추적중");
        // 카메라가 플레이어의 위치를 계속 따라다님
        Vector3 playerPos = player.transform.position;
        playerPos.z = -10;
        transform.position = playerPos;
    }

    private void Change3dCamera()
    {
        isExecute = false;

        // 카메라의 기본설정을 3d로 변경 및 위에서 아래로 내려보는 시야를 갖도록 함
        mainCamera.orthographic = false;
        transform.position = Position3d_ZoomOut;
        transform.rotation = Rotation3d;
    }
}

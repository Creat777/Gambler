using DG.Tweening;
using UnityEngine;

public class MainCameraAnimation : MonoBehaviour
{
    private MainCameraManage cameraMng;
    float delay = 1.0f;

    private void Awake()
    {
        cameraMng = GetComponent<MainCameraManage>();
    }

    public void GetSequnce_CameraZoomIn(Sequence sequence)
    {
        sequence.Append(transform.DOMove(cameraMng.Position3d_ZoomIn, delay));
    }
    public void GetSequnce_CameraZoomOut(Sequence sequence)
    {
        sequence.Append(transform.DOMove(cameraMng.Position3d_ZoomOut, delay));
    }
}

using Unity.Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera fullCamera;
    private Transform fullTransform;

    [SerializeField] private CinemachineCamera zoomCamera;
    [SerializeField] private Transform zoomTransform;

    private bool shouldSnapFullCamera = false;
    private Vector3 lastFullPosition;

    private void Start()
    {
        fullTransform = fullCamera.transform;
        lastFullPosition = fullTransform.position + Vector3.one;

        CameraDatabase.Instance.OnActiveCameraChange.AddListener(SnapFullCamera);

        zoomTransform.position = fullTransform.position;
        zoomCamera.transform.position = fullTransform.position;
    }

    private void Update()
    {
        Vector3 fullPosition = fullTransform.position;
        if (fullPosition == lastFullPosition)
            shouldSnapFullCamera = false;

        Vector3 zoomPosition = shouldSnapFullCamera ? fullPosition : zoomTransform.position;
        Vector3 zoomLocal = zoomPosition - fullPosition;

        zoomLocal.y = Mathf.Min(zoomLocal.y, 0f);

        float frustumHeightHalf = -zoomLocal.y * Mathf.Tan(fullCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidthHalf = frustumHeightHalf * fullCamera.aspect;

        zoomLocal.x = Mathf.Clamp(zoomLocal.x, -frustumWidthHalf, frustumWidthHalf);
        zoomLocal.z = Mathf.Clamp(zoomLocal.z, -frustumHeightHalf, frustumHeightHalf);

        zoomTransform.position = fullPosition + zoomLocal;

        zoomTransform.rotation = fullTransform.rotation;
        zoomCamera.Lens.FieldOfView = fullCamera.fieldOfView;

        lastFullPosition = fullPosition;
    }

    private void SnapFullCamera()
    {
        shouldSnapFullCamera = true;
    }
}

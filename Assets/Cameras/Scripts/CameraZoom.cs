using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class CameraZoom : MonoBehaviour
{
    public static CameraZoom Instance { get; private set; }
    private CameraZoom()
    {
        Instance = this;
    }

    [SerializeField] private Camera fullCamera;
    private Transform fullTransform;

    [SerializeField] private CinemachineCamera zoomCamera;
    [SerializeField] private Transform zoomTransform;

    [SerializeField] private float minY;

    private const int shouldSnapFullCameraReset = 10;
    private int shouldSnapFullCamera = shouldSnapFullCameraReset;
    private Vector3 lastFullPosition;

    [SerializeField] private InputActionReference zoomActionReference;
    [SerializeField] private InputActionReference panActionReference;
    private float? zoomInput = null;
    private Vector2? panInput = null;
    private bool allowInput = true;

    private void Awake()
    {
        StateTrack.Instance.OnBeginLoadState.AddListener(Reload);
        StateTrack.Instance.OnBeginRollback.AddListener(Reload);
    }

    private void Reload()
    {
        allowInput = true;
    }

    private void Start()
    {
        fullTransform = fullCamera.transform;
        lastFullPosition = fullTransform.position + Vector3.one;

        CameraDatabase.Instance.OnActiveCameraChange.AddListener(SnapFullCamera);

        zoomTransform.position = fullTransform.position;
        zoomCamera.transform.position = fullTransform.position;

        zoomActionReference.action.Enable();

        panActionReference.action.Enable();
    }

    public void ZoomToCamera(string cameraName)
    {
        zoomTransform.position = CameraDatabase.Instance.Cameras[cameraName].transform.position;
    }

    public void ToggleInput(bool allowed)
    {
        allowInput = allowed;
    }

    private void Update()
    {
        if (allowInput)
        {
            if (zoomActionReference.action.inProgress)
                zoomInput = zoomActionReference.action.ReadValue<Vector2>().y;


            if (panActionReference.action.inProgress)
                panInput = panActionReference.action.ReadValue<Vector2>();
        }

        Vector3 fullPosition = fullTransform.position;
        if (shouldSnapFullCamera > 0)
        {
            if (fullPosition == lastFullPosition)
                shouldSnapFullCamera--;
        }

        Vector3 zoomPosition;
        if (shouldSnapFullCamera > 0)
            zoomPosition = fullPosition;
        else
        {
            zoomPosition = zoomTransform.position;

            if (zoomInput.HasValue && !EventSystem.current.IsPointerOverGameObject())
            {
                zoomPosition.y += zoomInput.Value * (zoomPosition.y * 0.2f);
                zoomInput = null;
            }

            zoomPosition.y = Mathf.Max(minY, zoomPosition.y);

            if (panInput.HasValue)
            {
                Vector2 sourceInput = panInput.Value * zoomPosition.y;
                zoomPosition.x += sourceInput.x;
                zoomPosition.z += sourceInput.y;
                panInput = null;
            }
        }
        Vector3 zoomLocal = zoomPosition - fullPosition;


        zoomLocal.y = Mathf.Min(zoomLocal.y, 0f);

        float frustumHeightHalf = -zoomLocal.y * Mathf.Tan(fullCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidthHalf = frustumHeightHalf * fullCamera.aspect;

        zoomLocal.x = Mathf.Clamp(zoomLocal.x, -frustumWidthHalf, frustumWidthHalf);
        zoomLocal.z = Mathf.Clamp(zoomLocal.z, -frustumHeightHalf, frustumHeightHalf);


        zoomTransform.SetPositionAndRotation(fullPosition + zoomLocal, fullTransform.rotation);
        zoomCamera.Lens.FieldOfView = fullCamera.fieldOfView;

        lastFullPosition = fullPosition;
    }

    private void SnapFullCamera(CameraDatabase.ChangeMode mode)
    {
        if (mode == CameraDatabase.ChangeMode.Move)
            shouldSnapFullCamera = shouldSnapFullCameraReset;
    }
}

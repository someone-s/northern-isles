using UnityEngine;

public class CanvasSwap : MonoBehaviour
{
    private Vector3 worldPosition;
    [SerializeField] private float margin = 0.1f;
    [SerializeField] private GameObject staticObject;
    [SerializeField] private GameObject dynamicObject;
    [SerializeField] private bool beginOnStart = false;
    private bool priorState = false;
    private Camera mainCamera;

    private void Start()
    {
        if (gameObject.isStatic)
            worldPosition = transform.position;

        mainCamera = Camera.main;

        End();
        if (beginOnStart)
            Begin();
    }

    public void Begin()
    {
        enabled = true;
    }

    public void End()
    {
        staticObject.SetActive(true);
        dynamicObject.SetActive(false);
        priorState = false;
        enabled = false;
    }

    private void Update()
    {
        if (!gameObject.isStatic)
            worldPosition = transform.position;

        var viewPosition = mainCamera.WorldToViewportPoint(worldPosition);

        bool currentState = viewPosition.z > 0f
        && viewPosition.x >= -margin && viewPosition.x <= 1f + margin
        && viewPosition.y >= -margin && viewPosition.y <= 1f + margin;

        if (currentState != priorState)
        {
            staticObject.SetActive(!currentState);
            dynamicObject.SetActive(currentState);
        }

        priorState = currentState;
    }
}

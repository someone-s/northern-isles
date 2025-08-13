using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraPriority : MonoBehaviour
{
    private CinemachineCamera cinemachine;
    [SerializeField] private bool startPrioritized;

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineCamera>();
        cinemachine.Priority.Enabled = true;
        cinemachine.Priority.Value = startPrioritized ? 1 : 0;
    }

    public void Prioritize()
    {
        cinemachine.Priority.Value = 1;
    }

    public void Deprioritize()
    {
        cinemachine.Priority.Value = 0;
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera fromCamera;
    [SerializeField] private CinemachineCamera toCamera;

    public void Transition()
    {
        fromCamera.Priority = 0;
        toCamera.Priority = 1;
    }
}

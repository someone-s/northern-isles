using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class CameraDatabase : MonoBehaviour
{
    public static CameraDatabase Instance { get; private set; }

    private Dictionary<string, CinemachineCamera> cameras;
    [SerializeField] private string defaultCamera;
    private string activeCamera;

    public UnityEvent<ChangeMode> OnActiveCameraChange;

    public enum ChangeMode
    {
        Move,
        Expand
    }

    private CameraDatabase()
    {
        Instance = this;
    }

    private void Awake()
    {
        cameras = new();
        foreach (var camera in GetComponentsInChildren<CinemachineCamera>())
        {
            cameras.Add(camera.Name, camera);
            camera.Priority.Enabled = true;
            camera.Priority.Value = 0;
        }

        activeCamera = defaultCamera;
        Assert.IsTrue(cameras.ContainsKey(activeCamera));
        cameras[activeCamera].Priority.Value = 1;

        StateTrack.Instance.OnBeginLoadState.AddListener(Reload);
        StateTrack.Instance.OnBeginRollback.AddListener(Reload);
    }

    private void Reload()
    {
        Switch(defaultCamera, ChangeMode.Move);
    }

    [Button]
    public void Switch(string name, ChangeMode mode)
    {
        if (cameras.TryGetValue(name, out CinemachineCamera newActive))
        {
            cameras[activeCamera].Priority.Value = 0;
            newActive.Priority.Value = 1;
            activeCamera = name;

            OnActiveCameraChange.Invoke(mode);
        }
    }

    public CinemachineCamera GetActiveCamera()
    {
        if (cameras.TryGetValue(name, out CinemachineCamera active))
            return active;
        else
            return null;
    }
}

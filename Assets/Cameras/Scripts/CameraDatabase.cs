using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDatabase : MonoBehaviour
{   
    public static CameraDatabase Instance { get; private set; }

    private Dictionary<string, CinemachineCamera> cameras;
    [SerializeField] private string activeCamera;

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

        Assert.IsTrue(cameras.ContainsKey(activeCamera));
        cameras[activeCamera].Priority.Value = 1;
    }

    [Button]
    public void Switch(string name)
    {
        if (cameras.TryGetValue(name, out CinemachineCamera newActive))
        {
            cameras[activeCamera].Priority.Value = 0;
            newActive.Priority.Value = 1;
            activeCamera = name;
        }
    }
}

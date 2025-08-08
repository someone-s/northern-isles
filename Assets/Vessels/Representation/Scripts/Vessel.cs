using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    public VesselStorage Storage { get; private set; }
    public VesselNavigation Navigation { get; private set; }
    public VesselClick Click { get; private set; }

    public Guid Guid { get; private set; }

    [field: SerializeField]
    public string Type { get; private set; }

    private Vector3 cachedPosition;
    private Quaternion cachedRotation;

    private void Awake()
    {
        Storage = gameObject.GetComponentInChildren<VesselStorage>();
        Navigation = gameObject.GetComponentInChildren<VesselNavigation>();
        Click = gameObject.GetComponentInChildren<VesselClick>();
    }
    
    public void SetGuid(Guid guid)
    {
        Guid = guid;
    }

    public void SetOrientation(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        Navigation.Agent.Warp(position);
        cachedPosition = position;
        cachedRotation = rotation;
    }

    public JToken GetState()
    {
        transform.GetPositionAndRotation(out cachedPosition, out cachedRotation);

        return JToken.FromObject(new VesselState()
        {
            position = cachedPosition,
            rotation = cachedRotation,
            navigation = Navigation.GetState(),
            storage = Storage.GetState()
        });
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<VesselState>();
        cachedPosition = state.position;
        cachedRotation = state.rotation;
        transform.SetPositionAndRotation(cachedPosition, cachedRotation);
        Navigation.Agent.Warp(cachedPosition);
        Navigation.SetState(state.navigation);
        Storage.SetState(state.storage);
    }

    public void Rollback()
    {
        transform.SetPositionAndRotation(cachedPosition, cachedRotation);
        Navigation.Agent.Warp(cachedPosition);
        Navigation.Rollback();
        Storage.Rollback();
    }

    private struct VesselState
    {
        public Vector3 position;
        public Quaternion rotation;
        public JToken navigation;
        public JToken storage;
    }
}

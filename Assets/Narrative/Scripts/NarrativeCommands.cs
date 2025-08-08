using System;
using UnityEngine;
using Yarn.Unity;

public class NarrativeCommands
{
    private static void SetVariable(string variableName, string variableValue)
    {
        var variable = UnityEngine.Object.FindFirstObjectByType<DialogueRunner>().VariableStorage;

        variable.SetValue('$' + variableName, variableValue);
    }

    private static void SetVariable(string variableName, bool variableValue)
    {
        var variable = UnityEngine.Object.FindFirstObjectByType<DialogueRunner>().VariableStorage;

        variable.SetValue('$' + variableName, variableValue);
    }

    [YarnCommand("spawn_building")]
    public static void SpawnBuilding(string outVariableName, string portName, string type, int position)
    {
        Building building = BuildingDatabase.Instance.Spawn(
            Enum.Parse<BuildingType>(type),
            PortDatabase.Instance.Lookups[portName],
            position);

        SetVariable(outVariableName, building.Guid.ToString());
    }

    [YarnCommand("spawn_vessel")]
    public static void SpawnVessel(string outVariableName, string type, string locationName)
    {
        Transform orientation = NarrativeOrientationDatabase.Instance.Lookups[locationName];
        Vessel vessel = VesselDatabase.Instance.Spawn(type, orientation);

        SetVariable(outVariableName, vessel.Guid.ToString());
    }

    [YarnCommand("allow_vessel_click")]
    public static void AllowVesselClick(string vesselGuid)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances[Guid.Parse(vesselGuid)];
        vessel.Click.enabled = true;
    }

    [YarnCommand("prevent_vessel_click")]
    public static void PreventVesselClick(string vesselGuid)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances[Guid.Parse(vesselGuid)];
        vessel.Click.enabled = false;
    }

    [YarnCommand("prevent_port_click")]
    public static void PreventPortClick()
    {
        RouteDisplay.Instance.PortSelectOverride = _ => false;
    }

    [YarnCommand("restrict_port_click")]
    public static void RestrictPortClick(string portName)
    {
        Port target = PortDatabase.Instance.Lookups[portName];

        RouteDisplay.Instance.PortSelectOverride = port => port == target;
    }

    [YarnCommand("free_port_click")]
    public static void FreePortClick()
    {
        RouteDisplay.Instance.PortSelectOverride = null;
    }

    [YarnCommand("show_port")]
    public static void ShowPort(string portName)
    {
        Port port = PortDatabase.Instance.Lookups[portName];

        port.Visual.enabled = true;
    }

    [YarnCommand("on_vessel_selected_set")]
    public static void OnVesselSelectedSet(string vesselGuid, string variableName)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances[Guid.Parse(vesselGuid)];

        void listener(Vessel selectedVessel)
        {
            if (selectedVessel == vessel)
                SetVariable(variableName, true);

            RouteDisplay.Instance.OnVesselSelected.RemoveListener(listener);
        }

        RouteDisplay.Instance.OnVesselSelected.AddListener(listener);
    }

    [YarnCommand("on_port_selected_set")]
    public static void OnPortSelectedSet(string portName, string variableName)
    {
        Port port = PortDatabase.Instance.Lookups[portName];

        void listener(RouteData data)
        {
            if (data.Port == port)
                SetVariable(variableName, true);

            RouteDisplay.Instance.OnInstructionAdded.RemoveListener(listener);
        }

        RouteDisplay.Instance.OnInstructionAdded.AddListener(listener);
    }

    [YarnCommand("move_to_camera")]
    public static void MoveToCamera(string cameraName)
    {
        CameraDatabase.Instance.Switch(cameraName);
    }

}

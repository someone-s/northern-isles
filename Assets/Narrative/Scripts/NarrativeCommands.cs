using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NarrativeCommands
{
    private static void SetVariable(string variableName, float variableValue)
    {
        var variable = UnityEngine.Object.FindFirstObjectByType<DialogueRunner>().VariableStorage;

        variable.SetValue('$' + variableName, variableValue);
    }

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
    public static void SpawnVessel(string outVariableName, string type, string vesselName, string locationName)
    {
        Transform orientation = NarrativeOrientationDatabase.Instance.Lookups[locationName];
        Vessel vessel = VesselDatabase.Instance.Spawn(type, vesselName, orientation);

        SetVariable(outVariableName, vessel.Guid.ToString());
    }

    [YarnCommand("find_vessel")]
    public static void FindVessel(string outVariableName, string vesselName)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances.Select(pair => pair.Value).FirstOrDefault(vessel => vessel.name == vesselName);
        Assert.IsTrue(vessel != null);
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
            {
                SetVariable(variableName, true);
                RouteDisplay.Instance.OnVesselSelected.RemoveListener(listener);
            }
        }

        RouteDisplay.Instance.OnVesselSelected.AddListener(listener);
    }

    [YarnCommand("on_port_selected_set")]
    public static void OnPortSelectedSet(string portName, string variableName)
    {
        Port port = PortDatabase.Instance.Lookups[portName];

        void listener(RoutePort data)
        {
            if (data.Port == port)
            {
                SetVariable(variableName, true);
                RouteDisplay.Instance.OnPortAdded.RemoveListener(listener);
            }
        }

        RouteDisplay.Instance.OnPortAdded.AddListener(listener);
    }

    [YarnCommand("move_to_camera")]
    public static void MoveToCamera(string cameraName)
    {
        CameraDatabase.Instance.Switch(cameraName, CameraDatabase.ChangeMode.Move);
    }

    [YarnCommand("expand_to_camera")]
    public static void ExpandToCamera(string cameraName)
    {
        CameraDatabase.Instance.Switch(cameraName, CameraDatabase.ChangeMode.Expand);
    }

    [YarnCommand("zoom_to_camera")]
    public static void ZoomToCamera(string cameraName)
    {
        CameraZoom.Instance.ZoomToCamera(cameraName);
    }


    [YarnCommand("toggle_zoom_input")]
    public static void ToggleZoomInput(bool allowed)
    {
        CameraZoom.Instance.ToggleInput(allowed);
    }

    [YarnCommand("on_port_from_to_set")]
    public static void OnPortsFromToSet(string fromPortName, string toPortName, string variableName)
    {
        Port fromPort = PortDatabase.Instance.Lookups[fromPortName];
        Port toPort = PortDatabase.Instance.Lookups[toPortName];

        void listener(RoutePort data)
        {
            if ((data.Last().Port == fromPort && data.Port == toPort)
            || (data.Port == fromPort && data.Next().Port == toPort))
            {
                SetVariable(variableName, true);
                RouteDisplay.Instance.OnPortAdded.RemoveListener(listener);
                RouteDisplay.Instance.OnPortMoved.RemoveListener(listener);
                RouteDisplay.Instance.OnPortDeleted.RemoveListener(listener);
            }
        }

        RouteDisplay.Instance.OnPortAdded.AddListener(listener);
        RouteDisplay.Instance.OnPortMoved.AddListener(listener);
        RouteDisplay.Instance.OnPortDeleted.AddListener(listener);
    }

    [YarnCommand("on_cargo_delete_set")]
    public static void OnCargoDeleteSet(string variableName)
    {
        void listener()
        {
            SetVariable(variableName, true);
            RouteDisplay.Instance.OnCargoDeleted.RemoveListener(listener);
        }

        RouteDisplay.Instance.OnCargoDeleted.AddListener(listener);
    }

    [YarnCommand("on_cargo_dissatisfied_set")]
    public static void OnCargoDissatisfiedSet(string vesselGuid, string variableName)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances[Guid.Parse(vesselGuid)];

        void listener()
        {
            SetVariable(variableName, true);
            vessel.Storage.OnDissatisfiedCargo.RemoveListener(listener);
        }

        vessel.Storage.OnDissatisfiedCargo.AddListener(listener);
    }

    [YarnCommand("clear_cargo_dissatisfied_listeners")]
    public static void ClearCargoDissatisfiedListeners(string vesselGuid)
    {
        Vessel vessel = VesselDatabase.Instance.VesselInstances[Guid.Parse(vesselGuid)];


        vessel.Storage.OnDissatisfiedCargo.RemoveAllListeners();
    }

    [YarnCommand("set_auto_advance_dialogue")]
    public static void SetAutoAdvanceDialogue(bool autoAdvance)
    {
        LinePresenter[] presenters = UnityEngine.Object.FindObjectsByType<LinePresenter>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var presenter in presenters)
        {
            presenter.autoAdvance = autoAdvance;
        }
    }

    [YarnCommand("create_game_stop_watch")]
    public static void CreateGameStopWatch(string variableName)
    {
        SetVariable(variableName, GameStopWatchDatabase.Instance.CreateNewStopWatch().ToString());
    }

    [YarnCommand("sample_game_stop_watch")]
    public static void SampleGameStopWatch(string stopWatchGuid, string variableName)
    {
        float elapsedS = GameStopWatchDatabase.Instance.StopWatches[Guid.Parse(stopWatchGuid)].ElapsedS;

        SetVariable(variableName, elapsedS);
    }

    [YarnCommand("reset_game_stop_watch")]
    public static void ResetGameStopWatch(string stopWatchGuid)
    {
        GameStopWatchDatabase.Instance.StopWatches[Guid.Parse(stopWatchGuid)].Reset();
    }

    [YarnCommand("link_game_stop_watch_to_display")]
    public static void LinkGameStopWatchToDisplay(string stopWatchGuid, float maxValue)
    {
        GameStopWatch stopWatch = GameStopWatchDatabase.Instance.StopWatches[Guid.Parse(stopWatchGuid)];
        stopWatch.OnElapseChange.AddListener(time => GameStopWatchVisual.Instance.SetProgress(Mathf.Clamp(time / maxValue, 0f, 1f)));
        GameStopWatchVisual.Instance.SetProgress(stopWatch.ElapsedS);
    }

    [YarnCommand("unlink_game_stop_watches_to_display")]
    public static void UnlinkGameStopWatchesToDisplay(string stopWatchGuid)
    {
        GameStopWatchDatabase.Instance.StopWatches[Guid.Parse(stopWatchGuid)].OnElapseChange.RemoveAllListeners();
    }

    [YarnCommand("delete_game_stop_watch")]
    public static void DeleteGameStopWatch(string stopWatchGuid)
    {
        GameStopWatchDatabase.Instance.DeleteStopWatch(Guid.Parse(stopWatchGuid));
    }

    [YarnCommand("show_game_stop_watch_display")]
    public static void ShowGameStopWatchDisplay()
    {
        GameStopWatchVisual.Instance.gameObject.SetActive(true);
    }

    [YarnCommand("hide_game_stop_watch_display")]
    public static void HideGameStopWatchDisplay()
    {
        GameStopWatchVisual.Instance.gameObject.SetActive(false);
    }

    [YarnCommand("show_region_button")]
    public static void ShowRegionButton()
    {
        RegionVisual.Instance.ShowEnterButton();
    }

    [YarnCommand("on_region_shown_set")]
    public static void OnRegionShownSet(string variableName)
    {
        void listener()
        {
            SetVariable(variableName, true);
            RouteDisplay.Instance.OnCargoDeleted.RemoveListener(listener);
        }

        RegionVisual.Instance.OnRegionShown.AddListener(listener);
    }
}

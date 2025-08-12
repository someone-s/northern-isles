using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoutePort : MonoBehaviour
{

    [SerializeField] private TMP_Text textArea;

    public Port Port { get; private set; }
    public RouteDisplay Display { get; private set; }
    public string Name { get; private set; }


    public void SetDisplay(RouteDisplay display)
    {
        Display = display;
    }

    public void SetPort(Port port)
    {
        Port = port;

        var portName = Port.Visual.Name;
        textArea.text = portName;
        Name = portName;
    }

    public void MovePort(int oldIndex, int newIndex)
    {
        Display.MovePort(this, oldIndex, newIndex);

    }

    public void DeletePort()
    {
        bool success = Display.DeletePort(this, transform.GetSiblingIndex(), Port);
        if (!success)
            return;

        var deletablePanel = GetComponent<DeletablePanel>();
        if (deletablePanel != null)
            deletablePanel.Delete();

    }

}

public static class RoutePortExtension
{
    public static RoutePort Last(this RoutePort routePort)
    {
        int nextIndex = routePort.transform.GetSiblingIndex() - 1;
        if (nextIndex < 0) nextIndex = routePort.transform.parent.childCount - 1;

        return routePort.transform.parent.GetChild(nextIndex).GetComponent<RoutePort>();
    }

    public static RoutePort Next(this RoutePort routePort)
    {
        int nextIndex = routePort.transform.GetSiblingIndex() + 1;
        if (nextIndex >= routePort.transform.parent.childCount) nextIndex = 0;

        return routePort.transform.parent.GetChild(nextIndex).GetComponent<RoutePort>();
    }
}

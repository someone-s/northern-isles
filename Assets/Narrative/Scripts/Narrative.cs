using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using Yarn.Unity.Attributes;

public class Narrative : MonoBehaviour
{
    [SerializeField] private DialogueRunner runner;
    [SerializeField] private YarnProject project;
    [YarnNode(nameof(project)), SerializeField] private string firstNode;

    private Dictionary<string, List<NarrativeNodeEvents>> listeners;

    private Narrative()
    {
        listeners = new();
    }

    public void AddListener(string nodeName, NarrativeNodeEvents listener)
    {
        List<NarrativeNodeEvents> nodeListeners;
        if (!listeners.TryGetValue(nodeName, out nodeListeners))
        {
            nodeListeners = new();
            listeners.Add(nodeName, nodeListeners);
        }
        nodeListeners.Add(listener);
    }

    private void Start()
    {
        StateTrack.Instance.SaveState();

        runner.onNodeStart.AddListener(OnAnyNodeStart);
        runner.onNodeComplete.AddListener(OnAnyNodeEnd);

        runner.SetProject(project);
        runner.StartDialogue(firstNode);
    }


    private void OnAnyNodeStart(string nodeName)
    {
        if (listeners.TryGetValue(nodeName, out List<NarrativeNodeEvents> nodeListeners))
        {
            foreach (var listener in nodeListeners)
                listener.OnAnyNodeStart();
        }
    }

    private void OnAnyNodeEnd(string nodeName)
    {
        if (listeners.TryGetValue(nodeName, out List<NarrativeNodeEvents> nodeListeners))
        {
            foreach (var listener in nodeListeners)
                listener.OnAnyNodeEnd();
        }
    }
}
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class NarrativeNodeEvents : MonoBehaviour
{


    public UnityEvent OnNodeStart;
    public UnityEvent OnNodeEnd;

    private void Start()
    {
        var runner = FindFirstObjectByType<DialogueRunner>();
        runner.onNodeStart.AddListener(OnAnyNodeStart);
        runner.onNodeComplete.AddListener(OnAnyNodeEnd);
    }

    private void OnAnyNodeStart(string nodeName)
    {
        if (nodeName == gameObject.name)
            OnNodeStart.Invoke();
    }

    private void OnAnyNodeEnd(string nodeName)
    {
        if (nodeName == gameObject.name)
            OnNodeEnd.Invoke();
    }
}

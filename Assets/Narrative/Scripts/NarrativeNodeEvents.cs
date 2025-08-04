using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class NarrativeNodeEvents : MonoBehaviour
{


    public UnityEvent OnNodeStart;
    public UnityEvent OnNodeEnd;

    private void Awake()
    {
        var narrative = gameObject.GetComponentInParent<Narrative>();
        narrative.AddListener(gameObject.name, this);
    }

    public void OnAnyNodeStart()
    {
            OnNodeStart.Invoke();
    }

    public void OnAnyNodeEnd()
    {
            OnNodeEnd.Invoke();
    }
}

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NarrativeNode))]
public class ActionOrdered : MonoBehaviour
{
    private NarrativeNode node;


    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }


    public void OnCreatedInstruction(RouteData instructionData)
    {
        instructionData.OnActionMoved.AddListener(ActionMoved);
    }

    private void ActionMoved(List<IVesselAction> actions)
    {
        if (!gameObject.activeSelf)
            return;

        bool pass = true;

        bool firstLoadSpotted = false;
        foreach (var action in actions)
        {
            if (action is VesselUnloadAction)
            {
                // unload appear after load
                if (firstLoadSpotted)
                {
                    pass = false;
                    break;
                }

            }
            else if (action is VesselLoadAction)
                firstLoadSpotted = true;
        }

        if (pass)
            node.MarkComplete();
    }
}

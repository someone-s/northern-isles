using System.Collections.Generic;
using UnityEngine;

public class ActionOrdered : MonoBehaviour
{
    private NarrativeNode node;


    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }


    public void OnCreatedInstruction(VesselInstruction instruction)
    {
        instruction.OnOrderAction.AddListener(OnOrderAction);
    }

    private void OnOrderAction(VesselInstruction instruction)
    {
        if (!gameObject.activeSelf)
            return;

        bool pass = true;

        bool firstLoadSpotted = false;
        foreach (var action in instruction.actions)
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

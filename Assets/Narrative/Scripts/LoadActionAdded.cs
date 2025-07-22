using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NarrativeNode))]
public class LoadActionAdded : MonoBehaviour
{
    private NarrativeNode node;

    [Header("Type")]
    [SerializeField] private bool checkType = true;
    [SerializeField] private CargoType type;
    [Header("Amount")]
    [SerializeField] private bool checkAmount = true;
    [SerializeField] private float amount;
    [Header("Compartment")]
    [SerializeField] private bool checkCompartmentIndex = true;
    [SerializeField] private int compartmentIndex;


    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }

    public void OnCreatedInstruction(VesselInstruction instruction)
    {
        instruction.OnAddAction.AddListener(OnAddedAction);
    }

    private void OnAddedAction(IVesselAction action)
    {
        if (action is not VesselLoadAction)
            return;

        var loadAction = action as VesselLoadAction;
        loadAction.OnModifiedData.AddListener(EvaluateAction);

    }

    private void EvaluateAction(VesselLoadAction loadAction)
    {
        if (checkType)
            if (loadAction.Cargo != type)
                return;

        if (checkAmount)
            if (loadAction.Amount != amount)
                return;

        if (checkCompartmentIndex)
            if (loadAction.CompartmentIndex != compartmentIndex)
                return;

        
        node.MarkComplete();
    }
}

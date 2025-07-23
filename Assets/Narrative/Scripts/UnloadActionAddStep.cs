using System.Collections.Generic;
using UnityEngine;

public class UnloadActionAddStep : MonoBehaviour
{
    private NarrativeNode node;

    [Header("Type")]
    [SerializeField] private bool checkCargo = true;
    [SerializeField] private CargoType cargo;
    private bool cargoPassed = false;
    [Header("Amount")]
    [SerializeField] private bool checkQuantity = true;
    [SerializeField] private float quantity;
    private bool quantityPassed = false;
    [Header("Compartment")]
    [SerializeField] private bool checkCompartmentIndex = true;
    [SerializeField] private int compartmentIndex;
    private bool compartmentIndexPassed = false;

    private RouteData currentInstructionData = null;

    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }

    private void Start()
    {
        if (currentInstructionData != null)
            Setup();
    }

    public void OnCreatedInstruction(RouteData instructionData)
    {
        currentInstructionData = instructionData;

        if (gameObject.activeSelf)
            Setup();
    }

    private void Setup()
    {
        currentInstructionData.LoadAddOverride = AlwaysFalse;
        currentInstructionData.UnloadAddOverride = AlwaysTrue;
        currentInstructionData.OnUnloadAdded.AddListener(UnloadActionAdded);
    }

    private void UnFilter()
    {
        if (currentInstructionData == null)
            return;

        if (currentInstructionData.LoadAddOverride == AlwaysFalse)
            currentInstructionData.LoadAddOverride = AlwaysFalse;
        if (currentInstructionData.UnloadAddOverride == AlwaysTrue)
            currentInstructionData.UnloadAddOverride = AlwaysTrue;
        if (currentInstructionData.UnloadAddOverride == AlwaysFalse)
            currentInstructionData.UnloadAddOverride = AlwaysFalse;
    }

    private bool AlwaysTrue() => true;
    private bool AlwaysFalse() => false;

    private void UnloadActionAdded(UnloadActionData actionData)
    {
        if (checkQuantity)
        {
            if ((actionData.Action as VesselUnloadAction).Amount == this.quantity)
                quantityPassed = true;
            actionData.OnQuantityChanged.AddListener(EvaluateQuantity);
        }
        else
            quantityPassed = true;

        if (checkCargo)
        {
            if ((actionData.Action as VesselUnloadAction).Cargo == this.cargo)
                cargoPassed = true;
            actionData.OnCargoChanged.AddListener(EvaluateCargo);
        }
        else
            cargoPassed = true;

        if (checkCompartmentIndex)
        {
            if ((actionData.Action as VesselUnloadAction).CompartmentIndex == this.compartmentIndex)
                compartmentIndexPassed = true;
            actionData.OnCompartmentIndexChanged.AddListener(EvaluateCompartmentIndex);
        }
        else
            compartmentIndexPassed = true;

        currentInstructionData.UnloadAddOverride = AlwaysFalse;
    }


    private void EvaluateQuantity(float quantity)
    {
        if (quantity == this.quantity)
            quantityPassed = true;

        Evaluate();
    }


    private void EvaluateCargo(CargoType cargo)
    {
        if (cargo == this.cargo)
            cargoPassed = true;

        Evaluate();
    }

    private void EvaluateCompartmentIndex(int compartmentIndex)
    {
        if (compartmentIndex == this.compartmentIndex)
            compartmentIndexPassed = true;

        Evaluate();
    }

    private void Evaluate()
    {
        if (cargoPassed && compartmentIndexPassed && quantityPassed)
        {
            UnFilter();
            node.MarkComplete();
        }
    }
}

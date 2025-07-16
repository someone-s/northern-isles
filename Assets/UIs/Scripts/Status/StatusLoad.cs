using UnityEngine;
using UnityEngine.UI;

public class StatusLoad : MonoBehaviour
{
    public StatusCompartment Compartment { get; private set; }

    [SerializeField] private Image imageArea;

    public void SetCompartment(StatusCompartment compartment)
    {
        Compartment = compartment;
    }

    public void SetLoad(CargoType type)
    {
        imageArea.sprite = Compartment.Data.Display.Cargo.Sprites[type];
    }
}
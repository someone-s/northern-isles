using UnityEngine;
using UnityEngine.UI;

public class RouteLoad : MonoBehaviour
{
    public RouteCompartment Compartment { get; private set; }
    
    public CargoType Cargo { get; private set; }

    [SerializeField] private Image imageArea;

    public void SetCompartment(RouteCompartment compartment)
    {
        Compartment = compartment;
    }

    public void SetCargo(CargoType cargo)
    {
        Cargo = cargo;
        imageArea.sprite = SpriteCargo.Instance.Sprites[Cargo];
    }

    public void Delete()
    {
        Compartment.DeleteLoad(this);
    }
}
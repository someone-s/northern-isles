using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BuildingDelivery : MonoBehaviour
{
    [SerializeField] private float deliveryIntervalS;
    private float deliveryCountdownS;

    public UnityEvent<float> OnCountdownUpdate;
    public UnityEvent OnDeliverySatisfied;
    public UnityEvent OnDeliveryDissatisfied;

    private JToken cachedState;

    public void Reset()
    {
        deliveryCountdownS = deliveryIntervalS;
        if (deliveryIntervalS != float.PositiveInfinity)
            enabled = true;
        else
            enabled = false;

        OnCountdownUpdate.Invoke(1f);

        OnDeliverySatisfied.Invoke();
    }

    private void Update()
    {
        deliveryCountdownS -= Time.deltaTime * SpeedControl.Instance.TimeScale;
        if (deliveryCountdownS <= 0f)
        {
            OnCountdownUpdate.Invoke(0f);
            enabled = false;

            OnDeliveryDissatisfied.Invoke();
        }
        else
        {
            OnCountdownUpdate.Invoke(deliveryCountdownS / deliveryIntervalS);
        }
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new DeliveryState()
        {
            countdownS = deliveryCountdownS,
            enabled = enabled
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<DeliveryState>();
        deliveryCountdownS = state.countdownS;
        enabled = state.enabled;

        if (deliveryCountdownS != float.PositiveInfinity)
            OnCountdownUpdate.Invoke(deliveryCountdownS / deliveryIntervalS);
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    private struct DeliveryState
    {
        public float countdownS;
        public bool enabled;
    }
}
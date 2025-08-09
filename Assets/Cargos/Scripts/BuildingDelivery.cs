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
        enabled = true;

        OnCountdownUpdate.Invoke(1f);

        OnDeliverySatisfied.Invoke();
    }

    private void Update()
    {
        deliveryCountdownS -= Time.deltaTime;
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

        OnCountdownUpdate.Invoke(deliveryCountdownS);
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
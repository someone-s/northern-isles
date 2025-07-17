using UnityEngine;

public class AccountEffect : MonoBehaviour
{
    [SerializeField] private AudioClip uptake;
    [SerializeField] private AudioClip downturn;

    public void OnPositiveChange()
    {
        GlobalAudio.Instance.Play(uptake);
    }

    public void OnNegativeChange()
    {

        GlobalAudio.Instance.Play(downturn);
    }
}
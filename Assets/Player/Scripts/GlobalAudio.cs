using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio Instance { get; private set; }

    private GlobalAudio()
    {
        Instance = this;
    }

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
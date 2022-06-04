using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FallSFXHandler : AbstractFallFXHandler
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Play()
    {
        _audioSource.Play();
    }
}

using UnityEngine;

public class FallVFXHandler : AbstractFallFXHandler
{
    [SerializeField] private ParticleSystem _vfx;
    protected override void Play()
    {
        _vfx.Play();
    }
}

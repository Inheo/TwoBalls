using MoreMountains.NiceVibrations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;

    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;

    private readonly float _delay = 0.3f;
    private float _lostTime = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if(_lostTime > _delay && _rigidbody.velocity.y < -20 && Physics.Linecast(transform.position, Vector3.down * 2f + transform.position))
        {
            _vfx.Play();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            _lostTime = 0;
            _trailRenderer.emitting = false;
        }

        _lostTime += Time.deltaTime;
    }
}

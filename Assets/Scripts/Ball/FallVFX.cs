using MoreMountains.NiceVibrations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;

    private Rigidbody _rigidbody;

    private readonly float _delay = 0.3f;
    private float _lostTime = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_lostTime > _delay && _rigidbody.velocity.y < -20 && Physics.Linecast(transform.position, Vector3.down * 2f + transform.position))
        {
            _vfx.Play();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            _lostTime = 0;
        }

        _lostTime += Time.deltaTime;
    }
}

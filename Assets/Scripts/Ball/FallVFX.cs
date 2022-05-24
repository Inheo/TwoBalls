using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_rigidbody.velocity.y < -20 && Physics.Linecast(transform.position, Vector3.down * 2.3f + transform.position))
        {
            _vfx.Play();
        }
    }
}

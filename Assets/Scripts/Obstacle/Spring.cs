using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float _force = 50;
    [SerializeField] private ParticleSystem _jumpParticlePrefab;

    private void OnCollisionEnter(Collision other)
    {
        if(other.rigidbody != null)
        {
            other.rigidbody.AddForce(_force * Vector3.up, ForceMode.Impulse);

            SpawnVFX(other.transform.position);
        }
    }

    private void SpawnVFX(Vector3 position)
    {
        var vfx = Instantiate(_jumpParticlePrefab, position, Quaternion.identity);
        vfx.transform.localScale = Vector3.one * 2;
        vfx.Play();
        ParticleSystem.MainModule mainVfx = vfx.main;
        mainVfx.stopAction = ParticleSystemStopAction.Destroy;
    }
}

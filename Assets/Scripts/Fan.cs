using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float _force = 5;
    [SerializeField] private float _enabledTime = 2;
    [SerializeField] private float _disabledTime = 0.5f;
    [SerializeField] private ParticleSystem _fanParticle;
    [SerializeField] private Collider _fanCollider;

    public float Force => _force;

    private void Start()
    {
        StartCoroutine(ActiveFan());
    }

    private IEnumerator ActiveFan()
    {
        WaitForSeconds enabledDelay = new WaitForSeconds(_enabledTime);
        WaitForSeconds disabledDelay = new WaitForSeconds(_disabledTime);

        while(true)
        {
            _fanParticle.Play();
            _fanCollider.enabled = true;

            yield return enabledDelay;

            _fanParticle.Stop();
            _fanCollider.enabled = false;

            yield return disabledDelay;
        }
    }
}
using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private bool _intermittently = true;
    [SerializeField] private float _force = 5;
    [SerializeField] private float _enabledTime = 2;
    [SerializeField] private float _disabledTime = 0.5f;
    [SerializeField] private ParticleSystem _fanParticle;
    [SerializeField] private Collider _fanCollider;

    public float Force => _force;

    private void Start()
    {
        if (_intermittently == true)
        {
            StartCoroutine(ActiveFanIntermittently());
        }
        else
        {
            _fanParticle.Play();
            _fanCollider.enabled = true;
        }
    }

    private IEnumerator ActiveFanIntermittently()
    {
        WaitForSeconds enabledDelay = new WaitForSeconds(_enabledTime);
        WaitForSeconds disabledDelay = new WaitForSeconds(_disabledTime);

        while (true)
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
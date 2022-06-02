using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rippler : MonoBehaviour
{
    [SerializeField] private Ball _ball;

    private Vector3 _startScale;
    private Coroutine _ripple;

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    private void Start()
    {
        Subscribe();
    }


    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _ball.OnFinished += StartRipple;
        _ball.OnExitFinish += StopRipple;
    }

    private void Unsubscribe()
    {
        _ball.OnFinished -= StartRipple;
        _ball.OnExitFinish -= StopRipple;
    }

    private void StartRipple()
    {
        if (_ripple == null)
            _ripple = StartCoroutine(Ripple());
    }

    private void StopRipple()
    {
        transform.localScale = _startScale;
        
        if (_ripple != null)
        {
            StopCoroutine(_ripple);
            _ripple = null;
        }
    }

    private IEnumerator Ripple()
    {
        float duration = 0.4f;
        float lostTime = 0;
        Vector3 endScale = _startScale * 1.1f;

        while (true)
        {
            while (lostTime < 1)
            {
                lostTime += Time.deltaTime / duration;
                transform.localScale = Vector3.Lerp(_startScale, endScale, lostTime);
                yield return null;
            }

            while (lostTime > 0)
            {
                lostTime -= Time.deltaTime / duration;
                transform.localScale = Vector3.Lerp(_startScale, endScale, lostTime);
                yield return null;
            }
        }
    }
}

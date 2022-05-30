using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _deadVFXPrefab;
    [SerializeField] private Transform _rippleTransform;

    private Mover _mover;
    private Rigidbody _rigidbody;
    private Finish _currentFinish;

    private Vector3 _startScale;
    private Coroutine _ripple;

    public event System.Action OnFinished;
    public event System.Action OnFail;

    public bool IsFinished { get; private set; }

    private void Start()
    {
        IsFinished = false;
        _mover = GetComponent<Mover>();
        _rigidbody = GetComponent<Rigidbody>();

        _mover.OnMoveStart += ResetFinish;
        _mover.OnMoveStart += () => _trailRenderer.emitting = true;

        _trailRenderer.enabled = false;

        _startScale = _rippleTransform.localScale;
    }

    private void OnDestroy()
    {
        _mover.OnMoveStart -= ResetFinish;
        _mover.OnMoveStart -= () => _trailRenderer.emitting = true;

    }

    private void LateUpdate()
    {
        if (_currentFinish != null)
        {
            if (Vector3.Distance(transform.position, _currentFinish.transform.position) < _snapDistance)
            {
                IsFinished = true;
                OnFinished?.Invoke();
                if(_ripple == null)
                {
                    _ripple = StartCoroutine(Ripple());
                }
            }
            else if(_ripple != null)
            {
                StopCoroutine(_ripple);
                _ripple = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Finish finish))
        {
            SetFinish(finish);
        }

        if (other.TryGetComponent(out FailPlatform fail))
        {
            OnFail?.Invoke();
            Instantiate(_deadVFXPrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentFinish == null && other.TryGetComponent(out Finish finish))
        {
            SetFinish(finish);
        }
        else if(_rigidbody.velocity.y < 20 &&  other.TryGetComponent(out Fan fan))
        {
            _rigidbody.AddForce(Vector3.up * fan.Force * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Finish finish) && _currentFinish == finish)
        {
            ResetFinish();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_trailRenderer.enabled == false && other.collider.TryGetComponent(out Ball ball) == false)
            _trailRenderer.enabled = true;
    }

    private void ResetFinish()
    {
        _currentFinish = null;
        IsFinished = false;

        if (_ripple != null)
        {
            StopCoroutine(_ripple);
            _ripple = null;
        }

        _rippleTransform.localScale = _startScale;
    }

    private void SetFinish(Finish finish)
    {
        _currentFinish = finish;

        if(_ripple != null)
            StopCoroutine(_ripple);

        _ripple = StartCoroutine(Ripple());
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
                _rippleTransform.localScale = Vector3.Lerp(_startScale, endScale, lostTime);
                yield return null;
            }

            while (lostTime > 0)
            {
                lostTime -= Time.deltaTime / duration;
                _rippleTransform.localScale = Vector3.Lerp(_startScale, endScale, lostTime);
                yield return null;
            }
        }
    }
}
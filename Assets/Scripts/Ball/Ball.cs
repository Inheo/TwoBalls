using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _deadVFXPrefab;

    private Mover _mover;
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

        _mover.OnMoveStart += ResetFinish;

        _trailRenderer.enabled = false;

        _startScale = transform.localScale;
    }

    private void OnDestroy()
    {
        _mover.OnMoveStart -= ResetFinish;
    }

    private void LateUpdate()
    {
        if (_currentFinish != null)
        {
            if (Vector3.Distance(transform.position, _currentFinish.transform.position) < _snapDistance)
            {
                IsFinished = true;
                OnFinished?.Invoke();
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
            StopCoroutine(_ripple);

        transform.localScale = _startScale;
    }

    private void SetFinish(Finish finish)
    {
        _currentFinish = finish;

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
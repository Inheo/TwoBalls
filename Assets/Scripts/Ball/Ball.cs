using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _deadVFXPrefab;
    [SerializeField] private Transform _rippleTransform;

    private Rigidbody _rigidbody;
    private Finish _currentFinish;

    public event System.Action OnFinished;
    public event System.Action OnExitFinish;
    public event System.Action OnFail;

    public bool IsFinished { get; private set; }

    private void Start()
    {
        IsFinished = false;
        _rigidbody = GetComponent<Rigidbody>();

        _trailRenderer.enabled = false;
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
            else if(IsFinished == true)
            {
                OnExitFinish?.Invoke();
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
        if (_currentFinish == null && other.TryGetComponent(out Finish finish) && finish.IsBusy == false)
        {
            SetFinish(finish);
        }
        else if(_rigidbody.velocity.y < 20 && other.TryGetComponent(out Fan fan))
        {
            _rigidbody.velocity = Vector3.up * fan.Force;
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
        _currentFinish.IsBusy = false;
        _currentFinish = null;
        IsFinished = false;
        OnExitFinish?.Invoke();
    }

    private void SetFinish(Finish finish)
    {
        _currentFinish = finish;
        _currentFinish.IsBusy = true;
    }
}
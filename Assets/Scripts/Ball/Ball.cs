using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;

    private Finish _currentFinish;

    public event System.Action OnFinished;
    public event System.Action OnFail;

    public bool IsFinished { get; private set; }

    private void Start()
    {
        IsFinished = false;
        _rigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
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
            _currentFinish = finish;
        }

        if (other.TryGetComponent(out FailPlatform fail))
        {
            OnFail?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Finish finish) && _currentFinish == finish)
        {
            _currentFinish = null;
            IsFinished = false;
        }
    }
}

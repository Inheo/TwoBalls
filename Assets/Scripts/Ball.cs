using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;

    private Finish _currentFinish;

    public bool IsFinished { get; private set; }

    private void Start()
    {
        IsFinished = false;
        _rigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (_currentFinish != null)
        {
            if (Vector3.Distance(transform.position, _currentFinish.transform.position) < _snapDistance && _currentFinish.IsBusy == false)
            {
                transform.position = _currentFinish.transform.position;
                IsFinished = true;
                _currentFinish.IsBusy = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _sphereCollider.isTrigger = true;
            }
            else if (_currentFinish.IsBusy == true)
            {
                _currentFinish = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentFinish == null && other.TryGetComponent(out _currentFinish)) ;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out _currentFinish))
        {
            _currentFinish = null;
        }
    }
}

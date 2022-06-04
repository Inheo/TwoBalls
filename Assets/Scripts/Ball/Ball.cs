using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Mover))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _snapDistance = 0.1f;
    [SerializeField] private TrailRenderer _trailRenderer;

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

        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        Level.Instance.OnLevelComplete += CompletedLevel;
    }

    private void Unsubscribe()
    {
        Level.Instance.OnLevelComplete -= CompletedLevel;
    }

    private void CompletedLevel()
    {
        Vector3 to = transform.position;
        to.x = _currentFinish.transform.position.x;

        GetComponent<Mover>().MoveTo(0.3f, to);
    }

    private void LateUpdate()
    {
        if (Level.Instance.IsLevelEnd == false && _currentFinish != null)
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
        else if (other.TryGetComponent(out FailPlatform fail))
        {
            OnFail?.Invoke();
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
        if(_currentFinish != null)
            _currentFinish.IsBusy = false;
            
        _currentFinish = finish;
        _currentFinish.IsBusy = true;
    }
}
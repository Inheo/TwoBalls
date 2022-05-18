using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _durationMove = 0.5f;
    [SerializeField] private float _moveStep = 2;
    [SerializeField] private float _groundCheckerRadius = 0.2f;
    [SerializeField] private SwipeInput _swipeInput;
    [SerializeField] private LayerMask _platformLayer;

    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    private Collider[] _platformCollider = new Collider[1];

    public event System.Action OnMoveStart;

    public bool CanMove { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _swipeInput.OnSwipeHorizontal += Move;
    }

    private void Update()
    {
        if (_coroutine == null)
        {
            CanMove = Physics.OverlapSphereNonAlloc(transform.position + Vector3.down, _groundCheckerRadius, _platformCollider, _platformLayer) > 0;
        }
    }

    private void OnDestroy()
    {
        _swipeInput.OnSwipeHorizontal -= Move;
    }

    private void Move(int directionX)
    {
        if (_coroutine == null && Level.Instance.IsLevelEnd == false)
            _coroutine = StartCoroutine(MoveTo(Vector3.right * directionX));
    }

    private IEnumerator MoveTo(Vector3 direction)
    {
        OnMoveStart?.Invoke();
        float lostTime = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + direction * _moveStep;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _moveStep, _platformLayer))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.x = hitPoint.x + (_moveStep * 0.5f * Mathf.Sign(-direction.x));
            float distance = Vector3.Distance(startPosition, hitPoint);

            if (distance < _moveStep)
            {
                endPosition.x = hitPoint.x;
                lostTime = 1 - distance / _moveStep;
            }
        }

        while (lostTime < 1)
        {
            lostTime += Time.deltaTime / _durationMove;
            
            transform.position = Vector3.Lerp(new Vector3(startPosition.x, transform.position.y, transform.position.z),
                                            new Vector3(endPosition.x, transform.position.y, transform.position.z),
                                            lostTime);
            yield return null;
        }

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        _coroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Vector3.down, _groundCheckerRadius);
    }
}

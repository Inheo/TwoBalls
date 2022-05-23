using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _durationMove = 0.5f;
    [SerializeField] private float _moveStep = 2;
    [SerializeField] private SwipeInput _swipeInput;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private LayerMask _playerLayer;

    private SphereCollider _sphereCollider;

    private Coroutine _coroutine;

    public event System.Action OnMoveStart;
    public float Radius => _sphereCollider.radius * transform.lossyScale.x;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _swipeInput.OnSwipeHorizontal += Move;
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
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.position + direction * _moveStep;

        Check(direction, startPoint, ref endPoint, ref lostTime);

        while (lostTime < 1)
        {
            lostTime += Time.deltaTime / _durationMove;

            transform.position = Vector3.Lerp(new Vector3(startPoint.x, transform.position.y, transform.position.z),
                                            new Vector3(endPoint.x, transform.position.y, transform.position.z),
                                            lostTime);
            yield return null;
        }
        _coroutine = null;
    }

    private void Check(Vector3 direction, Vector3 startPoint, ref Vector3 endPoint, ref float lostTime)
    {
        if (CastRay(startPoint, direction, out RaycastHit hit, _moveStep / 2, _playerLayer))
        {
            if (CastRay(startPoint, direction, out hit, _moveStep * 2, _platformLayer))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.x = hitPoint.x + (Radius * Mathf.Sign(-direction.x)) - _moveStep;
                float distance = Vector3.Distance(startPoint, hitPoint);

                if (distance < _moveStep)
                {
                    endPoint.x = hitPoint.x;
                    lostTime = 1 - distance / _moveStep;
                }
            }

            return;
        }

        if (CastRay(startPoint, direction, out hit, _moveStep, _platformLayer))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.x = hitPoint.x + (Radius * Mathf.Sign(-direction.x));
            float distance = Vector3.Distance(startPoint, hitPoint);

            if (distance < _moveStep)
            {
                endPoint.x = hitPoint.x;
                lostTime = 1 - distance / _moveStep;
            }
        }
    }

    private bool CastRay(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance, int layer)
    {
        return Physics.Raycast(origin, direction, out hit, distance, layer);
    }
}
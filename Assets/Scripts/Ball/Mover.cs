using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 12;
    [SerializeField] private SwipeInput _swipeInput;

    private SphereCollider _sphereCollider;
    private Rigidbody _rigidbody;

    private Coroutine _moveInTime;

    public event System.Action OnMoved;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Level.Instance.IsLevelEnd == false)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

            Vector3 endPoint = transform.position + Vector3.right * _swipeInput.DirectionX;

            transform.position = Vector3.Lerp(transform.position, endPoint, _speed * Time.deltaTime);

            if(_swipeInput.DirectionX != 0)
                OnMoved?.Invoke();
        }
        else
        {
            Vector3 velocity = _rigidbody.velocity;
            velocity.x = 0;
            _rigidbody.velocity = velocity;
        }
    }

    private IEnumerator MoveInTime(float duration, Vector3 endPoint)
    {
        float lostTime = 0;
        Vector3 startPoint = transform.position;

        while (lostTime < 1)
        {
            lostTime += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPoint, endPoint, lostTime);
            yield return null;
        }
    }

    public void MoveTo(float duration, Vector3 endPoint)
    {
        if (_moveInTime != null)
            StopCoroutine(_moveInTime);

        _moveInTime = StartCoroutine(MoveInTime(duration, endPoint));
    }
}
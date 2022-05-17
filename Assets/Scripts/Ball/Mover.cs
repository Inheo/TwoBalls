using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ball))]
public class Mover : MonoBehaviour
{
    private const string MOUSE_X_PARAMETER = "Mouse X";
    [SerializeField] private float _durationMove = 0.5f;
    [SerializeField] private SwipeInput _swipeInput;

    private Rigidbody _rigidbody;
    private Coroutine _coroutine;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        float lostTime = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + direction * 2;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        while (lostTime < 1)
        {
            lostTime += Time.deltaTime / _durationMove;
            _rigidbody.MovePosition(Vector3.Lerp(new Vector3(startPosition.x, transform.position.y, transform.position.z), new Vector3(endPosition.x, transform.position.y, transform.position.z), lostTime));
            // transform.position = Vector3.Lerp(new Vector3(startPosition.x, transform.position.y, transform.position.z), new Vector3(endPosition.x, transform.position.y, transform.position.z), lostTime);
            yield return null;
        }

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        _coroutine = null;
    }
}

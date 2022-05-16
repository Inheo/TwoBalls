using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ball))]
public class Mover : MonoBehaviour
{
    private const string MOUSE_X_PARAMETER = "Mouse X";
    [SerializeField] private float _durationMove = 0.5f;
    [SerializeField] private SwipeInput _swipeInput;

    private Ball _ball;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ball = GetComponent<Ball>();
        _swipeInput.OnSwipeHorizontal += Move;
    }

    private void OnDestroy()
    {
        _swipeInput.OnSwipeHorizontal -= Move;
    }

    private void Move(int directionX)
    {
        if (_coroutine == null && _ball.IsFinished == false)
            _coroutine = StartCoroutine(MoveTo(Vector3.right * directionX));
    }

    private IEnumerator MoveTo(Vector3 direction)
    {
        float lostTime = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + direction * 2.4f;

        while (lostTime < 1 && _ball.IsFinished == false)
        {
            lostTime += Time.deltaTime / _durationMove;
            // _rigidbody.MovePosition(transform.position + direction * Time.deltaTime * 20);
            transform.position = Vector3.Lerp(new Vector3(startPosition.x, transform.position.y, transform.position.z), new Vector3(endPosition.x, transform.position.y, transform.position.z), lostTime);
            yield return null;
        }

        _coroutine = null;
    }
}

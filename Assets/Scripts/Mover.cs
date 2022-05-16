using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ball))]
public class Mover : MonoBehaviour
{
    private const string MOUSE_X_PARAMETER = "Mouse X";
    [SerializeField] private float _speed = 5;

    private Ball _ball;
    private Rigidbody _rigidbody;

    private float _deltaMouseX;
    private bool _isTouch;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ball = GetComponent<Ball>();
    }

    private void Update()
    {
        MouseButtonDown();

        MouseButtonUp();

        CalculateDelataXForEditor();

        CalculateDelataXForMobile();
    }

    private void FixedUpdate()
    {
        if (_ball.IsFinished == false)
            _rigidbody.velocity = new Vector3(_deltaMouseX == 0 ? _rigidbody.velocity.x : _deltaMouseX * _speed, _rigidbody.velocity.y, 0);
    }

    private void CalculateDelataXForMobile()
    {
        if (Input.touchCount > 0)
        {
            _deltaMouseX = Input.touches[0].deltaPosition.x;
        }
    }

    private void CalculateDelataXForEditor()
    {
        if (_isTouch)
        {
            _deltaMouseX = Input.GetAxis(MOUSE_X_PARAMETER);
        }
    }

    private void MouseButtonUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _isTouch = false;
        }
    }

    private void MouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isTouch = true;
        }
    }
}

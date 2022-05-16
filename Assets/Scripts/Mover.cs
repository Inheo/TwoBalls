using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    private const string MOUSE_X_PARAMETER = "Mouse X";
    [SerializeField] private float _speed;

    private Rigidbody _rb;

    private float _deltaMouseX;
    private bool _isTouch;
    private bool _canMove;

    private void Start()
    {
        _canMove = true;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isTouch = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isTouch = false;
        }

        if (_isTouch)
        {
            _deltaMouseX = Input.GetAxis(MOUSE_X_PARAMETER);
        }

        if (Input.touchCount > 0)
        {
            _deltaMouseX = Input.touches[0].deltaPosition.x;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_deltaMouseX == 0 ? _rb.velocity.x : _deltaMouseX * _speed , _rb.velocity.y, 0);
    }
}

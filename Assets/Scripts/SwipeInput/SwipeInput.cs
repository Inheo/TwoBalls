using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private const string DEFAULT_SWIPE_POLICY_NAME = "swipe-policy";
    [SerializeField] private AbstractSwipePolicy _swipePolicy;

    public event System.Action<int> OnSwipeHorizontal;

    public int DirectionX { get; private set; }

    private Vector3 _startMousePosition;
    private Vector3 _previousPosition;

    private void Start()
    {
        if (_swipePolicy == null)
        {
            _swipePolicy = new GameObject(DEFAULT_SWIPE_POLICY_NAME).AddComponent<SwipePolicyWhenAllBallsOnGround>();
            _swipePolicy.transform.parent = transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Input.mousePosition;
            _previousPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            float distanceX = Input.mousePosition.x - _previousPosition.x;

            if (Mathf.Abs(distanceX) > 3)
            {
                DirectionX = distanceX < 0 ? -1 : 1;
                OnSwipeHorizontal?.Invoke(DirectionX);
                _previousPosition = Input.mousePosition;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            DirectionX = 0;
        }
    }
}

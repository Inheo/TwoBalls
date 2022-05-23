using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private const string DEFAULT_SWIPE_POLICY_NAME = "swipe-policy";
    [SerializeField] private AbstractSwipePolicy _swipePolicy;

    public event System.Action<int> OnSwipeHorizontal;

    private Vector3 _startMousePosition;

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
        }

        if (Input.GetMouseButtonUp(0) && _swipePolicy.CanSwipe())
        {
            float distanceX = Input.mousePosition.x - _startMousePosition.x;

            if (Mathf.Abs(distanceX) > 5)
            {
                int directionX = distanceX < 0 ? -1 : 1;
                OnSwipeHorizontal?.Invoke(directionX);
            }
        }
    }
}

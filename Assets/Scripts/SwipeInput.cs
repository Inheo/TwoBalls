using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public event System.Action<int> OnSwipeHorizontal;

    private Vector3 _startMousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
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

using UnityEngine;

public class SinusMover : MonoBehaviour
{
    [SerializeField] private float _ySpeed = 1f;
    [SerializeField] private float _xSpeed = 1f;
    [SerializeField] private float _amplitude = 1f;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * _ySpeed) * _amplitude;
        float x = Mathf.Cos(Time.time * _xSpeed) * _amplitude;

        transform.position = new Vector3(_startPosition.x + x, _startPosition.y + y, transform.position.z);
    }
}

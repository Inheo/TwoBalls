using UnityEngine;

public class SinusMover : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _amplitude = 1f;

    private void Update()
    {
        float y = Mathf.Sin(Time.time * _speed) * _amplitude;

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}

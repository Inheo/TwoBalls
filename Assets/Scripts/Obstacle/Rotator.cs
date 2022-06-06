using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateEuler = Vector3.up;
    [SerializeField] private float _speed = 5;

    private void Update()
    {
        transform.Rotate(_rotateEuler * _speed * Time.deltaTime);
    }
}

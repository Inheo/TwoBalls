using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 12;
    [SerializeField] private SwipeInput _swipeInput;

    private SphereCollider _sphereCollider;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Level.Instance.IsLevelEnd == false)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

            Vector3 endPoint = transform.position + Vector3.right * _swipeInput.DirectionX;

            transform.position = Vector3.Lerp(transform.position, endPoint, _speed * Time.deltaTime);
        }
        else
        {
            Vector3 velocity = _rigidbody.velocity;
            velocity.x = 0;
            _rigidbody.velocity = velocity;
        }
    }
}
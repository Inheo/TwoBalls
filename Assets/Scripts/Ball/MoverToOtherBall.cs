using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class MoverToOtherBall : MonoBehaviour
{
    [SerializeField] private float _moveForce = 100;
    [SerializeField] private Ball _otherBall;

    private Mover _mover;
    private ObiActor _obiActor;

    private void Start()
    {
        _mover = GetComponent<Mover>();
        _obiActor = GetComponent<ObiActor>();
    }

    private void Update() {
        // if(_mover.IsMoved == false && Level.Instance.IsLevelEnd == false && Mathf.Abs(transform.position.x - _otherBall.transform.position.x) > 2.5f)
        // {
        //     Vector3 direction = (_otherBall.transform.position - transform.position).normalized;
        //     direction.y = 0;
        //     direction.z = transform.position.z > 0 ? -1 : transform.position.z < 0 ? 1 : 0;

        //     _obiActor.AddForce(direction * _moveForce * Time.deltaTime, ForceMode.Impulse);
        // }
    }
}

using System.Collections;
using Obi;
using UnityEngine;

[RequireComponent(typeof(ObiActor))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _force = 250f;
    [SerializeField] private float _groundCheckerRadius = 0.2f;
    [SerializeField] private SwipeInput _swipeInput;
    [SerializeField] private LayerMask _platformLayer;

    private ObiActor _obiActor;
    private Collider[] _platformCollider = new Collider[1];

    public event System.Action OnMoveStart;

    public bool CanMove { get; private set; }
    public bool IsMoved { get; private set; }

    private void Start()
    {
        _obiActor = GetComponent<ObiActor>();
        _swipeInput.OnSwipeHorizontal += Move;
    }

    private void Update()
    {
        CanMove = Physics.OverlapSphereNonAlloc(transform.position + Vector3.down, _groundCheckerRadius, _platformCollider, _platformLayer) > 0;
    }

    private void OnDestroy()
    {
        _swipeInput.OnSwipeHorizontal -= Move;
    }

    private void Move(int directionX)
    {
        if (Level.Instance.IsLevelEnd == false)
        {
            IsMoved = true;
            StartCoroutine(Delay(1.3f, () => IsMoved = false));
            Vector3 directionForce = Vector3.right * directionX * _force;
            directionForce.z = (directionForce - transform.position).normalized.z;
            _obiActor.AddForce(directionForce, ForceMode.Impulse);
        }
    }

    private IEnumerator Delay(float delayTime, System.Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Vector3.down, _groundCheckerRadius);
    }
}

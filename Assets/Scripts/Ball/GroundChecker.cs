using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _groundCheckerRadius = 0.2f;
    [SerializeField] private LayerMask _platformLayer;

    private Collider[] _platformCollider = new Collider[1];

    public bool IsGround()
    {
        return Physics.OverlapSphereNonAlloc(transform.position + Vector3.down, _groundCheckerRadius, _platformCollider, _platformLayer) > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Vector3.down, _groundCheckerRadius);
    }
}
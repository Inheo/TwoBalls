using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailSwitcher : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private FallEmitter _fallEmitter;

    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        _trailRenderer.enabled = false;

        Subscribe();
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_trailRenderer.enabled == false && other.collider.TryGetComponent(out Ball ball) == false)
            _trailRenderer.enabled = true;
    }

    private void Subscribe()
    {
        _mover.OnMoved += EmittingEnable;
        _fallEmitter.OnFalled += EmittingDisable;
    }
    private void Unsubscribe()
    {
        _mover.OnMoved += EmittingEnable;
        _fallEmitter.OnFalled += EmittingDisable;
    }

    private void EmittingEnable()
    {
        _trailRenderer.emitting = true;
    }

    private void EmittingDisable()
    {
        _trailRenderer.emitting = false;
    }
}
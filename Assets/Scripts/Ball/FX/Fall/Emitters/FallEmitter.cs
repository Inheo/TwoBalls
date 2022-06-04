using UnityEngine;

public class FallEmitter : AbstracFallEmitter
{
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Rigidbody _rigidbody;

    private readonly float _delay = 0.3f;
    private float _lostTime = 0;

    public override event System.Action OnFalled;

    private void Update()
    {
        if(IsFalled() == true)
        {
            _lostTime = 0;

            OnFalled?.Invoke();  
        }

        _lostTime += Time.deltaTime;
    }

    protected override bool IsFalled() => _lostTime > _delay && _rigidbody.velocity.y < -20 && Physics.Linecast(transform.position, Vector3.down * 2f + transform.position, _platformLayer);
}
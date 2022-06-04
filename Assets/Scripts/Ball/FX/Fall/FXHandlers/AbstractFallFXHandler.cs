using UnityEngine;

public abstract class AbstractFallFXHandler : MonoBehaviour
{
    [SerializeField] private AbstracFallEmitter _fallEmitter;

    private void Start()
    {
        _fallEmitter.OnFalled += Play;
    }

    private void OnDestroy()
    {
        _fallEmitter.OnFalled -= Play;
    }

    protected abstract void Play();
}

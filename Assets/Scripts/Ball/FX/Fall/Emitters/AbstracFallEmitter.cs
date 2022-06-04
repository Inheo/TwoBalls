using UnityEngine;

public abstract class AbstracFallEmitter : MonoBehaviour
{
    public abstract event System.Action OnFalled;

    protected abstract bool IsFalled();
}

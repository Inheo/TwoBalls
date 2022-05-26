using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{
    [SerializeField] protected float AnimationDuration = 1;

    protected RectTransform RectTransform;

    protected virtual void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public abstract void PlayAnimation();
}
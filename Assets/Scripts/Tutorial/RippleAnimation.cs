using UnityEngine;
using DG.Tweening;

public class RippleAnimation : UIAnimation
{
    private Vector2 _startSizeDelta;
    private Vector2 _endSizeDelta;

    protected override void Awake()
    {
        base.Awake();

        _startSizeDelta = RectTransform.sizeDelta;
        _endSizeDelta = new Vector2(_startSizeDelta.x + 30, _startSizeDelta.y + 30);
    }

    public override void PlayAnimation()
    {
        RectTransform.DOSizeDelta(_endSizeDelta, AnimationDuration).From(_startSizeDelta).SetLoops(-1, LoopType.Yoyo);
    }
}
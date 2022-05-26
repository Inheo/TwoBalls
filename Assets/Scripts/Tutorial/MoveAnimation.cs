using UnityEngine;
using DG.Tweening;

public class MoveAnimation : UIAnimation
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;

    public override void PlayAnimation()
    {
        RectTransform.DOAnchorPos(_endPosition, AnimationDuration).From(_startPosition).SetLoops(-1, LoopType.Yoyo);
    }
}

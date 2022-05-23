using System.Linq;
using UnityEngine;

public class SwipePolicyWhenAllBallsOnGround : AbstractSwipePolicy
{
    [SerializeField] private GroundChecker[] _groundCheckers;

    private void Start()
    {
        _groundCheckers = FindObjectsOfType<GroundChecker>();
    }

    public override bool CanSwipe()
    {
        return _groundCheckers.Where(mover => mover.IsGround() == true).Count() == _groundCheckers.Length;
    }
}

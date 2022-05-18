using System.Linq;
using UnityEngine;

public class AllMoversCanMoveSwipePolicy : AbstractSwipePolicy
{
    [SerializeField] private Mover[] _ballMovers;

    private void Start()
    {
        _ballMovers = FindObjectsOfType<Mover>();
    }

    public override bool CanSwipe()
    {
        return _ballMovers.Where(mover => mover.CanMove == true).Count() == _ballMovers.Length;
    }
}

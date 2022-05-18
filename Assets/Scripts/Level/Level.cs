using System.Linq;
using UnityEngine;
using Scripts.Data;

public class Level : MonoBehaviour
{
    [SerializeField] private VFXPlayer _vfxPlayer;
    [SerializeField] private Ball[] _balls;

    public event System.Action OnLevelComplete;
    public event System.Action OnLevelFail;

    public bool IsLevelEnd { get; private set; }

    public static Level Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsLevelEnd = false;
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        for (int i = 0; i < _balls.Length; i++)
        {
            _balls[i].OnFinished += CheckWin;
            _balls[i].OnFail += Fail;
        }
    }

    private void Unsubscribe()
    {
        for (int i = 0; i < _balls.Length; i++)
        {
            _balls[i].OnFinished -= CheckWin;
            _balls[i].OnFail -= Fail;
        }
    }

    private void CheckWin()
    {
        if(IsLevelEnd == true) return;

        bool isWin = _balls.Where(ball => ball.IsFinished == true).Count() == _balls.Length;

        if (isWin == true)
        {
            IsLevelEnd = true;
            _vfxPlayer.Play();
            PlayerProgress.GetData().CompleteLevel();
            
            OnLevelComplete?.Invoke();
        }
    }

    private void Fail()
    {
        if(IsLevelEnd == true) return;
        
        IsLevelEnd = true;
        OnLevelFail?.Invoke();
    }
}
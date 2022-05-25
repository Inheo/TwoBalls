using System.Linq;
using UnityEngine;
using Scripts.Data;
using MoreMountains.NiceVibrations;
using System.Collections;

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
        MMVibrationManager.StopAllHaptics();
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
        if (IsLevelEnd == true) return;

        bool isWin = _balls.Where(ball => ball.IsFinished == true).Count() == _balls.Length;

        if (isWin == true)
        {
            IsLevelEnd = true;
            _vfxPlayer.Play();
            PlayerProgress.GetData().CompleteLevel();
            PlayWinVibration(0.8f, 3);

            OnLevelComplete?.Invoke();
        }
    }

    private void Fail()
    {
        if (IsLevelEnd == true) return;

        IsLevelEnd = true;
        OnLevelFail?.Invoke();
    }

    private void PlayWinVibration(float duration, int countSteps)
    {
        StartCoroutine(PlayVibration(duration, countSteps));
    }

    private IEnumerator PlayVibration(float durationAllVibration, int countSteps)
    {
        float oneStepDuration = durationAllVibration / countSteps;
        WaitForSeconds delay = new WaitForSeconds(oneStepDuration * 1.5f);

        for (int i = 0; i < countSteps - 1; i++)
        {
            MMVibrationManager.ContinuousHaptic(0.2f, 0.8f, oneStepDuration, HapticTypes.Success, this);
            yield return delay;
        }

        MMVibrationManager.ContinuousHaptic(0.2f, 0.8f, oneStepDuration * 2.3f, HapticTypes.Success, this);
    }
}
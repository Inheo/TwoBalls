using MistplaySDK;
using UnityEngine;

public class MistplayEventSender : MonoBehaviour
{
    [SerializeField] private Game _game;

    private void Awake()
    {
        _game.OnStartLevel += MistplayAnalyticsManager.StartLevel;
        _game.OnFinishLevel += MistplayAnalyticsManager.FinishLevel;
        _game.OnFailLevel += MistplayAnalyticsManager.FailLevel;
    }

    private void OnDestroy()
    {
        _game.OnStartLevel -= MistplayAnalyticsManager.StartLevel;
        _game.OnFinishLevel -= MistplayAnalyticsManager.FinishLevel;
        _game.OnFailLevel -= MistplayAnalyticsManager.FailLevel;
    }
}

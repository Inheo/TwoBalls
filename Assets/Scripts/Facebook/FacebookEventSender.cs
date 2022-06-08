using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

namespace Assets.Scripts
{

    public class FacebookEventSender : MonoBehaviour
    {
        private const string LEVEL_NUMBER_PARAMETER = "Level number: ";
        private const string START_LEVEL_PARAMETER = "Start level";
        private const string FINISH_LEVEL_PARAMETER = "Finish level";
        private const string FAIL_LEVEL_PARAMETER = "Fail level";

        [SerializeField] private Game _game;

        private void Start()
        {
            _game.OnStartLevel += StartLevel;
            _game.OnFinishLevel += FinishLevel;
            _game.OnFailLevel += FailLevel;
        }

        public void StartLevel(int level)
        {
            SendLogAppEvent(level, START_LEVEL_PARAMETER);
        }

        public void FinishLevel(int level)
        {
            SendLogAppEvent(level, FINISH_LEVEL_PARAMETER);
        }

        public void FailLevel(int level)
        {
            SendLogAppEvent(level, FAIL_LEVEL_PARAMETER);
        }

        public void SendLogAppEvent(int level, string appEventName)
        {
            var tutParams = new Dictionary<string, object>();
            tutParams[LEVEL_NUMBER_PARAMETER] = level.ToString();

            FB.LogAppEvent
            (
                appEventName,
                parameters: tutParams
            );
        }

        private void OnDestroy()
        {
            _game.OnStartLevel -= StartLevel;
            _game.OnFinishLevel -= FinishLevel;
            _game.OnFailLevel -= FailLevel;
        }
    }
}
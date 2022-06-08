namespace MistplaySDK
{
#if mistplay_gameanalytics_enabled

    using UnityEngine;
    using GameAnalyticsSDK;
    using System;
    using System.Collections;

    public class MistplayAnalyticsManager : MonoBehaviour, IGameAnalyticsATTListener
    {
        public static event Action OnRemoteConfigsReady, OnRemoteConfigsUpdated;
        bool readyCallback;

        void Awake()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                InitGameAnalyticsWithCustomID();
            }

            GameAnalytics.OnRemoteConfigsUpdatedEvent += RemoteConfigUpdated;
            readyCallback = true;

            StartCoroutine(WaitForReady());
        }

        void OnDestroy()
        {
            GameAnalytics.OnRemoteConfigsUpdatedEvent -= RemoteConfigUpdated;
        }

        IEnumerator WaitForReady()
        {
            while (!readyCallback || GameAnalytics.GetRemoteConfigsContentAsString().Length < 3)
                yield return null;

            readyCallback = false;
            OnRemoteConfigsReady?.Invoke();
        }

        void RemoteConfigUpdated()
        {
            OnRemoteConfigsUpdated?.Invoke();
        }

        void InitGameAnalyticsWithCustomID()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            InitGameAnalyticsWithCustomID();
        }
        public void GameAnalyticsATTListenerRestricted()
        {
            InitGameAnalyticsWithCustomID();
        }
        public void GameAnalyticsATTListenerDenied()
        {
            InitGameAnalyticsWithCustomID();
        }
        public void GameAnalyticsATTListenerAuthorized()
        {
            InitGameAnalyticsWithCustomID();
        }

        public static bool IsRemoteConfigsReady => GameAnalytics.IsRemoteConfigsReady();
        public static string GetRemoteConfigsContentAsString() => GameAnalytics.GetRemoteConfigsContentAsString();
        public static string GetRemoteConfigsValueAsString(string id) => GameAnalytics.GetRemoteConfigsValueAsString(id);

        public static void StartLevel(int level)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, level.ToString());
        }

        public static void FinishLevel(int level, int completion)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, level.ToString(), completion);
        }

        public static void FinishLevel(int level)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, level.ToString());
        }

        public static void FailLevel(int level, int completion)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, level.ToString(), completion);
        }

        public static void FailLevel(int level)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, level.ToString());
        }

        public static void GenericProgression(string p0, string p1 = null, string p2 = null)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, p0, p1, p2);
        }

        public static void CustomTrackingEvent(string id, float value) => GameAnalytics.NewDesignEvent(id, value);
    }

#endif
}
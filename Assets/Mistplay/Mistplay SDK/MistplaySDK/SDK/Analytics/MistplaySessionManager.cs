using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MistplaySDK
{
    [DefaultExecutionOrder(-1000)]
    public class MistplaySessionManager : Singleton<MistplaySessionManager>
    {
        #region Types
        public delegate bool Event(Context context);
        public struct Context
        {
            public int Session;
            public int Day;
            public float SessionDuration;
            public float LifetimeDuration;
        }
        #endregion

        #region Keys
        const string SessionCountKey = "MSM_SessionCount";
        const string DayCountKey = "MSM_DayCount";
        const string LastDayKey = "MSM_LastDay";
        const string LifetimeDurationKey = "MSM_LifetimeDuration";
        #endregion

        #region Cache
        int sessionCount;
        int dayCount;
        float previousSessionsDuration;
        #endregion

        const float EvaluationDelay = 5; // Delay in seconds between evaluating each event, might have to reduce it if you have a lot

        HashSet<Event> events;

        void Start()
        {
            NewSession();
            StartCoroutine(Evaluate());
        }

        IEnumerator Evaluate()
        {
            var ctx = new Context()
            {
                Session = sessionCount,
                Day = dayCount,
                LifetimeDuration = LifetimeDuration
            };

            var toRemove = new HashSet<Event>();

            while(true)
            {
                if(events != null)
                foreach(var e in events)
                {
                    ctx.SessionDuration = Time.realtimeSinceStartup;

                    if(e(ctx)) toRemove.Add(e);

                    yield return new WaitForSeconds(EvaluationDelay);
                }

                foreach(var e in toRemove)
                    events.Remove(e);

                toRemove.Clear();
                
                yield return new WaitForSeconds(EvaluationDelay);
            }
        }

        void OnApplicationQuit()
        {
            SaveSessionTime();
        }

        void NewSession()
        {
            if(PlayerPrefs.HasKey(SessionCountKey))
                sessionCount = PlayerPrefs.GetInt(SessionCountKey) + 1;
            else sessionCount = 1;
            
            PlayerPrefs.SetInt(SessionCountKey, sessionCount);

            if(PlayerPrefs.HasKey(LastDayKey))
            {
                dayCount = PlayerPrefs.GetInt(DayCountKey);
                var lastDay = PlayerPrefs.GetInt(LastDayKey);

                if(EpochDay != lastDay)
                {
                    ++dayCount;
                    PlayerPrefs.SetInt(LastDayKey, EpochDay);
                    PlayerPrefs.SetInt(DayCountKey, dayCount);
                }
            }
            else
            {
                PlayerPrefs.SetInt(LastDayKey, EpochDay);
                PlayerPrefs.SetInt(DayCountKey, 0);
            }

            if(PlayerPrefs.HasKey(LifetimeDurationKey))
                previousSessionsDuration = PlayerPrefs.GetFloat(LifetimeDurationKey);

            PlayerPrefs.Save();

            Log("Session Count : " + sessionCount);
            Log("Day Count : " + dayCount);
        }

        void SaveSessionTime()
        {
            PlayerPrefs.SetFloat(LifetimeDurationKey, LifetimeDuration);
            PlayerPrefs.Save();
        }

        float LifetimeDuration => previousSessionsDuration + Time.realtimeSinceStartup;
        int EpochDay => (System.DateTime.Now - new System.DateTime(2022, 5, 15)).Days;

        public void AddEvent(Event callback)
        {
            if(events == null) events = new HashSet<Event>();
            events.Add(callback);
        }
    }
}
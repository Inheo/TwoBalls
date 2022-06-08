using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameAnalyticsSDK;

namespace MistplaySDK
{
    public class MistplayABTestManager : Singleton<MistplayABTestManager>
    {
        [Serializable]
        public class Test
        {
            public string Key;
            public Variation[] Variations;
        }

        [Serializable]
        public class Variation
        {
            public string Value;
            public UnityEvent On, Off;

            public Variation(string value)
            {
                Value = value;
                On = new UnityEvent();
                Off = new UnityEvent();
            }
        }

        [field: SerializeField] public List<Test> Tests { get; private set; }
        
        protected override void OnAwake()
        {
            if(Tests == null) Tests = new List<Test>();

            // Current Hex / Ads test
            var ads = new Variation("ads");
            ads.On.AddListener(() => MistplayAdvertisingManager.Instance.InitializeAds());

            var hex = new Variation("hex");
            hex.On.AddListener(() => MistplayHexManager.Instance.Initialize());

            var adshex = new Variation("adshex");
            adshex.On.AddListener(() => MistplayHexManager.Instance.Initialize());
            adshex.On.AddListener(() => MistplayAdvertisingManager.Instance.InitializeAds());

            var test = new Test();
            test.Key = "ads";
            test.Variations = new Variation[]{ ads, hex, adshex }; 
            Tests.Add(test);

            MistplayAnalyticsManager.OnRemoteConfigsReady += OnReady;
        }

        void OnReady()
        {
            Debug.Log("Mistplay AB Test : Ready");
            foreach(var test in Tests)
            {
                var value = GameAnalytics.GetRemoteConfigsValueAsString(test.Key);
                Debug.Log($"Mistplay AB Test : Test '{test.Key}' : '{value}'");
                if(value != null)
                {
                    foreach(var variation in test.Variations)
                    if(value.Contains(variation.Value))
                    {
                        Debug.Log($"Mistplay AB Test : Variation '{variation.Value}'");
                        variation.On.Invoke();
                    }
                    else variation.Off.Invoke();
                }
            }
        }
    }
}
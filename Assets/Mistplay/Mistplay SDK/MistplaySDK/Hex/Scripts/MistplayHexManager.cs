using UnityEngine;

#if UNITY_ANDROID
using com.mistplay.hex;
#endif

namespace MistplaySDK
{
    public class MistplayHexManager : Singleton<MistplayHexManager>
    {
        [SerializeField] OverlaySide overlaySide;
        [SerializeField] int overlayPadding = 80;
        [SerializeField] bool initializeByDefault = false;

        protected override void OnAwake()
        {
            if(initializeByDefault)
                Initialize();
        }

        public void Initialize()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            Hex.SetOverlaySide(overlaySide);
            Hex.SetOverlayTopPadding(overlayPadding);
            Hex.SubscribeToEvents(eventName => MistplayAnalyticsManager.GenericProgression("Hex", eventName));

            _ = Hex.Launch();
            DontDestroyOnLoad(this);
            #endif
        }
    }
}
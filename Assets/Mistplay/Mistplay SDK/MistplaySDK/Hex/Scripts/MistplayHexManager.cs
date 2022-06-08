using UnityEngine;

#if UNITY_ANDROID
using com.mistplay.hex;
#endif

namespace MistplaySDK
{
    public class MistplayHexManager : MonoBehaviour
    {
        [SerializeField] OverlaySide overlaySide;
        [SerializeField] int overlayPadding = 80;

        void Awake()
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
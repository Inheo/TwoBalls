using UnityEngine;
using GameAnalyticsSDK;

#if UNITY_IPHONE || UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace MistplaySDK
{
    public class MistplayPrivacyManager : Singleton<MistplayPrivacyManager>
    {
        [System.Serializable] public struct PartnerPrivacyPolicy { public string partner, link; }

        [SerializeField] PartnerPrivacyPolicy[] partnerPolicies;

        const string advertisingTrackingKey = "AdvertisingTracking";
        const string analyticsTrackingKey = "AnalyticsTracking";

        MistplayPrivacyUI ui;

        protected override void OnAwake()
        {
            ui = GetComponentInChildren<MistplayPrivacyUI>(true);
            ui.Initialize(partnerPolicies);

            MaxSdkCallbacks.OnSdkInitializedEvent += MaxSdkCallbacks_OnSdkInitializedEvent;
        }

        void MaxSdkCallbacks_OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            if(sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies
            && !MaxSdk.HasUserConsent()
            && GetAdvertisingTracking())
                ShowConsentDialog();
        }

        public bool GetAnalyticsTracking()
        {
            return PlayerPrefs.GetInt(analyticsTrackingKey, 1) == 1;
        }

        public bool GetAdvertisingTracking()
        {
            return PlayerPrefs.GetInt(advertisingTrackingKey, 1) == 1;
        }

        public void SetAnalyticsTrackingOn(bool isOn)
        {
            PlayerPrefs.SetInt(analyticsTrackingKey, isOn ? 1 : 0);
        }

        public void SetAdvertisingTrackingOn(bool isOn)
        {
            PlayerPrefs.SetInt(advertisingTrackingKey, isOn ? 1 : 0);
        }

        public void UpdateTracking()
        {
            if(GetAdvertisingTracking())
            {
                Debug.Log("GRANTING ADVERTISING CONSENT");
                GrantAdvertisingConsent();
            }
            else
            {
                Debug.Log("REVOKING ADVERTISING CONSENT");
                RevokeAdvertisingConsent();
            }

            if(GetAnalyticsTracking())
            {
                Debug.Log("GRANTING ANALYTICS CONSENT");
                GrantAnalyticsConsent();
            }
            else
            {
                Debug.Log("REVOKING ANALYTICS CONSENT");
                RevokeAnalyticsConsent();
            }
        }

        void ShowConsentDialog()
        {
            #if UNITY_IOS || UNITY_IPHONE
                Debug.Log("CHECKING IOS TRACKING PERMISSIONS");
                if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                {
                    Debug.Log("REQUESTING IOS TRACKING PERMISSIONS");
                    ATTrackingStatusBinding.RequestAuthorizationTracking();
                }
                else
                {
                    Debug.Log("IOS TRACKING PERMISSIONS HAVE ALREADY BEEN REQUESTED");
                }
            #else
                ui.ShowConsent();
            #endif
        }

        public void GrantAdvertisingConsent()
        {
            Debug.Log("GRANTING ADVERTISING CONSENT");
            #if mistplay_max_enabled
            MaxSdk.SetHasUserConsent(true);
            #endif
        }

        public void RevokeAdvertisingConsent()
        {
            Debug.Log("REVOKING ADVERTISING CONSENT");
            #if mistplay_max_enabled
            MaxSdk.SetHasUserConsent(false);
            #endif
        }

        public void GrantAnalyticsConsent()
        {
            Debug.Log("GRANTING ANALYTICS CONSENT");
            GameAnalytics.SetEnabledEventSubmission(true);
        }

        public void RevokeAnalyticsConsent()
        {
            Debug.Log("REVOKING ANALYTICS CONSENT");
            GameAnalytics.SetEnabledEventSubmission(false);
        }

        public void OpenMistplayPolicy()
        {
            Application.OpenURL("https://www.mistplay.com/mobile-games/privacy-policy");
        }

        public void OpenMailToSupport()
        {
            Application.OpenURL("mailto:suppport@mistplay.com?subject=Mistplay Studios GDPR");
        }
    }
}

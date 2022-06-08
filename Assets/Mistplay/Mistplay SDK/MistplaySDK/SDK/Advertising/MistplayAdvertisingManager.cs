using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if mistplay_appsflyer_enabled
using AppsFlyerSDK;
#endif
using System;
using System.Linq;
#if mistplay_gameanalytics_enabled
using GameAnalyticsSDK;
#endif

namespace MistplaySDK
{

#if mistplay_appsflyer_enabled
    public class MistplayAdvertisingManager : Singleton<MistplayAdvertisingManager>, IAppsFlyerConversionData
#else
    public class MistplayAdvertisingManager : Singleton<MistplayAdvertisingManager>
#endif
    {

        public delegate void InterstitialEventFinishedDelegate();
        public static event InterstitialEventFinishedDelegate InterstitialAdFinished;

        public delegate void RewardedVideoEventFinishedDelegate();
        public static event RewardedVideoEventFinishedDelegate RewardedVideoAdFinished;

        public delegate void RewardVideoAdEventRewardDelegate(RewardedVideoAdType type);
        public static event RewardVideoAdEventRewardDelegate RewardedVideoAdReward;

        public delegate void RewardedVideoEventLoadedDelegate();
        public static event RewardedVideoEventLoadedDelegate RewardedVideoLoaded;

        public string RewardedVideoAdUnitAndroid;
        public string InterstitialAdUnitAndroid;
        public string BannerAdUnitAndroid;

        public string AdMobAndroidAppID;

        public string RewardedVideoAdUnitIOS;
        public string InterstitialAdUnitIOS;
        public string BannerAdUnitIOS;

        public string AdMobIosAppID;

        [SerializeField] bool initializeByDefault = false;

        [Tooltip("Based on Mistplay's recommendations, we may or may not want to show banner ads in the game")]
        public bool ShouldShowBannerAds = true;

        [Tooltip("When you're first testing your implementation, set the debugger to true to show you whether everything is properly integrated.")]
        public bool ShouldShowDebugger = false;

        private string interstitialAdUnit;
        private string rewardedVideoAdUnit;
        private string bannerAdUnit;

        private Dictionary<RewardedVideoAdType, bool> rewards = new Dictionary<RewardedVideoAdType, bool>();

        // Fields for Appsflyer attribution
        //******************************//
        [DevKey] public string devKey;
        public string appID;
        public bool isDebug;
        public bool getConversionData;
        //******************************//

        public enum RewardedVideoAdType
        {
            ExtraCoins,
            CustomizationDiscount,
            SkipLevel,
            ShopCoins,
            Other
        }

        public class AdsInitialized { public bool Banner, Interstitial, Rewarded; }

/*
        public bool InterstitialAdIsReady
        {
            get;
            private set;
        }
*/

        private bool _shouldShowInterstitialAd;
        private bool ShouldShowInterstitialAd
        {
            get
            {
                return (_shouldShowInterstitialAd);
            }
            set
            {
                _shouldShowInterstitialAd = value;
            }
        }

        private int interstitialAdsRetryAttempt;
        private int rewardedVideoAdsRetryAttempt;
        private static float timeSinceLastInterstitialAd = 0f;
        private static float minTimeBetweenInterstitialAds = 30f;
        private const string MinTimeBetweenInterstitialAdsABTestKey = "minAdsTime";
        private const string BannerAdsABTestKey = "Banner_Ads";

        public AdsInitialized InitializedAds { get; private set; }

        void Start()
        {
            InitializedAds = new AdsInitialized();
            SetupInitialRewards();

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
                // AppLovin SDK is initialized, start loading ads

#if UNITY_IOS || UNITY_IPHONE
                if (MaxSdkUtils.CompareVersions(UnityEngine.iOS.Device.systemVersion, "14.5") != MaxSdkUtils.VersionComparisonResult.Lesser)
                {
                    // Note that App transparency tracking authorization can be checked via `sdkConfiguration.AppTrackingStatus` for Unity Editor and iOS targets
                    SetAdvertisingTrackingIOS(sdkConfiguration);

                    // 1. Set Facebook ATE flag here, THEN
                }
#endif

                // initialize Appsflyer
                SetupAppsflyerAttribution();

                // capture ILRD to send to Appsflyer or other platforms
                // Attach callbacks based on the ad format(s) you are using
                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
                MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
                MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;

                // 2. Load ads 
                InitializeRewardedAds();

                if(initializeByDefault)
                    InitializeAds();

                if (ShouldShowDebugger)
                {
                    // Show Mediation Debugger
                    MaxSdk.ShowMediationDebugger();
                }
#if mistplay_gameanalytics_enabled
                GameAnalyticsILRD.SubscribeMaxImpressions();
#endif
            };

            MaxSdk.SetSdkKey("O3fEsTvR-mIZptRTnPSobUSQ0Usahf6fKg7CgHWrl5T4uZH4xFPUTur_J9RWWT_T9fIIBC8d4TTnHZ3pFVGUWV");
          //  MaxSdk.SetUserId(PlayerID.ID);
            MaxSdk.InitializeSdk();
        }   

        public void InitializeAds()
        {
            InitializeInterstitialAds();
            if (ShouldShowBannerAds)
                InitializeBannerAds();
        }

        private void SetupInitialRewards()
        {
            rewards.Add(RewardedVideoAdType.ExtraCoins, false);
            rewards.Add(RewardedVideoAdType.ShopCoins, false);
            rewards.Add(RewardedVideoAdType.CustomizationDiscount, false);
            rewards.Add(RewardedVideoAdType.SkipLevel, false);
            rewards.Add(RewardedVideoAdType.Other, false);
        }

        private void FixedUpdate()
        {
            timeSinceLastInterstitialAd += Time.fixedUnscaledDeltaTime;

            if (timeSinceLastInterstitialAd >= minTimeBetweenInterstitialAds)
            {
                ShouldShowInterstitialAd = true;
            }
            else
            {
                ShouldShowInterstitialAd = false;
            }

        }

        #region Interstitial Ads

        public void ShowInterstitialAd()
        {
            if(!InitializedAds.Interstitial) return;
            
            //  if (MaxSdk.IsInterstitialReady(interstitialAdUnit) && ShouldShowInterstitialAd)
            // removed any timer dependency
            if (MaxSdk.IsInterstitialReady(interstitialAdUnit))
            {
                MaxSdk.ShowInterstitial(interstitialAdUnit);
                ShouldShowInterstitialAd = false;
            }
            else
            {
                if (InterstitialAdFinished != null)
                {
                    InterstitialAdFinished();
                }
                Debug.LogError("Max SDK Interstitial Ads are not yet ready.");
            }
        }
        private void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

            // Reset retry attempt
            interstitialAdsRetryAttempt = 0;
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

            interstitialAdsRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialAdsRetryAttempt));

            Invoke("LoadInterstitial", (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {

            Debug.LogError("Interstitial ad failed to display: " + errorInfo.AdLoadFailureInfo + " Error Code: " + errorInfo.Code);

            if (InterstitialAdFinished != null)
            {
                InterstitialAdFinished();
            }

            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            LoadInterstitial();

        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

            if (InterstitialAdFinished != null)
            {
                InterstitialAdFinished();
            }

            ShouldShowInterstitialAd = false;
            timeSinceLastInterstitialAd = 0f;

            // Interstitial ad is hidden. Pre-load the next ad.
            LoadInterstitial();
        }

        private void LoadInterstitial()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            if(InterstitialAdUnitAndroid != null)
            {
                interstitialAdUnit = InterstitialAdUnitAndroid;
                InitializedAds.Interstitial = true;
            }
            else
            {
                Debug.LogError("No Interstitial Ad Unit was entered for Android");
            }
#elif UNITY_IPHONE || UNITY_IOS
           if(InterstitialAdUnitIOS != null)
            {
                interstitialAdUnit = InterstitialAdUnitIOS;
                InitializedAds.Interstitial = true;
            }
            else
            {
                Debug.LogError("No Interstitial Ad Unit was entered for iOS");
            }
#else
Debug.Log("Not Running on iOS or Android so no ad network will be instantiated and ads will not work");
#endif

            if(interstitialAdUnit!=null && !interstitialAdUnit.Equals(""))
            {
                MaxSdk.LoadInterstitial(interstitialAdUnit);
                InitializedAds.Interstitial = true;
            }
            else
            {
                Debug.LogWarning("No Interstitial Ad Unit entered. Interstitial ads will not launch.");
            }
        }
        #endregion

        #region Rewarded Video Ads
        public void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first rewarded ad
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            if (RewardedVideoAdUnitAndroid != null)
            {
                rewardedVideoAdUnit = RewardedVideoAdUnitAndroid;
            }
            else
            {
                Debug.LogWarning("No Interstitial Ad Unit was entered for Android");
            }
#elif UNITY_IPHONE || UNITY_IOS
           if(RewardedVideoAdUnitIOS != null)
            {
                rewardedVideoAdUnit = RewardedVideoAdUnitIOS;
            }
            else
            {
                Debug.LogWarning("No Rewarded Video Ad Unit was entered for iOS");
            }
#else
Debug.Log("Not Running on iOS or Android so no ad network will be instantiated and ads will not work");
#endif

            if(rewardedVideoAdUnit!=null && !rewardedVideoAdUnit.Equals(""))
            {
                MaxSdk.LoadRewardedAd(rewardedVideoAdUnit);

                InitializedAds.Rewarded = true;
            }
            else
            {
                Debug.LogWarning("No Rewarded Video Ad Unit entered. Rewarded Video ads will not launch.");
            }

        }

        public bool IsRewardedVideoAdReady()
        {
            if (MaxSdk.IsInitialized() && InitializedAds.Rewarded)
            {
                return MaxSdk.IsRewardedAdReady(rewardedVideoAdUnit);
            }

            return false;
        }

        public void ShowRewardedVideoAd()
        {
            ShowRewardedVideoAd(RewardedVideoAdType.Other);
        }

        public void ShowRewardedVideoAd(RewardedVideoAdType type)
        {
            if(InitializedAds.Rewarded && MaxSdk.IsRewardedAdReady(rewardedVideoAdUnit))
            {
                MaxSdk.ShowRewardedAd(rewardedVideoAdUnit);
                rewards[type] = true;
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

            if(RewardedVideoLoaded != null)
            {
                RewardedVideoLoaded();
            }

            // Reset retry attempt
            rewardedVideoAdsRetryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

            rewardedVideoAdsRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedVideoAdsRetryAttempt));

            Invoke("LoadRewardedAd", (float)retryDelay);
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            if (RewardedVideoAdFinished != null)
            {
                RewardedVideoAdFinished();
            }
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            LoadRewardedAd();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (RewardedVideoAdFinished != null)
            {
                RewardedVideoAdFinished();
            }
            // Rewarded ad is hidden. Pre-load the next ad
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {

            RewardedVideoAdType type = rewards.FirstOrDefault(p => p.Value == true).Key;
           
            // The rewarded ad displayed and the user should receive the reward.
            if (RewardedVideoAdReward != null)
            {
                RewardedVideoAdReward(type);
            }
            else
            {
                Debug.LogError("Rewarded video ad reward is not being granted to the player since no one is listening to the event");
            }

            // after the reward is given, reset the value back to false
            rewards[type] = false;


        }


        #endregion

        #region Banner Ads

        public void InitializeBannerAds()
        {
#if UNITY_ANDROID
            if (BannerAdUnitAndroid != null)
            {
                bannerAdUnit = BannerAdUnitAndroid;
                InitializedAds.Banner = true;
            }
            else
            {
                Debug.LogError("No Banner Ad Unit was entered for Android");
            }
#elif UNITY_IPHONE || UNITY_IOS
           if(BannerAdUnitIOS != null)
            {
                bannerAdUnit = BannerAdUnitIOS;
                InitializedAds.Banner = true;
            }
            else
            {
                Debug.LogError("No Banner Ad Unit was entered for iOS");
            }
#else
Debug.Log("Not Running on iOS or Android so no ad network will be instantiated and banner ads will not work");
#endif

            if (bannerAdUnit != null && !bannerAdUnit.Equals(""))
            {
                // Banners are automatically sized to 320?50 on phones and 728?90 on tablets
                // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
                MaxSdk.CreateBanner(bannerAdUnit, MaxSdkBase.BannerPosition.BottomCenter);

                // Set background or background color for banners to be fully functional
                MaxSdk.SetBannerBackgroundColor(bannerAdUnit, Color.white);

                MaxSdk.ShowBanner(bannerAdUnit);
                InitializedAds.Banner = true;
            }
            else
            {
                Debug.LogWarning("Banner Ad Unit is empty, Banner ads will not display");
            }

              
        }

        public void ShowBannerAd()
        {
            MaxSdk.ShowBanner(bannerAdUnit);
        }

        public void HideBannerAd()
        {
            MaxSdk.HideBanner(bannerAdUnit);
        }

        #endregion

        #region Appsflyer Attribution Setup

        private void SetupAppsflyerAttribution()
        {
#if mistplay_appsflyer_enabled
            //  AppsFlyer.setCustomerUserId(PlayerID.ID);

#if UNITY_ANDROID && !UNITY_EDITOR
        AppsFlyerAndroid.setCollectAndroidID(true);
        AppsFlyerAndroid.setCollectIMEI(true);
        Debug.Log("Setting Android ID & IMEI Collection to True");
#endif

            // These fields are set from the editor so do not modify!
            //******************************//
            AppsFlyer.setIsDebug(isDebug);
            AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
            //******************************//



            AppsFlyer.startSDK();

          //  AppsFlyerAdRevenue.setIsDebug(isDebug);





#endif
        }




        // Mark AppsFlyer CallBacks
        public void onConversionDataSuccess(string conversionData)
        {
#if mistplay_appsflyer_enabled
            AppsFlyer.AFLog("didReceiveConversionData", conversionData);
            Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
            // add deferred deeplink logic here
#endif
        }

        public void onConversionDataFail(string error)
        {
#if mistplay_appsflyer_enabled
            AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
#endif
        }

        public void onAppOpenAttribution(string attributionData)
        {
#if mistplay_appsflyer_enabled
            AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
#endif
        }

        public void onAppOpenAttributionFailure(string error)
        {
#if mistplay_appsflyer_enabled
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
#endif
        }

#endregion

#region Facebook Privacy Settings
        /*
         * Facebook specific privacy metrics
         */

#if UNITY_IPHONE || UNITY_IOS
        public void SetAdvertisingTrackingIOS(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            Debug.Log("SDK CONFIGURATION CALLED, SETTING FACEBOOK FLAG");

            int trackingEventSent = PlayerPrefs.GetInt("IOS15TrackingEventSent", 0);

            if (sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized)
            {
                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);

#if mistplay_gameanalytics_enabled
                if(trackingEventSent == 0)
                {
                    GameAnalytics.NewDesignEvent("IOSTRACKING:ENABLED");
                }
#endif
            }
            else
            {
                if (sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Denied)
                    Debug.Log("STATUS FOR FAN ATT IS DENIED");
                else if (sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.NotDetermined)
                    Debug.LogError("STATUS FOR FAN ATT IS NOT DETERMINED");
                else if (sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Restricted)
                    Debug.Log("STATUS FOR FAN ATT IS RESTRICTED");
                else if(sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Unavailable)
                    Debug.LogError("STATUS FOR FAN ATT IS UNAVAILABLE");


                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(false);

#if mistplay_gameanalytics_enabled
                if (trackingEventSent == 0)
                {
                    GameAnalytics.NewDesignEvent("IOSTRACKING:DISABLED");
                }
#endif

            }

        }
#endif
#endregion


        #region Impression-Level User Revenue (ILRD) Data

        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            SendILRDToMistplayDB(adUnitId, adInfo);
        }

        private void SendILRDToMistplayDB(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            double revenue = adInfo.Revenue;

            // Miscellaneous data
            string applicationID = Application.identifier; // application id: On Apple platforms this is the 'bundleIdentifier' saved in the info.plist file, on Android it's the 'package' from the AndroidManifest.xml.
            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
            string userID = PlayerID.ID;

            //TODO: Send to DB


        }



#endregion


        private void OnDestroy()
        {

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;

            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;

        }

    }

}

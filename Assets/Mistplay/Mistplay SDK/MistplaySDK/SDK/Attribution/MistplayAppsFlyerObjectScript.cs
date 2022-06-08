 #if mistplay_appsflyer_enabled
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;
using System;
using MistplaySDK;

#if UNITY_ANDROID && mistplay_firebase_enabled
using Firebase.Messaging;
using Firebase.Unity;
#endif

namespace MistplaySDK
{


    // This class is intended to be used the the AppsFlyerObject.prefab

    public class MistplayAppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
    {


        // These fields are set from the editor so do not modify!
        //******************************//
        public string devKey;
        public string appID;
        public bool isDebug;
        public bool getConversionData;
        //******************************//

        private bool tokenSent;

        void Start()
        {

            //AppsFlyer.setCustomerUserId(PlayerID.ID);

            // These fields are set from the editor so do not modify!
            //******************************//
            AppsFlyer.setIsDebug(isDebug);
            AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
            //******************************//

            AppsFlyer.startSDK();


       //     AppsFlyerAdRevenue.setIsDebug(isDebug);

#if mistplay_mopub_enabled
            // track ad revenue data with MoPub 
 //           AppsFlyerAdRevenue.start(AppsFlyerAdRevenueType.MoPub);

     //       MoPubManager.OnImpressionTrackedEventBg += OnImpressionTrackedEventBg;
#endif
            //to measure uninstall with Appsflyer for Android we use Firebase Cloud Messaging
#if UNITY_ANDROID && mistplay_firebase_enabled
            //: UNCOMMENT THIS IF YOU INTEGRATE FIREBASE
            //   FirebaseManager.OnFireBaseInitialized += FireBaseInitialized;
#endif

            //to measure uninstall with Appsflyer for iOS
#if UNITY_IOS
          //  UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
#endif

        }

#if UNITY_ANDROID && mistplay_firebase_enabled
        private bool subscribedToUninstallEvent;
        private void FireBaseInitialized()
        {
            if (!subscribedToUninstallEvent) 
            { 
                //to measure uninstall with Appsflyer for Android
                //ARTOUR: UNCOMMENT THIS IF YOU INTEGRATE FIREBASE
                //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                subscribedToUninstallEvent = true;
            }
        }
#endif

/*
#if mistplay_mopub_enabled
        private void OnImpressionTrackedEventBg(string adUnitId, MoPub.ImpressionData impressionData)
        {


            if (impressionData.NetworkName != null)
            {
                if (impressionData.NetworkName.Equals("undisclosed"))
                {
                    Debug.Log("Sending Ad Revenue Data about Facebook");

                    Dictionary<string, string> adRevenueEvent = new Dictionary<string, string>();

                    // send the ad view event to Appsflyer specifically for Facebook
                    adRevenueEvent.Add("af_adrev_ad_type", impressionData.AdUnitFormat);
                    AppsFlyer.sendEvent("af_ad_view", adRevenueEvent);
                }
            }


        }
#endif
*/

        private void OnDestroy()
        {
#if mistplay_mopub_enabled
        //    MoPubManager.OnImpressionTrackedEventBg -= OnImpressionTrackedEventBg;
#endif
        }


        void Update()
        {
            /*          //to measure uninstall with Appsflyer for iOS
              #if UNITY_IOS
                     if (!tokenSent)
                      { 
                          byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
                          if (token != null)
                          {   
                              AppsFlyeriOS.registerUninstall(token);
                              tokenSent = true;
                          }
                      } 
              #endif
            */
        }


#if UNITY_ANDROID && mistplay_firebase_enabled
        //to measure uninstall with Appsflyer for Android
           public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
           {
                   AppsFlyerAndroid.updateServerUninstallToken(token.Token);
           }
#endif

        // Mark AppsFlyer CallBacks
        public void onConversionDataSuccess(string conversionData)
        {
            AppsFlyer.AFLog("didReceiveConversionData", conversionData);
            Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
            // add deferred deeplink logic here
        }

        public void onConversionDataFail(string error)
        {
            AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
        }

        public void onAppOpenAttribution(string attributionData)
        {
            AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
        }

        public void onAppOpenAttributionFailure(string error)
        {
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }
    }

}

#endif


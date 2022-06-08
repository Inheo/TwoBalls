using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;


namespace MistplaySDK
{ 
    
    public class MistplaySampleManager : MonoBehaviour
    {

        public Button RewardedVideoButton;
        public Text StatusText;

        // Start is called before the first frame update
        private void Start()
        {
            // You'll want to keep this object through different scene changes so that you're always handling advertising events
            DontDestroyOnLoad(this);

            /* 
             * HANDLE INTERSTITIAL ADS (FULL SCREEN) EVENTS: 
             */

            Debug.LogWarning("SAMPLE MANAGER REGISTERING FOR EVENTS");

            // register for interstitial dismissed event to know when the player has finished watching the interstitial ad
            //   MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

            MistplayAdvertisingManager.InterstitialAdFinished += OnInterstitialDismissedEvent;


            /* 
             * HANDLE REWARDED VIDEO EVENTS:
             */

            // register for this event to reward the player for watching a video ad
            MistplayAdvertisingManager.RewardedVideoAdReward += OnRewardedVideoReward;

            // register for this event to know when the player is finished watching the rewarded video ad so for example you can load the next level
            //  MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
            MistplayAdvertisingManager.RewardedVideoAdFinished += OnRewardedVideoFinishedEvent;

            // register for this event to know when a rewarded video ad is loaded, for example to update your button to enabled
            //  MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
            MistplayAdvertisingManager.RewardedVideoLoaded += OnRewardedVideoLoadedEvent;

            // register for this event in case
           // MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;

            // enable or disable the interaction with the rewarded video button depending on whether we have a rewarded video ready to be shown
            PrepareRewardedVideoButton();



            /*
             * BANNER ADS are automatically requested and shown by the MistplayAdvertisingManager
             * To Disable them, go to the Mistplay-Mopub Advertising Manager prefab
             */
        
        }

        private void PrepareRewardedVideoButton()
        {
            if (MistplayAdvertisingManager.Instance.IsRewardedVideoAdReady())
            {
                StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad is ready to play";
                // the rewarded video ad is ready, we can enable the button
                RewardedVideoButton.interactable = true;
            }
            else
            {
                StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad is not yet loaded";
                // Rewarded Video Ad is not yet ready so disable the rewarded video ad button
                // once the rewarded video is ready, OnRewardedVideoLoadedEvent will be called so you can re-enable the button with that event (see below)
                RewardedVideoButton.interactable = false;

            }
        }


#region ButtonClickHandlers

        /*
        * GAMEANALYTICS SECTION
        */

        public void SendLevelStartEvent()
        {
            //Send analytics for start of level with the name of the level (in this case we are using the scene name as the level name)
            GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, SceneManager.GetActiveScene().name);
            StatusText.text += "\n<b> GAMEANALYTICS: </b> Sent Level Start Event";
        }

        public void SendLevelCompletedEvent()
        {
            //Send analytics for completion of level with the name of the level (in this case we are using the scene name as the level name)
            GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, SceneManager.GetActiveScene().name);
            StatusText.text += "\n<b> GAMEANALYTICS: </b> Sent Level COMPLETE Event";
        }

        public void SendLevelFailedEvent()
        {
            //Send analytics for failure of level with the name of the level (in this case we are using the scene name as the level name)
            GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Fail, SceneManager.GetActiveScene().name);
            StatusText.text += "\n<b> GAMEANALYTICS: </b> Sent Level FAIL Event";
        }


       /*
       * ADVERTISING SECTION
       */

        public void ShowFullScreenAdButton()
        {
            // Show an interstitial Ad when the level completes
            MistplayAdvertisingManager.Instance.ShowInterstitialAd();
            StatusText.text += "\n<b> ADVERTISING: </b> Showing Fullscreen Ad";
        }

        public void ShowRewardedVideoAdButtonExtraCoins()
        {
            // Show a rewarded video ad that gives the player extra coins 
            MistplayAdvertisingManager.Instance.ShowRewardedVideoAd();
            StatusText.text += "\n<b> ADVERTISING: </b> Showing Rewarded Video Ad";
        }

        public void ShowBannerAdsButton()
        {
            MistplayAdvertisingManager.Instance.ShowBannerAd();
            StatusText.text += "\n<b> ADVERTISING: </b> Showing Banner Ad";
        }

        public void DisableBannerAdsButton()
        {
            MistplayAdvertisingManager.Instance.HideBannerAd();
            StatusText.text += "\n<b> ADVERTISING: </b> Hiding Banner Ad";
        }


#endregion


        /* Interstitial dismissed event will let you know when the player has finished watching the interstitial ad
         * For example, at the end of a level, after showing the interstitial ad,
         * this is where you could change levels to load the next one (if level succesfully completed) or retry this one (if level failed).
         */
        private void OnInterstitialDismissedEvent()
        {
            // interstitial (full screen) ad was shown
            // Load next level
            StatusText.text += "\n<b> ADVERTISING: </b> Fullscreen Ad Finished, should load next level";
        }



        /*
         * Rewarded video is completed so we need to reward the player based on whatever rewarded video type they watched
         * For example, we may want to give them extra coins, or we may want to skip a level 
         *
         */
        private void OnRewardedVideoReward(MistplayAdvertisingManager.RewardedVideoAdType type)
        {
            switch (type)
            {
                case MistplayAdvertisingManager.RewardedVideoAdType.ExtraCoins:
                    // AWARD PLAYER EXTRA COINS
                    StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad Finished, reward player with coins!";
                    break;
                case MistplayAdvertisingManager.RewardedVideoAdType.CustomizationDiscount:
                    // DISCOUNT ON SKINS
                    break;
                case MistplayAdvertisingManager.RewardedVideoAdType.SkipLevel:
                    // SKIP A LEVEL
                    break;
                case MistplayAdvertisingManager.RewardedVideoAdType.ShopCoins:
                    // AWARD PLAYER COINS
                    break;
                case MistplayAdvertisingManager.RewardedVideoAdType.Other:
                    // AWARD SOMETHING ELSE
                    break;
            }
        }
        

        private void OnRewardedVideoFinishedEvent()
        {
            StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad Finished, load next level";
        }

        /*
         * Event called when rewarded video is loaded and ready to be played
         * We can for example enable a rewarded video ad button now that the video is ready
         */
        private void OnRewardedVideoLoadedEvent()
        {
            if (RewardedVideoButton != null)
            {
                StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad is now ready to play";
                RewardedVideoButton.interactable = true;
            }
        }

        private void OnRewardedVideoFailedToPlayEvent(string adUnitID, string errorMessage)
        {
            // Rewarded Video failed to play for whatever reason
            // If for example this is at the end of a level, we can just skip to the next level here.
            StatusText.text += "\n<b> ADVERTISING: </b> Rewarded Video Ad failed to play";
        }




        private void OnDestroy()
        {
            // Unsubscribe from events

            MistplayAdvertisingManager.InterstitialAdFinished -= OnInterstitialDismissedEvent;


            /* 
             * HANDLE REWARDED VIDEO EVENTS:
             */

            // register for this event to reward the player for watching a video ad
            MistplayAdvertisingManager.RewardedVideoAdReward -= OnRewardedVideoReward;

            // register for this event to know when the player is finished watching the rewarded video ad so for example you can load the next level
            //  MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
            MistplayAdvertisingManager.RewardedVideoAdFinished -= OnRewardedVideoFinishedEvent;

            // register for this event to know when a rewarded video ad is loaded, for example to update your button to enabled
            //  MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
            MistplayAdvertisingManager.RewardedVideoLoaded -= OnRewardedVideoLoadedEvent;


        }


        
    }


}

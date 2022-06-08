using System;
using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using Google.Play.Review;
using Google.Play.Common;
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace MistplaySDK
{
    public class MistplayReviewsManager : Singleton<MistplayReviewsManager>
    {
        public const string ReviewedKey = "MRM_Reviewed";

        [Tooltip("The minimum required rating to show the player the store prompt")] [SerializeField] [Range(0, 5)] int MinimumRating;

        MistplayReviewPrompt prompt;

        protected override void OnAwake()
        {
            prompt = GetComponentInChildren<MistplayReviewPrompt>(true);
        }

        public void Show()
        {
            prompt.Show(OnReview, OnCancel);
        }

        void OnReview(int rating)
        {
            if(rating >= MinimumRating)
            {
                OpenGooglePrompt();
                OpenIOSPrompt();
            }
            else MistplayFeedbackManager.Instance.Show();
        }

        void OnCancel()
        {

        }

        void OpenGooglePrompt()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR

            var reviewManager = new ReviewManager();
            var task = reviewManager.RequestReviewFlow();

            task.Completed += info =>
            {
                if (info.Error == ReviewErrorCode.NoError)
                {
                    var result = reviewManager.LaunchReviewFlow(info.GetResult());
                    StartCoroutine(WaitForReview(result));
                }
                else
                {
                    Debug.LogError("REVIEW PROMPT: " + info.Error.ToString());
                }
            };

            #endif
        }

        IEnumerator WaitForReview(PlayAsyncOperation<VoidResult, ReviewErrorCode> result)
        {
            while(!result.IsDone) yield return null;
            if(result.IsSuccessful) PlayerPrefs.SetInt(ReviewedKey, 1);
        }

        void OpenIOSPrompt()
        {
            #if UNITY_IOS && !UNITY_EDITOR

            if(!Device.RequestStoreReview())
                Debug.LogError("COULD NOT OPEN REVIEW PROMPT");
            
            #endif
        }
    }
}
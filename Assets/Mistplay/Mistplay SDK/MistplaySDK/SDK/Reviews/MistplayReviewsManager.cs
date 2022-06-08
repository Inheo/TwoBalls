using System;
using UnityEngine;

#if UNITY_ANDROID
using Google.Play.Review;
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace MistplaySDK
{
    public class MistplayReviewsManager : MonoBehaviour
    {
        [Tooltip("The minimum required rating to show the player the store prompt")] [SerializeField] [Range(0, 5)] int MinimumRating;

        MistplayReviewPrompt prompt;
        public static MistplayReviewsManager Instance { get; private set; }

        void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
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
                    reviewManager.LaunchReviewFlow(info.GetResult());
                }
                else
                {
                    Debug.LogError("REVIEW PROMPT: " + info.Error.ToString());
                }
            };

            #endif
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
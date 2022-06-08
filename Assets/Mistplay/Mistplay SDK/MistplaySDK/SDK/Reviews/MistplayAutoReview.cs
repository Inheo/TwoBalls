using UnityEngine;

namespace MistplaySDK
{
    class MistplayAutoReview : MonoBehaviour
    {
        [SerializeField] int startShowingDay = 1;
        [SerializeField] int showsEveryNDays = 2;
        [SerializeField] float showsAfterNSeconds = 120;

        void Awake()
        {
            MistplaySessionManager.Instance.AddEvent(ShowPrompt);
        }

        bool ShowPrompt(MistplaySessionManager.Context context)
        {
            if(context.Day >= startShowingDay
            && (context.Day - startShowingDay) % showsEveryNDays == 0
            && context.SessionDuration >= showsAfterNSeconds)
            {
                MistplayReviewsManager.Instance.Show();
                return true;
            }

            return false;
        }
    }
}
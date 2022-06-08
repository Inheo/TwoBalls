using UnityEngine;
using UnityEngine.UI;

namespace MistplaySDK
{
    class MistplayPrivacyUI : MonoBehaviour
    {
        [SerializeField] GameObject consentForm;
        [SerializeField] GameObject manageDataSettings;
        [SerializeField] GameObject fixSettings;

        [Space]
        [SerializeField] Toggle analytics;
        [SerializeField] Toggle advertising;
        [SerializeField] Button policyButton;
        [Space]
        [SerializeField] Toggle age16, agree;
        [SerializeField] Image playImage;
        [SerializeField] Color playColor, lockedPlayColor;

        bool initialized;

        void Start()
        {
            UpdateToggles();

            analytics.onValueChanged.AddListener((isOn) => MistplayPrivacyManager.Instance.SetAnalyticsTrackingOn(isOn));
            advertising.onValueChanged.AddListener((isOn) => MistplayPrivacyManager.Instance.SetAdvertisingTrackingOn(isOn));
        
            age16.onValueChanged.AddListener(UpdatePlayButton);
            agree.onValueChanged.AddListener(UpdatePlayButton);

            playImage.color = lockedPlayColor;
        }

        public void Initialize(MistplayPrivacyManager.PartnerPrivacyPolicy[] policies)
        {
            if(!initialized)
            {
                for(var i = 0; i < policies.Length; ++i)
                {
                    var button = Instantiate(policyButton, Vector3.zero, Quaternion.identity, policyButton.transform.parent);
                    var url = policies[i].link;
                    button.onClick.AddListener(() => Application.OpenURL(url));
                    button.GetComponentInChildren<Text>().text = policies[i].partner + " Privacy Policy";
                    button.gameObject.SetActive(true);
                }

                initialized = true;
            }
        }

        public void ShowConsent()
        {
            Debug.LogWarning("SHOWING CONSENT PROMPT");
            gameObject.SetActive(true);
            consentForm.SetActive(true);
        }

        public void ShowPrivacySettings()
        {
            HideCategories();
            gameObject.SetActive(true);
            manageDataSettings.SetActive(true);
        }

        void HideCategories()
        {
            consentForm.SetActive(false);
            manageDataSettings.SetActive(false);
            fixSettings.SetActive(false);
        }

        void Hide()
        {
            HideCategories();
            gameObject.SetActive(false);
        }

        void UpdatePlayButton(bool _)
        {
            playImage.color = age16.isOn && agree.isOn ? playColor : lockedPlayColor;
        }

        public void OnAcceptTracking()
        {
            if(age16.isOn && agree.isOn)
            {
                MistplayPrivacyManager.Instance.GrantAdvertisingConsent();
                MistplayPrivacyManager.Instance.GrantAnalyticsConsent();
                Hide();
            }
        }

        public void OnCloseSettings()
        {
            if(MistplayPrivacyManager.Instance.GetAnalyticsTracking() && MistplayPrivacyManager.Instance.GetAdvertisingTracking())
            {
                MistplayPrivacyManager.Instance.GrantAdvertisingConsent();
                MistplayPrivacyManager.Instance.GrantAnalyticsConsent();
                Hide();
            }
            else
            {
                HideCategories();
                fixSettings.SetActive(true);
            }
        }

        public void OnSetFinalSettings()
        {
            Hide();
            MistplayPrivacyManager.Instance.UpdateTracking();
        }

        void UpdateToggles()
        {
            analytics.isOn = MistplayPrivacyManager.Instance.GetAnalyticsTracking();
            advertising.isOn = MistplayPrivacyManager.Instance.GetAdvertisingTracking();
        }
    }
}
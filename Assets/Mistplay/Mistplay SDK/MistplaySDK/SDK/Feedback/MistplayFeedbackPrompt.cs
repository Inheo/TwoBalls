using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace MistplaySDK
{
    class MistplayFeedbackPrompt : MonoBehaviour
    {
        [SerializeField] Button validate, cancel;
        [SerializeField] Dropdown dropdown;
        [SerializeField] InputField summary, description;
        [Space]
        [SerializeField] float fadeInDuration;
        [SerializeField] AnimationCurve fadeInBounce;
        [SerializeField] float fadeOutDuration;
        [SerializeField] AnimationCurve fadeOutBounce;
        
        CanvasGroup group;

        Action<string> onValidate;
        
        int rating;

        void Awake()
        {
            validate.onClick.AddListener(Validate);
            cancel.onClick.AddListener(Cancel);
            group = GetComponent<CanvasGroup>();
        }

        public void Show(Action<string> onValidate)
        {
            this.onValidate = onValidate;

            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(Bounce(transform.GetChild(0), fadeInBounce, fadeInDuration));
            StartCoroutine(Fade(group, fadeInDuration, 0, 1));

            description.Select();
        }

        void Hide()
        {
            StartCoroutine(Bounce(transform.GetChild(0), fadeOutBounce, fadeOutDuration));
            StartCoroutine(Fade(group, fadeOutDuration, 1, 0, () => gameObject.SetActive(false)));
        }

        void Validate()
        {
            onValidate(description.text);
            Hide();
        }

        void Cancel()
        {
            Hide();
        }

        IEnumerator Bounce(Transform element, AnimationCurve curve, float duration, Action callback = null)
        {
            var start = Time.time;
            while(Time.time - start < duration)
            {
                var interpolation = (Time.time - start) / duration;
                element.localScale = Vector3.one * curve.Evaluate(interpolation);
                yield return null;
            }

            element.localScale = Vector3.one * curve.Evaluate(1);

            if(callback != null) callback();
        }

        IEnumerator Fade(CanvasGroup group, float duration, float from, float to, Action callback = null)
        {
            var start = Time.time;
            while(Time.time - start < duration)
            {
                var interpolation = (Time.time - start) / duration;
                group.alpha = Mathf.Lerp(from, to, interpolation);
                yield return null;
            }

            group.alpha = to;

            if(callback != null) callback();
        }
    }
}
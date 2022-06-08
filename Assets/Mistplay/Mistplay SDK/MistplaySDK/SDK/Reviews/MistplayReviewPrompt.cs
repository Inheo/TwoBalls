using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace MistplaySDK
{
    class MistplayReviewPrompt : MonoBehaviour
    {
        [SerializeField] Button validate, cancel;
        [SerializeField] Button[] stars;
        [Space]
        [SerializeField] float fadeInDuration;
        [SerializeField] AnimationCurve fadeInBounce;
        [SerializeField] float fadeOutDuration;
        [SerializeField] AnimationCurve fadeOutBounce;
        [Space]
        [SerializeField] AnimationCurve starsBounce;
        [SerializeField] float starsBounceDuration;
        [SerializeField] Sprite starsSprite, starsSelectedSprite;
        [SerializeField] Color starsFaded, starsSelected;
        
        CanvasGroup group;

        Action<int> onValidate;
        Action onCancel;
        
        int rating;

        void Awake()
        {
            validate.onClick.AddListener(Validate);
            cancel.onClick.AddListener(Cancel);

            for(int i = 0; i < stars.Length; ++i)
            {
                var rating = i + 1;
                stars[i].onClick.AddListener(() => Set(rating));
                stars[i].GetComponent<Image>().color = starsFaded;
            }

            group = GetComponent<CanvasGroup>();
        }

        public void Show(Action<int> onValidate, Action onCancel)
        {
            this.onValidate = onValidate;
            this.onCancel = onCancel;

            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(Bounce(transform.GetChild(0), fadeInBounce, fadeInDuration));
            StartCoroutine(Fade(group, fadeInDuration, 0, 1));
        }

        void Hide()
        {
            StartCoroutine(Bounce(transform.GetChild(0), fadeOutBounce, fadeOutDuration));
            StartCoroutine(Fade(group, fadeOutDuration, 1, 0, () => gameObject.SetActive(false)));
        }

        void Validate()
        {
            onValidate(rating);
            Hide();
        }

        void Cancel()
        {
            onCancel();
            Hide();
        }

        void Set(int rating)
        {
            this.rating = rating;
            for(int i = 0; i < stars.Length; ++i)
            {
                var img = stars[i].GetComponent<Image>();
                if(i >= rating)
                {
                    img.color = starsFaded;
                    img.sprite = starsSprite;
                }
                else
                {
                    img.color = starsSelected;
                    img.sprite = starsSelectedSprite;
                }
            }

            stars[rating - 1].StopAllCoroutines();
            stars[rating - 1].StartCoroutine(Bounce(stars[rating - 1].transform, starsBounce, starsBounceDuration));
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
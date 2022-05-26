using Scripts.Data;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private SwipeInput _swipeInput;

    private UIAnimation[] _uiAnimations; 

   private void Start()
   {
       if(PlayerProgress.GetData().IsSwipeTutorialComplete == true)
       {
           gameObject.SetActive(false);
           return;
       }

       _uiAnimations = GetComponentsInChildren<UIAnimation>();

       for (var i = 0; i < _uiAnimations.Length; i++)
       {
           _uiAnimations[i].PlayAnimation();
       }

       _swipeInput.OnSwipeHorizontal += OnSwipe;
   } 

   private void OnDestroy()
   {
       _swipeInput.OnSwipeHorizontal -= OnSwipe;
   }

   private void OnSwipe(int direction)
   {
       PlayerProgress.GetData().SwipeTutorialCompleted();

       gameObject.SetActive(false);
   }
}
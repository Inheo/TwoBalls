using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace com.mistplay.hex
{
    /// <summary>
    /// The Hex Unity wrapper class
    /// </summary>
    public sealed class Hex
    {
        private static Hex Instance { get { return lazy.Value; } }
        private static readonly Lazy<Hex> lazy = new Lazy<Hex>(() => new Hex());

        private AndroidJavaObject hex;
        private AndroidJavaClass player;
        private AndroidJavaObject activity;

        private Hex()
        {
            hex = new AndroidJavaObject("com.mistplay.hex.Hex");
            player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        }

        /// <summary>
        /// Launch Hex
        /// </summary>
        /// <returns>Boolean if the user is eligible for the SDK</returns>
        public static async Task<bool> Launch()
        {
            return await Instance.LaunchInternal();
        }

        private async Task<bool> LaunchInternal()
        {
            // Setup the TaskCompletionSource to convert the callback as an async function
            var taskCompletion = new TaskCompletionSource<bool>();
            var cancellationToken = new CancellationTokenSource(millisecondsDelay: 10000);
            cancellationToken.Token.Register(() => taskCompletion.TrySetResult(false), useSynchronizationContext: false);

            // Call the android function with the C# callback
            var launchCallback = new LaunchCallback(taskCompletion);
            activity.Call("launch", launchCallback);
            return await taskCompletion.Task;
        }

        private class LaunchCallback : AndroidJavaProxy
        {
            private readonly TaskCompletionSource<bool> taskCompletion;

            public LaunchCallback(TaskCompletionSource<bool> taskCompletion) : base("com.mistplay.hex.Hex$OnCompletionCallback")
            {
                this.taskCompletion = taskCompletion;
            }

            public void onCompletion(bool isSuccess)
            {
                taskCompletion.TrySetResult(isSuccess);
            }
        }

        /// <summary>
        /// Subscribe to analytics events fired by Hex
        /// </summary>
        public static void SubscribeToEvents(Action<string> onEventAction)
        {
            Instance.SubscribeToEventsInternal(onEventAction);
        }

        private void SubscribeToEventsInternal(Action<string> onEventAction)
        {
            var eventCallback = new EventCallback(onEventAction);
            hex.Call("subscribeToEvents", eventCallback);
        }

        private class EventCallback : AndroidJavaProxy
        {
            private readonly Action<string> onEventAction;

            public EventCallback(Action<string> onEventAction) : base("com.mistplay.hex.Hex$OnEventCallback")
            {
                this.onEventAction = onEventAction;
            }

            public void onEvent(string eventName)
            {
                onEventAction?.Invoke(eventName);
            }
        }

        /// <summary>
        /// Close Hex and remove its reward overlay
        /// </summary>
        public static void Close()
        {
            Instance.CloseInternal();
        }

        private void CloseInternal()
        {
            activity.Call("hideHex");
        }

        /// <summary>
        /// Set on which side on the screen the Hex layout will be shown
        /// </summary>
        /// <param name="side">The side of the screen, being left or right</param>
        public static void SetOverlaySide(OverlaySide side)
        {
            Instance.SetOverlaySideInternal(side);
        }

        private void SetOverlaySideInternal(OverlaySide side)
        {
            hex.Call("setOverlaySide", activity, side.ToString());
        }

        /// <summary>
        /// Set the top padding to apply to the Hex layout to avoid interfering with existing UI
        /// </summary>
        /// <param name="topPadding">The top padding, as DP</param>
        public static void SetOverlayTopPadding(int topPadding)
        {
            Instance.SetOverlayTopPaddingInternal(topPadding);
        }

        private void SetOverlayTopPaddingInternal(int topPadding)
        {
            hex.Call("setOverlayTopPadding", activity, topPadding);
        }
    }

    public enum OverlaySide
    {
        left,
        right
    }
}
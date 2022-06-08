using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace MistplaySDK
{



    public class FacebookAdditionalFlag : MonoBehaviour
    {

        /*
    #if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _sendFacebookData(int isEnabled);

        
        void OnSdkConfiguration(MoPub.SdkConfiguration config)
        {
            SendFacebookData(1);
        }
    #endif
        */

        /*
         * We are using a native plugin to set the Facebook flag in Objective C, as such we need to send an int instead of a bool
         * if isEnabled is set to 1, the flag will be set to TRUE, otherwise it will be set to FALSE
         */

        /*
        static void SendFacebookData(int isEnabled)
        {
       
    #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _sendFacebookData(isEnabled);
            }
    #endif

        }

        */
    }

}


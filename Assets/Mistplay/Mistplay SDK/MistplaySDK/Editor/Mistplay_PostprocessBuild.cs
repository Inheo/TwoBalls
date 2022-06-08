using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEngine;

namespace MistplaySDK
{
#if UNITY_2018_1_OR_NEWER
    public class Mistplay_PostprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
#else
    public class Mistplay_PostprocessBuild
#endif
    {
        private static string mistplay_mopub = "mistplay_mopub_enabled";
        private static string mistplay_gameanalytics = "mistplay_gameanalytics_enabled";
        private static string mistplay_appsflyer = "mistplay_appsflyer_enabled";
        private static string mistplay_firebase = "mistplay_firebase_enabled";
        private static string mistplay_max = "mistplay_max_enabled";
        private static string mistplay_facebook = "mistplay_facebook_disabled";

#if UNITY_2018_1_OR_NEWER
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            Update3rdPartyIntegrations();
        }
#endif

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Update3rdPartyIntegrations();
        }

        private static void Update3rdPartyIntegrations()
        {
            UpdateGameAnalytics();
            UpdateMoPub();
            UpdateAppsflyer();
            UpdateFirebase();
            UpdateMax();
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_mopub_enabled` to true if MoPub classes are detected within the Unity project
        /// </summary>
        private static void UpdateMoPub()
        {
            var mopubTypes = new string[] { "MoPubBase", "MoPubManager" };
            if (TypeExists(mopubTypes))
            {
                UpdateDefines(mistplay_mopub, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(mistplay_mopub, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_gameanalytics_enabled` to true if MoPub classes are detected within the Unity project
        /// </summary>
        private static void UpdateGameAnalytics()
        {
            var gameAnalyticsTypes = new string[] { "GameAnalyticsSDK.GameAnalytics" };
            if (TypeExists(gameAnalyticsTypes))
            {
                UpdateDefines(mistplay_gameanalytics, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
            else
            {
                UpdateDefines(mistplay_gameanalytics, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_appsflyer_enabled` to true if Appsflyer classes are detected within the Unity project
        /// </summary>
        private static void UpdateAppsflyer()
        {
            var appsflyerTypes = new string[] { "AppsFlyerSDK", "AppsFlyer", "AppsFlyerSDK.AppsFlyer" };
            if (TypeExists(appsflyerTypes))
            {
                UpdateDefines(mistplay_appsflyer, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
            else
            {
                UpdateDefines(mistplay_appsflyer, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_appsflyer_enabled` to true if Appsflyer classes are detected within the Unity project
        /// </summary>
        private static void UpdateFirebase()
        {
            var firebaseTypes = new string[] { "Firebase" };
            if (TypeExists(firebaseTypes))
            {
                UpdateDefines(mistplay_firebase, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
            else
            {
                UpdateDefines(mistplay_firebase, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone });
            }
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_max_enabled` to true if Max classes are detected within the Unity project
        /// </summary>
        private static void UpdateMax()
        {
            var maxTypes = new string[] { "MaxSdk"};
            if (TypeExists(maxTypes))
            {
                UpdateDefines(mistplay_max, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(mistplay_max, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
        }

        /// <summary>
        /// Sets the scripting define symbol `mistplay_facebook_enabled` to true if Facebook classes are detected within the Unity project
        /// </summary>
        private static void UpdateFacebook()
        {
            var facebookTypes = new string[] { "FB", "Facebook", "Facebook.Unity", "Facebook.Unity.FB" };
            if (TypeExists(facebookTypes))
            {
                UpdateDefines(mistplay_facebook, true, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }
            else
            {
                UpdateDefines(mistplay_facebook, false, new BuildTargetGroup[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
            }

        }


        private static void UpdateDefines(string entry, bool enabled, BuildTargetGroup[] groups)
        {
            foreach (var group in groups)
            {
                var defines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                var edited = false;
                if (enabled && !defines.Contains(entry))
                {
                    defines.Add(entry);
                    edited = true;
                }
                else if (!enabled && defines.Contains(entry))
                {
                    defines.Remove(entry);
                    edited = true;
                }
                if (edited)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines.ToArray()));
                }
            }
        }

        private static bool TypeExists(params string[] types)
        {
            if (types == null || types.Length == 0)
                return false;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (types.Any(type => assembly.GetType(type) != null))
                    return true;
            }

            return false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MistplaySDK;

[CustomEditor(typeof(MistplayAdvertisingManager))]
public class MistplayAdvertisingManagerEditor : Editor
{
    Layout.Category[] categories;
    bool debug;

    Texture iconAndroid; 
    Texture iconIOS;
    Texture iconBanner;
    Texture iconBug;
    Texture iconAppsflyer;

    void OnEnable() 
    { 
        iconAndroid     = EditorGUIUtility.IconContent("BuildSettings.Android On").image;
        iconIOS         = Resources.Load("Icon_iOS") as Texture;
        iconBanner      = Resources.Load("Icon_Banner") as Texture;
        iconBug         = Resources.Load("Icon_Bug") as Texture;
        iconAppsflyer   = Resources.Load("Icon_AppsFlyer") as Texture;

        categories = new Layout.Category[5]
        {
            new Layout.Category("Android Ad Units", iconAndroid, 
            "RewardedVideoAdUnitAndroid", 
            "InterstitialAdUnitAndroid", 
            "BannerAdUnitAndroid", 
            "AdMobAndroidAppID"),

            new Layout.Category("iOS Ad Units", iconIOS, 
            "RewardedVideoAdUnitIOS", 
            "InterstitialAdUnitIOS", 
            "BannerAdUnitIOS", 
            "AdMobIosAppID"),
            
            new Layout.Category("Banner Ads", iconBanner, "ShouldShowBannerAds"),
            new Layout.Category("Debugger", iconBug, "ShouldShowDebugger"),
            new Layout.Category("AppsFlyer Attribution", iconAppsflyer, "devKey", "appID", "isDebug", "getConversionData")
        };

        for(int i = 0; i < categories.Length; i++)
        {
            float t = (float) i / (float) (categories.Length - 1);
            categories[i].color = Colors.MistplayCyan; // Color.Lerp(Colors.MistplayCyan, Colors.MistplayBlue, t);
        }
        
        var manager = ((MistplayAdvertisingManager) target);
        manager.AdMobAndroidAppID = AppLovinSettings.Instance.AdMobAndroidAppId;
        manager.AdMobIosAppID = AppLovinSettings.Instance.AdMobIosAppId;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        if(Layout.DrawMistplayHeader("Advertising", Colors.MistplayCyan)) return;

        foreach(var c in categories)
        {
            c.Draw(serializedObject);
            Layout.Space(1);
        }

        var manager = ((MistplayAdvertisingManager) target);
        AppLovinSettings.Instance.AdMobAndroidAppId = manager.AdMobAndroidAppID;
        AppLovinSettings.Instance.AdMobIosAppId = manager.AdMobIosAppID;

        if(serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }

        Layout.Footer();
        Layout.RepaintIfMouseHover(this);
    }
}
